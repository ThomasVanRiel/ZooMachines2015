using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DMMode : GameMode {
	private int _killsToWin;
	private float _timeToRespawn;
	private Dictionary<PlayerController, int> _players;
	private Dictionary<int, int> _scores;
	private int _winner = -1;

	public DMMode(int ktw) {
		_killsToWin = ktw;
		_timeToRespawn = 3.0f;
	}

	public void Setup(GameManager gm, int nbPlayer) {
		_players = new Dictionary<PlayerController, int> ();
		_scores = new Dictionary<int, int> ();

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

		// Get the killer player
		int killerID = _players[killer];
	
		if (killer == killed)
			// if it was suicide
			_scores[killerID] -= 1;
		else
			// otherwise, give it one point
			_scores[killerID] += 1;

		// determine if the killer is the winner, and there are still no winner
		if (_scores[killerID] >= _killsToWin && _winner != -1)
			_winner = killerID;

		// TODO: make the killed respawn after a given time
	}

	// Returns whether or not the game is over
	public bool IsGameOver() {
		return _winner != -1;
	}

	// Get the id of the winning player
	public int Winner() {
		return _winner;
	}
}
