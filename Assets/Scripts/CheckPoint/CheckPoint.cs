using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public Vector3 savedPOS;
	public Quaternion savedROT;
	public int shieldCharge;
	public int bombCharge;
	public int hullHealth;
	public int heal_player = 1;

	private LastCheckpoint pData;
	private PlayerController pController;
	private bool hasHealed = false;

	// Use this for initialization
	void Start () {
		savedPOS = gameObject.transform.position;
		savedROT = gameObject.transform.rotation;
		shieldCharge = 0;
		bombCharge = 0;
		hullHealth = 0;

		pData = GameObject.FindGameObjectWithTag("Player").GetComponent<LastCheckpoint>();
		pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController> ();
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player Collider")){
			
			if(!hasHealed && pController.getCurrHullIntegrity() < pController.getMaxHullIntegrity()){
				pController.setHullIntegrity(pController.getCurrHullIntegrity() + heal_player);
				hasHealed = true;
			}

			shieldCharge = pController.getShieldCharge();
			hullHealth = pController.getCurrHullIntegrity();
			pData.setCheckPoint(shieldCharge, bombCharge, hullHealth, savedPOS, savedROT);

			transform.GetChild(1).GetComponent<Light>().color = Color.red;
			transform.GetChild(2).GetComponent<Light>().color = Color.red;
			bombCharge = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>().currBombCharge;
		}
	}
}
