using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicVolume : MonoBehaviour {

    private float _targetVolume;

    public AudioClip[] clips;
    private int _clipIndex = 0;
    public float VolumeMultiplier = 0.5f;
    void Start() {
        //GetComponent<AudioSource>().volume = float.Parse(Utilities.Instance.GetSetting("MusicVolume")) * VolumeMultiplier;
        _targetVolume = float.Parse(Utilities.Instance.GetSetting("MusicVolume")) * VolumeMultiplier;
        StopCoroutine("ChangeClip");
        StopCoroutine("ChangeVolume");
        StartCoroutine("ChangeVolume");
    }

    IEnumerator ChangeVolume() {
        while (Mathf.Abs(GetComponent<AudioSource>().volume - _targetVolume) > 0.00001f) {
            yield return new WaitForFixedUpdate();
            if (GetComponent<AudioSource>().volume < _targetVolume) {
                GetComponent<AudioSource>().volume += 0.001f;
            } else {
                GetComponent<AudioSource>().volume -= 0.001f;
            }
        }
    }

    IEnumerator ChangeClip() {
        while (GetComponent<AudioSource>().volume > 0.00001f) {
            yield return new WaitForFixedUpdate();
            GetComponent<AudioSource>().volume -= 0.005f;
        }
        ++_clipIndex;
        _clipIndex %= 2;
        GetComponent<AudioSource>().clip = clips[_clipIndex];
        GetComponent<AudioSource>().Play();
        while (GetComponent<AudioSource>().volume < _targetVolume) {
            yield return new WaitForFixedUpdate();
            GetComponent<AudioSource>().volume += 0.005f;
        }

    }

    public void SetVolume(float volume) {
        _targetVolume = volume * VolumeMultiplier;
        StopCoroutine("ChangeClip");
        StopCoroutine("ChangeVolume");
        StartCoroutine("ChangeVolume");
    }

    public void SetVolumeImmediate(float volume) {
        _targetVolume = volume * VolumeMultiplier;
        GetComponent<AudioSource>().volume = _targetVolume;
    }


    public void ChangeAudioClip() {
        StopCoroutine("ChangeVolume");
        StopCoroutine("ChangeClip");
        StartCoroutine("ChangeClip");
    }
}
