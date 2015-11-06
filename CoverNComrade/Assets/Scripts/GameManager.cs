using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public enum GameModeChoice {
		LastManStanding,
		DeathMatch
	};

	public GameObject LevelPrefab;
	public GameObject PlayerPrefab; // the player prefab to spawn
	public GameObject CursorUI;
	public GameObject InfoUI;
	public LevelController Level;
	public GameModeChoice ModeChoice = GameModeChoice.LastManStanding;

	private CameraHandler _cam;
	private Dictionary<PlayerController, int> _players;
	private GameMode _gameMode;
	private int _nextSpawn;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate PlayerKilled;

	public delegate IEnumerator Command();

	void Start () {
		InfoUI.SetActive(false);
		_cam = Camera.main.GetComponent<CameraHandler>();

		switch(ModeChoice) {
		case GameModeChoice.LastManStanding:
			_gameMode = new LMSMode();
			break;
		case GameModeChoice.DeathMatch:
			_gameMode = new DMMode(10);
			break;
		default:
			Debug.LogErrorFormat("Selected game mode does not exist! Doing LMS.");
			_gameMode = new LMSMode();
			break;
		}
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

#if UNITY_EDITOR_WIN
		_gameMode.Setup(this, InputManager.AmountOfMice);
#else
		_gameMode.Setup(this, Level.spawnPositions.Length);
#endif
	}

	public PlayerController SpawnPlayer(int playerID) {
		Transform spawnPos = Level.spawnPositions[_nextSpawn++];
		if (_nextSpawn >= Level.spawnPositions.Length)
			_nextSpawn = 0;

		GameObject playerObject = GameObject.Instantiate(PlayerPrefab, spawnPos.position, spawnPos.rotation) as GameObject;
		PlayerController player = playerObject.GetComponent<PlayerController>();
		playerObject.GetComponent<CursorDisplay>().CursorUI = CursorUI;
		playerObject.GetComponent<MouseInputReceiver>().PlayerID = playerID;
		playerObject.name = string.Format("Player {0}", playerID + 1);

		_players[player] = playerID;

		if (_cam != null) {
			_cam.SetPlayerList(new List<PlayerController>(_players.Keys));
		}

		return player;
	}

	public void Execute(Command cmd) {
		StartCoroutine(cmd());
	}
}
