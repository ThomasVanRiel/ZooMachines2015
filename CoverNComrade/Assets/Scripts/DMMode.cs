using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DMMode : MonoBehaviour, GameMode {
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
		if (!_players.ContainsKey(killer)) {
			Debug.LogWarningFormat("[DM] Killer {0} is unknown to this game", killer);
			return;
		}

		if (!_players.ContainsKey(killed)) {
			Debug.LogWarningFormat("[DM] Killed {0} is unknown to this game", killed);
			return;
		}

		// Get the killer player
		int killerID = _players[killer];
		int killedID = _players[killed];
	
		if (killerID == killedID) {
			// if it was suicide
			_scores[killerID] -= 1;
			Debug.LogFormat("Player {0} lost one point for killing himself!", killedID);
		} else {
			// otherwise, give it one point
			_scores[killerID] += 1;
			Debug.LogFormat("Player {0} got one point for killing player {1}!", killerID, killedID);
		}

		// determine if the killer is the winner, and there are still no winner
		if (_scores[killerID] >= _killsToWin && _winner != -1)
			_winner = killerID;

		_players.Remove(killed);
		StartCoroutine(RespawnPlayerIn(killedID, _timeToRespawn));
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

		Debug.LogFormat("Respawning player {0} after {1}", id, t);
		_players[_gm.SpawnPlayer(id)] = id;
	}
}
