using UnityEngine;
using System.Collections;

public class reactorBomb : MonoBehaviour {
	public GameObject escapeDoor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//when the player shoots the thing, and if the bomb's fully charged, do the other thing
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Bomb Explosion"){
			if(other.gameObject.GetComponent<PowerBombExplosion> ().GetPowerLevel () >= 100) {
				GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombDetach>().detached = true;
				escapeDoor.GetComponent<ShatterWhenHit>().hitPoints = 0;
				//hpInternal--;
				//Destroy(collision.gameObject);
			}
		}
	}
}
