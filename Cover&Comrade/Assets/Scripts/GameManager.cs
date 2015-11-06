using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public Transform[] spawnPositions;
	public GameObject playerPrefab; // the player prefab to spawn

	private GameMode _gameMode;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate playerKilled;

	void Start () {
		List<PlayerController> players = new List<PlayerController>();

		// TODO: instantiate properly each player with an independent input controller
		//		 depending on the number of mice the input manager can return
		foreach (Transform spawnPos in spawnPositions) {
			GameObject playerObject = GameObject.Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation) as GameObject;
			PlayerController player = playerObject.GetComponent<PlayerController>();

			players.Add(player);
		}

		_gameMode = new LMSMode(players);
		playerKilled += _gameMode.PlayerKilled;
	}
	
	// Update is called once per frame
	void Update () {
		if (_gameMode.IsGameOver()) {
			Debug.Log("Game is over!");
			Debug.LogFormat("Winner is {0}", _gameMode.Winner());

			// TODO: ends the game here and display the winner
		}
	}

	void Destroy() {
		playerKilled -= _gameMode.PlayerKilled;
	}
}
