using UnityEngine;
using System.Collections;

public class HurtZone : MonoBehaviour {
	public bool instaKill = false; //Whether the player is instantly killed
	//public int dmgRate = 50; //Damage done if instaKill == false (commented due to new hullIntegrity system, see playercontroller)
	/*// Use this for initialization
	void Start () {
	
	}*/

	//If the player enters the trigger, damage them
	void OnTriggerEnter(Collider other){
		GameObject tmp = other.gameObject;
		//Debug.Log ("Entered Trigger!");

		if (tmp.CompareTag("Player")) {
			if (instaKill)
				tmp.GetComponent<PlayerController> ().setHullIntegrity(0);
			else {
				tmp.GetComponent<PlayerController> ().takeDamage();
			}
		}
	}


	/*// Update is called once per frame
	void Update () {
	
	}*/
}
