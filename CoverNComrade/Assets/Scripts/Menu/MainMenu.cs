using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    public UnityStandardAssets.ImageEffects.BlurOptimized CameraToBlur;
    public Camera[] CamerasToDisable;

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
            item.Menu.transform.localScale = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenMenu(string menuKey) {
        GameObject menu = null;
        if (_menuDictionary.TryGetValue(menuKey.ToLower(), out menu)) {
            if (_openMenu == menu) {
                CloseMenu();
                return;
            }
            if (_openMenu != null) {
                if (_openMenu.GetComponent<SettingsMenu>() != null) {
                    _openMenu.GetComponent<SettingsMenu>().SaveFile();
                }

                _openMenu.transform.localScale = Vector3.zero;
                //_openMenu.SetActive(false);
            }
            _openMenu = menu;
            _openMenu.transform.localScale = Vector3.one;
            //menu.SetActive(true);
            CameraToBlur.enabled = true;
            foreach (var camera in CamerasToDisable) {
                camera.enabled = false;
            }

        }
    }

    public void CloseMenu() {
        if (_openMenu != null) {
            _openMenu.transform.localScale = Vector3.zero;
            if (_openMenu.GetComponent<SettingsMenu>() != null) {
                _openMenu.GetComponent<SettingsMenu>().SaveFile();
            }
            //_openMenu.SetActive(false);
            _openMenu = null;
            CameraToBlur.enabled = false;
            foreach (var item in CamerasToDisable) {
                item.enabled = true;
            }
            foreach (var camera in CamerasToDisable) {
                camera.enabled = true;
            }
        }
    }

    public void QuitGame() {
        //Debug.Log("QuitGame");
        CloseMenu();
        Application.Quit();
    }

    public void QuickStart() {
        Debug.Log("QuickStart");
    }

}
