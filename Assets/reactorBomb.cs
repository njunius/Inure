using UnityEngine;
using System.Collections;

public class reactorBomb : MonoBehaviour {
	public GameObject escapeDoor;

	private bool wasBombed = false;
	private bool isNewColor = false;
	private ParticleSystem reactionParticles;
	private float reactionPulseInterval = 5f;

	// Use this for initialization
	void Start () {
		reactionParticles = transform.FindChild ("Bomb Particles").gameObject.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (wasBombed) {
			if (!isNewColor) {
				transform.FindChild ("ReactorDetailBomb").gameObject.GetComponent<MeshRenderer> ().enabled = true;
				GetComponent<MeshRenderer> ().enabled = false;
				reactionParticles.gameObject.SetActive (true);
				Invoke ("BreakDoor", 1f);
				isNewColor = true;
			}
		}
	}

	//when the player shoots the thing, and if the bomb's fully charged, do the other thing
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Bomb Explosion"){
			if(other.gameObject.GetComponent<PowerBombExplosion> ().GetPowerLevel () >= 100 && !wasBombed) {
				wasBombed = true;
				GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombDetach>().detached = true;
				//hpInternal--;
				//Destroy(collision.gameObject);
			}
		}
	}

	void BreakDoor() {
		escapeDoor.GetComponent<ShatterWhenHit>().enabled = true;
		escapeDoor.GetComponent<ShatterWhenHit>().hitPoints = 0;
	}
}
