using UnityEngine;
using UnityEngine.UI;
using System.Collections;using System.Xml.Linq;

public class LocalizationSlave : MonoBehaviour {

    public string XMLName;
	// Use this for initialization
	void Awake () {
        Localization.Instance.UpdateDelegateImplementation += UpdateLanguage;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void UpdateLanguage() {
        GetComponent<Text>().text = Localization.Instance.GetTranslation(XMLName);
    }


}
