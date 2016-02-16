using UnityEngine;
using System.Collections;

public class LastCheckpoint : MonoBehaviour {

	public Vector3 savedPOS;
	public int shieldCharge;
	public int bombCharge;
	public int hullHealth;

	private PlayerController pController;
	private BombController bomb;

	// Save the initial data upon spawning
	void Start () {
		savedPOS = gameObject.transform.position;
		pController = gameObject.GetComponent<PlayerController> ();
		bomb = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>();
		bombCharge = bomb.currBombCharge;
		shieldCharge = pController.getShieldCharge();
		hullHealth = pController.getCurrHullIntegrity();
	}
	
	public void setCheckPoint(int newShield, int newBomb, int newHull, Vector3 newPOS){
		shieldCharge = newShield;
		bombCharge = newBomb;
		hullHealth = newHull;
		savedPOS = newPOS;
	}

	public int getShield (){ return shieldCharge;}

	public int getBomb (){ return bombCharge;}

	public int getHealth(){ return hullHealth;}

	public Vector3 getCheckPOS(){ return savedPOS;}
}
