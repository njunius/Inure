using UnityEngine;
using System.Collections;

public class ShatterWhenHit : MonoBehaviour {
	public GameObject shatterInto;
    public int hitPoints = 1;

	private int hpInternal = 0;
	// Use this for initialization
	void Start () {
		hpInternal = hitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		if(hpInternal <= 0){
			Instantiate(shatterInto, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
	//when the player shoots the thing, reduce HP by 1
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Bomb Explosion"){
			if(other.gameObject.transform.parent.gameObject.GetComponent<PowerBomb> ().GetPowerLevel () >= hitPoints) {
				hpInternal = 0;
				//hpInternal--;
				//Destroy(collision.gameObject);
			}
		}
	}
}
