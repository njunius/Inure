using UnityEngine;
using System.Collections;

public class turretTrigger : MonoBehaviour {

	public GameObject[] turretsInRoom = new GameObject[2];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < turretsInRoom.Length; i++) {
			turretsInRoom [i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
