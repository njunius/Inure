using UnityEngine;
using System.Collections;

public class HurtZone : MonoBehaviour {
	public bool instaKill = false; //Whether the player is instantly killed
	public int dmgRate = 50; //Damage done if instaKill == false
	/*// Use this for initialization
	void Start () {
	
	}*/

	//If the player enters the trigger, damage them
	void OnTriggerEnter(Collider other){
		GameObject tmp = other.gameObject;
		//Debug.Log ("Entered Trigger!");

		if (tmp.tag == "Player") {
			if (instaKill)
				tmp.GetComponent<PlayerController> ().currHullIntegrity = 0;
			else {
				tmp.GetComponent<PlayerController> ().currHullIntegrity -= dmgRate;
			}
		}
	}


	/*// Update is called once per frame
	void Update () {
	
	}*/
}
