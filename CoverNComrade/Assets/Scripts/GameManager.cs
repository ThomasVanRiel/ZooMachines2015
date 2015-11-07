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

	//public GameObject LevelPrefab;
	public GameObject PlayerPrefab; // the player prefab to spawn
	public GameObject CursorUI;
	public GameObject InfoUI;
	public GameModeChoice ModeChoice = GameModeChoice.LastManStanding;

	private CameraHandler _cam;
	private LevelController _level;
	private Dictionary<PlayerController, int> _players;
	private GameMode _gameMode;
	private int _nextSpawn;

	public delegate void PlayerKilledDelegate(PlayerController killer, PlayerController killed);
	public static PlayerKilledDelegate PlayerKilled;

	public delegate IEnumerator Command();

    public Vector3[] SpawnPoints;
    private int _spawnPointsIndex;
    private List<int> _spawnPointsOrder;

	void Start () {
		InfoUI.SetActive(false);
		//GameObject levelObject = Instantiate(LevelPrefab) as GameObject;
		//_level = levelObject.GetComponent<LevelController>();
		_cam = Camera.main.GetComponent<CameraHandler>();

		ModeChoice = GameModeChoice.DeathMatch;
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
		_players = new Dictionary<PlayerController, int> ();
		PlayerKilled += _gameMode.PlayerKilled;

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

		//GameObject levelObject = Instantiate(LevelPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		//_level = levelObject.GetComponent<LevelController>();
#if UNITY_EDITOR_WIN || !UNITY_EDITOR
		_gameMode.Setup(this, InputManager.AmountOfMice);
#else
		_gameMode.Setup(this, _level.spawnPositions.Length);
#endif
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

		_players[player] = playerID;

		if (_cam != null) {
			_cam.SetPlayerList(new List<PlayerController>(_players.Keys));
		}

		return player;
	}

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
}
