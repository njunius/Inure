using UnityEngine;
using System.Collections;

public class turretTriggerOff : MonoBehaviour {

	public GameObject[] turretsInRoom = new GameObject[2];

	// Use this for initialization
	/*void Start () {
		for (int i = 0; i < turretsInRoom.Length; i++) {
			turretsInRoom [i].SetActive(false);
		}
	}*/

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player Collider")){
			
			for (int i = 0; i < turretsInRoom.Length; i++) {
				
				turretsInRoom [i].SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
