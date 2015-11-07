using UnityEngine;
using System.Collections;

public class TestSettings : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	    Debug.Log(Utilities.Instance.GetSetting("Language"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
