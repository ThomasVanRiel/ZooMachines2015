using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public LevelController Level;
	public GameObject PlayerPrefab; // the player prefab to spawn
	public GameObject CursorUI;
	public GameObject InfoUI;

	private GameMode _gameMode;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate playerKilled;

	void Start () {
		InfoUI.SetActive(false);
		StartCoroutine(Setup());
	}
	
	// Update is called once per frame
	void Update () {
		if (_gameMode != null && _gameMode.IsGameOver()) {
			InfoUI.SetActive(true);
			InfoUI.GetComponent<Text>().text = string.Format("{0} is the last man standing", _gameMode.Winner());
		}
	}

	void Destroy() {
		playerKilled -= _gameMode.PlayerKilled;
	}

	IEnumerator Setup() {
		while (InputManager.AmountOfMice == 0) {
			Debug.LogWarning("no mouse detected, waiting");
			yield return new WaitForSeconds(0.5f);
		}
		
		List<PlayerController> players = new List<PlayerController>();
		int mouseID = 0;
#if UNITY_EDITOR_WIN
		// TODO: in case there are more mice is than spawn points,
		//		 we need to properly place the new player.
		int nextSpawn = 0;
		for (int i = 0; i < InputManager.AmountOfMice; i++) {
			Transform spawnPos = Level.spawnPositions[nextSpawn++];
			mouseID = i;
#else
		foreach (Transform spawnPos in Level.spawnPositions) {
#endif
			GameObject playerObject = GameObject.Instantiate(PlayerPrefab, spawnPos.position, spawnPos.rotation) as GameObject;
			PlayerController player = playerObject.GetComponent<PlayerController>();
			playerObject.GetComponent<CursorDisplay>().CursorUI = CursorUI;
			playerObject.GetComponent<MouseInputReceiver>().ID = mouseID;
			playerObject.name = string.Format("Player {0}", mouseID + 1);
			
			players.Add(player);

#if UNITY_EDITOR_WIN
			if (nextSpawn > Level.spawnPositions.Length) {
				nextSpawn = 0;
			}
#endif
			
			yield return null;
		}

		_gameMode = new LMSMode(players);
		playerKilled += _gameMode.PlayerKilled;
	}
}
