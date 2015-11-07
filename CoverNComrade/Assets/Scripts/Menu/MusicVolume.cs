using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicVolume : MonoBehaviour {

    public float VolumeMultiplier = 0.5f;
    void Start() {
        GetComponent<AudioSource>().volume = float.Parse(Utilities.Instance.GetSetting("MusicVolume")) * VolumeMultiplier;
    }

    public void SetVolume(float volume) {
        GetComponent<AudioSource>().volume = volume * VolumeMultiplier;
    }
}
