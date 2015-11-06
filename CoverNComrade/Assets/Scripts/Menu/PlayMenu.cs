using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayMenu : MonoBehaviour {

    [System.Serializable]
    public struct MenuPair {
        public string Name;
        public GameObject Menu;
    }
    public MenuPair[] Menus;

    private Dictionary<string, GameObject> _menuDictionary = new Dictionary<string, GameObject>();
    private GameObject _openMenu;

    // Use this for initialization
    void Awake() {
        foreach (var item in Menus) {
            _menuDictionary.Add(item.Name.ToLower(), item.Menu);
        }

        foreach (var item in _menuDictionary) {
            item.Value.SetActive(false);
        }

        OpenMenu(Menus[0].Name);
    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenMenu(string menuKey) {
        GameObject menu = null;
        if (_menuDictionary.TryGetValue(menuKey.ToLower(), out menu)) {
            if (_openMenu != null) {
                _openMenu.SetActive(false);
            }
            _openMenu = menu;
            menu.SetActive(true);
        }
    }
}
