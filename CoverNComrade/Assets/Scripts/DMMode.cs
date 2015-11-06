using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DMMode : GameMode {
	private int _killsToWin;
	private float _timeToRespawn;
	private Dictionary<PlayerController, int> _players;
	private Dictionary<int, int> _scores;
	private int _winner = -1;
	private GameManager _gm;

	public DMMode(int ktw) {
		_killsToWin = ktw;
		_timeToRespawn = 3.0f;
	}

	public void Setup(GameManager gm, int nbPlayer) {
		_players = new Dictionary<PlayerController, int> ();
		_scores = new Dictionary<int, int> ();
		_gm = gm;

		for (int i = 0; i < nbPlayer; i++) {
			_players.Add(gm.SpawnPlayer(i), i);
			_scores.Add(i, 0);
		}
	}

	public void PlayerKilled(PlayerController killer, PlayerController killed) {
		if (killer != null && _players.ContainsKey(killer)) {
			int killerID = _players[killer];

			if (killer == killed) {
				// if it was suicide
				_scores[killerID] -= 1;
				Debug.LogFormat("Player {0} lost one point for killing himself!", killerID);
			} else {
				// otherwise, give it one point
				_scores[killerID] += 1;
				Debug.LogFormat("Player {0} got one point!", killerID);
			}

			// determine if the killer is the winner, and there are still no winner
			if (_scores[killerID] >= _killsToWin && _winner != -1)
				_winner = killerID;
		}

		if (killed != null && _players.ContainsKey(killed)) {
			int killedID = _players[killed];

			_players.Remove(killed);
			GameManager.Command cmd = () => { return RespawnPlayerIn(killedID, _timeToRespawn); };
			_gm.Execute(cmd);
		}
	}

	// Returns whether or not the game is over
	public bool IsGameOver() {
		return _winner != -1;
	}

	// Get the id of the winning player
	public int Winner() {
		return _winner;
	}

	private IEnumerator RespawnPlayerIn(int id, float t) {
		yield return new WaitForSeconds(t);

		Debug.LogFormat("Respawning player {0} after {1}", id);
		_players[_gm.SpawnPlayer(id)] = id;
	}
}
