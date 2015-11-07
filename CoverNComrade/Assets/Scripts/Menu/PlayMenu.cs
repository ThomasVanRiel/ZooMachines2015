using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayMenu : MonoBehaviour {

    [System.Serializable]
    public struct MenuPair {
        public string Name;
        public GameObject Menu;

        public MenuPair(string name, GameObject menu) {
            Name = name;
            Menu = menu;
        }
    }

    private string _selectedLevel;

    public MenuPair[] Menus;

    private Dictionary<string, GameObject> _menuDictionary = new Dictionary<string, GameObject>();
    private MenuPair _openMenu;

    // Use this for initialization
    void Awake() {
        foreach (var item in Menus) {
            _menuDictionary.Add(item.Name.ToLower(), item.Menu);
        }

        foreach (var item in _menuDictionary) {
            item.Value.SetActive(false);
        }
        PlayerPrefs.SetInt("GameMode", (int)GameManager.GameModeChoice.DeathMatch);
        _selectedLevel = "Minim";

        OpenMenu(Menus[0].Name);
    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenMenu(string menuKey) {
        GameObject menu = null;
        if (_menuDictionary.TryGetValue(menuKey.ToLower(), out menu)) {
            if (_openMenu.Menu != null) {
                _openMenu.Menu.SetActive(false);
            }
            _openMenu = new MenuPair(menuKey, menu);
            menu.SetActive(true);

            switch (menuKey.ToLower()) {
                case "deathmatch":
                    PlayerPrefs.SetInt("GameMode", (int)GameManager.GameModeChoice.DeathMatch);
                    break;
                case "lastmanstanding":
                    PlayerPrefs.SetInt("GameMode", (int)GameManager.GameModeChoice.LastManStanding);
                    break;
                default:
                    break;
            }

            Debug.Log(PlayerPrefs.GetInt("GameMode"));
        }
    }

    public void SetLevel(string levelName) {
        _selectedLevel = levelName;
    }

    public void StartGame() {
        Application.LoadLevel(_selectedLevel);
    }
}
