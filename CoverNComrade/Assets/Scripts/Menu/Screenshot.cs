using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour {

    // Use this for initialization
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) {
            var fileName = string.Format("{0}/{1}", Application.dataPath, string.Format("{0}-{1}-{2}_{3}-{4}-{5}.png", System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second));
            Debug.Log(fileName);
            Application.CaptureScreenshot(fileName);
        }
    }
}
