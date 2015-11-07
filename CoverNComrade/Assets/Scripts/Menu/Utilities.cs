using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class Utilities : MonoBehaviour {
    #region Singleton
    static Utilities instance;
    public static Utilities Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Utilities>();
                if (instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = "Utilities";
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<Utilities>();
                }
            }
            return instance;
        }
    }
    #endregion
    
    private Dictionary<string, string> _settings = new Dictionary<string, string>();

    public void UpdateSettings() {
        _settings.Clear();
        XElement settingsList = XElement.Load(string.Format("{0}/{1}", Application.dataPath, "Settings.xml"));

        foreach (var item in settingsList.Descendants()){
            _settings.Add(item.Name.ToString(), item.Value);
        }
    }

    public string GetSetting(string settingName) {
        string value = "Setting does not exist, check spelling";
        if (_settings.TryGetValue(settingName, out value)) {
            return value;
        }
        return value;
    }
}
