using UnityEngine;
using System.Collections;

public class LastCheckpoint : MonoBehaviour {

	public Vector3 savedPlayerPOS, savedCamPOS;
	public Quaternion savedPlayerROT, savedCamROT;
	public float shieldCharge;
	public int bombCharge;
	public int hullHealth;

	private PlayerController pController;
	private BombController bomb;

	// Save the initial data upon spawning
	void Start () {
		savedPlayerPOS = gameObject.transform.position;
		savedPlayerROT = gameObject.transform.rotation;
		savedCamPOS = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		savedCamROT = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
		pController = gameObject.GetComponent<PlayerController> ();
		//bomb = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>();
		//bombCharge = bomb.currBombCharge;
		shieldCharge = pController.getShieldCharge();
		hullHealth = pController.getCurrHullIntegrity();

	}

	//This is usually called by PlayerController during a reload
	//NOTE: Assumes the main camera exists
	public void setCheckPoint(float newShield, int newBomb, int newHull, Vector3 newPOS, Quaternion newROT){
		shieldCharge = newShield;
		bombCharge = newBomb;
		hullHealth = newHull;
		savedPlayerPOS = newPOS;
		savedPlayerROT = newROT;
		savedCamPOS = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		savedCamROT = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
	}

	public float getShield (){ return shieldCharge;}

	public int getBomb (){ return bombCharge;}

	public int getHealth(){ return hullHealth;}

	public Vector3 getCheckPOS(){ return savedPlayerPOS;}

	public Quaternion getCheckROT(){ return savedPlayerROT;}

	public Vector3 getCheckCamPOS(){return savedCamPOS;}

	public Quaternion getCheckCamROT(){return savedCamROT;}


}
