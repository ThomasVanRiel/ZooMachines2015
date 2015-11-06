using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class Localization : MonoBehaviour {

    #region Singleton
    static Localization instance;
    public static Localization Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Localization>();
                if (instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = "Localization";
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<Localization>();
                }
            }
            return instance;
        }
    }
    #endregion

    public string FileName = "Language.xml";
    public string _currentLanguage = "english";

    public delegate void UpdateLanguageDelegate();
    public UpdateLanguageDelegate UpdateDelegateImplementation;

    private Dictionary<string, XElement> _languageDictionary = new Dictionary<string, XElement>();

    void Awake() {
        DontDestroyOnLoad(this.gameObject);




        UpdateDictionary();
    }

    public void UpdateDictionary() {

        XElement xmlDictionary;

#if DEBUG
        xmlDictionary = XElement.Load(string.Format("{0}/{1}", Application.dataPath, FileName));
#else
        xmlDictionary = XElement.Load(string.Format("{0}/{1}", Application.dataPath, FileName));
#endif


        foreach (var item in xmlDictionary.Elements("Language")) {
            _languageDictionary.Add(item.Attribute("language").Value.ToLower(), item);
        }
        UpdateDelegateImplementation();

    }

    public string GetTranslation(string name) {
        var list = _languageDictionary[_currentLanguage.ToLower()].Descendants(name);
        foreach (var item in list) {
            return item.Value;
        }
        return "Error, no translation found!";
    }

    public void ChangeLanguage(string language) {
        _currentLanguage = language;
        UpdateDelegateImplementation();
    }

}
