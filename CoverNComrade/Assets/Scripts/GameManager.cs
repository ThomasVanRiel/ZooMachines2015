using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject LevelPrefab;
	public GameObject PlayerPrefab; // the player prefab to spawn
	public GameObject CursorUI;
	public GameObject InfoUI;

	private CameraHandler _cam;
	private LevelController _level;
	private Dictionary<PlayerController, int> _players;
	private GameMode _gameMode;
	private int _nextSpawn;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate PlayerKilled;

	void Start () {
		InfoUI.SetActive(false);
		GameObject levelObject = Instantiate(LevelPrefab) as GameObject;
		_level = levelObject.GetComponent<LevelController>();
		_cam = Camera.main.GetComponent<CameraHandler>();
		_gameMode = new LMSMode();
		_players = new Dictionary<PlayerController, int> ();
		PlayerKilled += _gameMode.PlayerKilled;

		StartCoroutine(Setup());
	}
	
	// Update is called once per frame
	void Update () {
		if (_gameMode != null && _gameMode.IsGameOver()) {
			InfoUI.SetActive(true);
			InfoUI.GetComponent<Text>().text = string.Format("Player {0} won", _gameMode.Winner() + 1);
		}
	}

	void Destroy() {
		PlayerKilled -= _gameMode.PlayerKilled;
	}

	IEnumerator Setup() {
		while (InputManager.AmountOfMice == 0) {
			Debug.LogWarning("no mouse detected, waiting");
			yield return new WaitForSeconds(0.5f);
		}

		GameObject levelObject = Instantiate(LevelPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		_level = levelObject.GetComponent<LevelController>();
		_gameMode.Setup(this, InputManager.AmountOfMice);
	}

	public PlayerController SpawnPlayer(int id) {
		Transform spawnPos = _level.spawnPositions[_nextSpawn++];
		if (_nextSpawn > _level.spawnPositions.Length)
			_nextSpawn = 0;

		GameObject playerObject = GameObject.Instantiate(PlayerPrefab, spawnPos.position, spawnPos.rotation) as GameObject;
		PlayerController player = playerObject.GetComponent<PlayerController>();
		playerObject.GetComponent<CursorDisplay>().CursorUI = CursorUI;
		playerObject.GetComponent<MouseInputReceiver>().ID = id;
		playerObject.name = string.Format("Player {0}", id + 1);

		_players[player] = id;

		if (_cam != null) {
			_cam.SetPlayerList(new List<PlayerController>(_players.Keys));
		}

		return player;
	}
}
