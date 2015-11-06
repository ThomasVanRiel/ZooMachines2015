using UnityEngine;
using System.Collections.Generic;

public class LMSMode : GameMode {
	private Dictionary<PlayerController, int> _players;

	public void Setup(GameManager gm, int nbPlayers) {
		_players = new Dictionary<PlayerController, int>();

		for (int i = 0; i < nbPlayers; i++) {
			_players.Add(gm.SpawnPlayer(i), i);
		}
	}

	public void PlayerKilled(PlayerController killer, PlayerController killed) {
		if (killed != null && _players.ContainsKey(killed))
			_players.Remove(killed);
		else
			Debug.Log("[LMS] killed player does not exist!");
	}

	public bool IsGameOver() {
		if (_players != null) {
			return _players.Count == 1;
		}

		return false;
	}

	public int Winner() {
		if (_players.Count == 1) {
			List<int> ids = new List<int>(_players.Values);
			return ids[0];
		}
		return -1;
	}
}
