using UnityEngine;
using System.Collections.Generic;

public class LMSMode : GameMode {
	private List<PlayerController> _players;

	public LMSMode(List<PlayerController> players) {
		_players = players;
	}

	public void PlayerKilled(PlayerController killer, PlayerController killed) {
		if (_players.Contains(killed))
			_players.Remove(killed);
		else
			Debug.Log("killed player does not exist!");
	}

	public bool IsGameOver() {
		return _players.Count == 1;
	}

	public PlayerController Winner() {
		if (_players.Count == 1) {
			return _players[0];
		}
		return null;
	}
}
