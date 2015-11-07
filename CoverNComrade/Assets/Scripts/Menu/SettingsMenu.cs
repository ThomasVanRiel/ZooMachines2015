using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class SettingsMenu : MonoBehaviour {

    [System.Serializable]
    public class SettingsPair {
        public string Name;
        public GameObject Object;
    }
    public SettingsPair[] Settings;

    public string FileName;

    private Dictionary<string, GameObject> _settingsDictionary = new Dictionary<string, GameObject>();

    private List<Dropdown.OptionData> _languages = new List<Dropdown.OptionData>();

    void Awake() {
        foreach (var item in Settings) {
            _settingsDictionary.Add(item.Name, item.Object);
        }

        //Languages
        XElement xmlDictionary;
        xmlDictionary = XElement.Load(string.Format("{0}/{1}", Application.dataPath, Localization.Instance.FileName));
        foreach (var item in xmlDictionary.Elements("Language")) {
            _languages.Add(new Dropdown.OptionData(item.Attribute("language").Value));
        }


        GameObject languageObject = _settingsDictionary["Language"];
        if (languageObject != null) {
            languageObject.GetComponent<Dropdown>().options.Clear();
            languageObject.GetComponent<Dropdown>().options = _languages;
        }

        //LoadSettings

        LoadFromFile(string.Format("{0}/{1}", Application.dataPath, FileName));

    }

    void OnDisable() {
        XElement settingsList = new XElement("Settings");

        foreach (var item in _settingsDictionary) {
            string variable = item.Key;
            string value = string.Empty;
            //string type = string.Empty;
            Component dataComponent = null;
            dataComponent = item.Value.GetComponent<Slider>();
            if (dataComponent == null) {
                dataComponent = item.Value.GetComponent<Dropdown>();
                if (dataComponent == null) {
                    dataComponent = item.Value.GetComponent<Toggle>();
                    if (dataComponent == null) {
                        return;
                    } else {
                        value = ((Toggle)dataComponent).isOn.ToString();
                    }
                } else {
                    value = _languages[((Dropdown)dataComponent).value].text;
                }
            } else {
                value = ((Slider)dataComponent).value.ToString();
            }

            settingsList.Add(new XElement(variable, value));
        }

        settingsList.Save(string.Format("{0}/{1}", Application.dataPath, FileName));
    }


    void LoadFromFile(string fileName) {
        XElement settingsList = XElement.Load(fileName);

        foreach (var item in _settingsDictionary) {
            Component dataComponent = null;
            dataComponent = item.Value.GetComponent<Slider>();
            if (dataComponent == null) {
                dataComponent = item.Value.GetComponent<Dropdown>();
                if (dataComponent == null) {
                    dataComponent = item.Value.GetComponent<Toggle>();
                    if (dataComponent == null) {
                        return;
                    } else {
                        ((Toggle)dataComponent).isOn = bool.Parse(settingsList.Element(item.Key).Value);
                    }
                } else {
                    ((Dropdown)dataComponent).value = _languages.FindIndex(o => o.text == settingsList.Element(item.Key).Value);
                    LanguageChange(((Dropdown)dataComponent).value);
                }
            } else {
                ((Slider)dataComponent).value = float.Parse(settingsList.Element(item.Key).Value);
            }
        }

        Utilities.Instance.UpdateSettings();
    }

    public void LanguageChange(int index) {
        _settingsDictionary["Language"].GetComponent<Dropdown>().value = index;
        Localization.Instance.ChangeLanguage(_languages[index].text);
    }
}
