using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public enum State {
		Waiting,
		Ready,
		InProgess,
		GameOver
	};

	public enum GameModeChoice {
		LastManStanding,
		DeathMatch
	};

	//public GameObject LevelPrefab;
	public GameObject PlayerPrefab; // the player prefab to spawn
	public GameObject CursorUI;
	public GameModeChoice ModeChoice = GameModeChoice.LastManStanding;

	// UI
	public GameObject WaitingUI;
	public GameObject ReadyUI;
	public Text CountdownText;
	public GameObject InProgressUI;
	public GameObject GameOverUI;
	public Text InfoText;

	// Properties
	private CameraHandler _cam;
	private Dictionary<int, PlayerController> _players;
	private GameMode _gameMode;
	private int _nextSpawn;
	private State _currentState;
	private delegate void updateState();
	private updateState _updateState;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate PlayerKilled;

	public delegate IEnumerator Command();

    public Vector3[] SpawnPoints;
    private int _spawnPointsIndex;
    private List<int> _spawnPointsOrder = new List<int>();
	
	void Start () {
		WaitingUI.SetActive(false);
		ReadyUI.SetActive(false);
		InProgressUI.SetActive(false);
		GameOverUI.SetActive(false);

		ChangeState(State.Waiting);

		_cam = Camera.main.GetComponent<CameraHandler>();

		// Load the last played game mode
		if (PlayerPrefs.HasKey("GameMode")) {
			Debug.LogFormat("Got last played game mode: {0}", PlayerPrefs.GetInt("GameMode"));
			ModeChoice = (GameModeChoice)(PlayerPrefs.GetInt("GameMode"));
		}
	
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

        //Spawn points
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            _spawnPointsOrder.Add(i);
        }
        // random order spawnpoints
        for (int i = 0; i < _spawnPointsOrder.Count; i++)
        {
            int temp = _spawnPointsOrder[i];
            int rand = Random.Range(0, _spawnPointsOrder.Count);
            _spawnPointsOrder[i] = _spawnPointsOrder[rand];
            _spawnPointsOrder[rand] = temp;
        }

		_players = new Dictionary<int, PlayerController> ();
		PlayerKilled += _gameMode.PlayerKilled;
	}

	void ChangeState(State state) {
		switch (state) {
		case State.Waiting:
			// Wait for at least 2 players
			WaitingUI.SetActive(true);

			_currentState = State.Waiting;
			_updateState = UpdateWaiting;
			break;
		case State.Ready:
			// Start the countdown, players can still join
			WaitingUI.SetActive(false);
			ReadyUI.SetActive(true);

			_currentState = State.Ready;
			_updateState = UpdateReady;

			// start countdown of 10 seconds, then change state to ready
			StartCoroutine(ChangeStateAfter(State.InProgess, 10));
			break;
		case State.InProgess:
			// Game in progress
			ReadyUI.SetActive(false);
			InProgressUI.SetActive(true);

			_gameMode.Setup(this, _players);
			foreach (KeyValuePair<int, PlayerController> player in _players) {
				player.Value.CanMove = true;
				player.Value.CanShoot = true;
			}

			_currentState = State.InProgess;
			_updateState = UpdateInProgress;
			break;
		case State.GameOver:
			// Game is over, designate the winner
			InProgressUI.SetActive(false);
			GameOverUI.SetActive(true);

			foreach (KeyValuePair<int, PlayerController> player in _players) {
				player.Value.CanMove = false;
				player.Value.CanShoot = false;
			}

			_currentState = State.GameOver;
			_updateState = UpdateGameOver;
			break;
		}
	}

	private IEnumerator ChangeStateAfter(State state, int n) {
		for (int i = n; i > 0; i--) {
			CountdownText.text = string.Format("{0}", i);
			yield return new WaitForSeconds(1.0f);
		}
		
		ChangeState(State.InProgess);
	}

	// Update is called once per frame
	void Update () {
		if (_updateState != null) {
			_updateState();
		}

	    if (_currentState != State.GameOver )
	    {
#if UNITY_EDITOR_WIN || !UNITY_EDITOR
            Cursor.lockState = CursorLockMode.Locked;
#endif
            Cursor.visible = false;
        }
	    else
        {
#if UNITY_EDITOR_WIN || !UNITY_EDITOR
            Cursor.lockState = CursorLockMode.None;
#endif
            Cursor.visible = true;

        }
	}

	// UpdateWaiting is the update method in Waiting state
	void UpdateWaiting() {
		if (_players.Count >= 2) {
			Debug.LogFormat("Changing to ready state");
			ChangeState(State.Ready);
			return;
		}
		Debug.LogFormat("Waiting for {0} player(s)...", 2 - _players.Count);

		int newPlayer = InputManager.GetMouseButtonClicked(0);
		if (newPlayer != -1 && !_players.ContainsKey(newPlayer)) {
			SpawnPlayer(newPlayer);
		}
	}

	// UpdateReady is the update method in Ready state
	void UpdateReady() {
		int newPlayer = InputManager.GetMouseButtonClicked(0);
		if (newPlayer != -1 && !_players.ContainsKey(newPlayer)) {
			SpawnPlayer(newPlayer);
		}
	}

	// UpdateInProgress is the update method in InProgress state
	void UpdateInProgress() {
		if (_gameMode != null && _gameMode.IsGameOver()) {
			InfoText.text = string.Format("Player {0} won", _gameMode.Winner() + 1);
			ChangeState(State.GameOver);
		}
	}

	// UpdateGameOver is the update method for the GameOver state
	void UpdateGameOver() {
		// TODO: wait for players to make their choice
	}

	void Destroy() {
		PlayerKilled -= _gameMode.PlayerKilled;
	}

	public PlayerController SpawnPlayer(int playerID) {
		//Transform spawnPos = _level.spawnPositions[_nextSpawn++];
		//if (_nextSpawn >= _level.spawnPositions.Length)
		//	_nextSpawn = 0;

        Vector3 spawnPos = SpawnPoints[GetOrderedRandomSpawnPointIndex()];

		GameObject playerObject = GameObject.Instantiate(PlayerPrefab, spawnPos, Quaternion.identity) as GameObject;
		PlayerController player = playerObject.GetComponent<PlayerController>();
		playerObject.GetComponent<CursorDisplay>().CursorUI = CursorUI;
		playerObject.GetComponent<MouseInputReceiver>().PlayerID = playerID;
		playerObject.name = string.Format("Player {0}", playerID + 1);

		_players[playerID] = player;

		if (_cam != null) {
			_cam.SetPlayerList(new List<PlayerController>(_players.Values));
		}

		return player;
	}

	// Execute executes a command in a new CoRoutine
	public void Execute(Command cmd) {
		StartCoroutine(cmd());
	}

    void OnDrawGizmosSelected()
    {
        if (SpawnPoints.Length <= 0)
            return;

        Gizmos.color = Color.magenta;
        foreach (var spawn in SpawnPoints)
        {
            Gizmos.DrawLine(spawn, spawn + Vector3.up * 20);
            Gizmos.DrawSphere(spawn, 1);
        }
    }

    int GetOrderedRandomSpawnPointIndex()
    {
        int index = _spawnPointsOrder[_spawnPointsIndex];
        _spawnPointsIndex = (++_spawnPointsIndex) % SpawnPoints.Length;
        return index;
    }

	public void Restart() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	public void Quit() {
		Application.LoadLevel("MainMenu");
	}
}
