using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    public GameObject Prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Y)) {
            SpawnPlayer();
        }
	}

    public void SpawnPlayer() {
        var obj = Instantiate(Prefab, transform.position, Quaternion.identity) as GameObject;
        obj.GetComponent<RagdollScript>().Ragdolled = true;
    }
}
