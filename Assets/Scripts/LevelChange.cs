using UnityEngine;
using System.Collections;

public class LevelChange : MonoBehaviour {
	public string levelName = "01-Entrance";
	void OnTriggerEnter(Collider other) {
		Debug.Log ("Collided!");
		if(other.CompareTag("Player Collider")){
			Debug.Log ("Is Player!");
            Application.LoadLevel (levelName);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
