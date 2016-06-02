using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour {
	public GameObject reactor;
	public GameObject escapeDoor;
	public GameObject[] shieldPieces;
	public int enableReactorField = 16;
	public int turretInterval = 4;
	public GameObject[] turretTurnOn;
	public bool[] hasSpawned;
	public GameObject reactorCore;

	private int shieldCount = 0;
	private int countTMP = 0;
	public bool activatedDoor = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Count # of shield pieces destroyed (== null)
		for(int i = 0; i < shieldPieces.Length; i++){
			if(shieldPieces[i] == null)
				countTMP++;
		}

		//If one or more is destroyed since last check,
		//add to perminent count
		if(countTMP > shieldCount)
			shieldCount = countTMP;

		countTMP = 0;

		//If shieldCount == certain #, spawn a turret at a point
		int divideTMP = shieldCount / turretInterval;

		if(shieldCount % turretInterval == 0 
			&& divideTMP < hasSpawned.Length
			&& !hasSpawned[divideTMP]){

			turretTurnOn[divideTMP].GetComponent<Turret> ().TurnOn ();
			
			hasSpawned[divideTMP] = true;
		}
		//If shieldCount > enableReactorField, enable the sphere trigger
		if(shieldCount > enableReactorField){
			reactor.GetComponent<SphereCollider>().enabled = true;
			Destroy(reactorCore);
			/*if(!activatedDoor){
				escapeDoor.GetComponent<ShatterWhenHit>().enabled = true;
				activatedDoor = true;
			}*/
			reactor.transform.GetChild(0).GetComponent<Light>().color = Color.red;
		}
	}
}