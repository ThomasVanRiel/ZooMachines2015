using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public LevelController level;
	public GameObject playerPrefab; // the player prefab to spawn

	private GameMode _gameMode;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate playerKilled;

	void Start () {
		List<PlayerController> players = new List<PlayerController>();

#if UNITY_EDITOR_WIN
		// TODO: in case number of mice is higher than the number of spawns,
		//		 we need to properly place the new player.
		int nextSpawn = 0;
		for (int i = 0; i < InputManager.AmountOfMice; i++) {
			Transform spawnPos = level.spawnPositions[nextSpawn++];
#else
		foreach (Transform spawnPos in level.spawnPositions) {
#endif
			GameObject playerObject = GameObject.Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation) as GameObject;
			PlayerController player = playerObject.GetComponent<PlayerController>();
			
			players.Add(player);

#if UNITY_EDITOR_WIN
			if (nextSpawn > level.spawnPositions.Length) {
				nextSpawn = 0;
			}
#endif
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
