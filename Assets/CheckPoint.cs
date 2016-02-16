using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public Vector3 savedPOS;
	public int shieldCharge;
	public int bombCharge;
	public int hullHealth;

	private LastCheckpoint pData;
	private PlayerController pController;

	// Use this for initialization
	void Start () {
		savedPOS = gameObject.transform.position;
		shieldCharge = 0;
		bombCharge = 0;
		hullHealth = 0;

		pData = GameObject.FindGameObjectWithTag("Player").GetComponent<LastCheckpoint>();
		pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController> ();
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player Collider")){
			
			bombCharge = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>().currBombCharge;
			shieldCharge = pController.getShieldCharge();
			hullHealth = pController.getCurrHullIntegrity();
			pData.setCheckPoint(shieldCharge, bombCharge, hullHealth, savedPOS);

			transform.GetChild(0).GetComponent<Light>().color = Color.red;
		}
	}
}
