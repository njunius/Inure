using UnityEngine;
using System.Collections;

public class ShatterWhenShot : MonoBehaviour {
	public GameObject shatterInto;
	public int hitPoints = 4;
    private AudioSource source;

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
	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Player Projectile" || other.gameObject.tag == "Bomb Explosion"){
				hpInternal--;
		}
	}
}
