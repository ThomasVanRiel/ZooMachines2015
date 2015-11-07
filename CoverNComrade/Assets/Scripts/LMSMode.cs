using UnityEngine;
using System.Collections.Generic;

public class LMSMode : GameMode {
	private Dictionary<PlayerController, int> _players;

	public void Setup(GameManager gm, Dictionary<int, PlayerController> players) {
		_players = new Dictionary<PlayerController, int>();

		foreach (KeyValuePair<int, PlayerController> entry in players) {
			_players.Add(entry.Value, entry.Key);
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
