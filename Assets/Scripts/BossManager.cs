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
	private Transform innerShields;
	private Transform midShields;
	private Transform outerShields;
	private Transform lightning;
	private Transform endRakingTurrets;
	private Transform midRakingTurrets;
	private Transform T_Turrets;
	private Transform triangleTurrets;

	private int shieldCount = 0;
	private int countTMP = 0;
	public bool activatedDoor = false;
	private bool isSusceptible = false;
	// Use this for initialization
	void Start () {
		innerShields = transform.FindChild ("ShieldInner-CrackedWhole");
		midShields = transform.FindChild ("ShieldMid-CrackedWhole");
		outerShields = transform.FindChild ("ShieldOuter-CrackedWhole");
		lightning = transform.FindChild ("LightningRodParticleSystems");
		endRakingTurrets = transform.FindChild ("Turrets").FindChild ("PulseRaking").FindChild ("End");
		midRakingTurrets = transform.FindChild ("Turrets").FindChild ("PulseRaking").FindChild ("Mid");
		T_Turrets = transform.FindChild ("Turrets").FindChild ("T");
		triangleTurrets = transform.FindChild ("Turrets").FindChild ("PulseTriangleWall");
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



			hasSpawned[divideTMP] = true;
		}
		//If shieldCount > enableReactorField, enable the sphere trigger
		if(shieldCount > enableReactorField && !isSusceptible){
			isSusceptible = true;
			Orbit curOrbit;
			for (int numInner = 0; numInner < innerShields.childCount; ++numInner) {
				curOrbit = innerShields.GetChild (numInner).GetComponent<Orbit> ();
				curOrbit.SetRotationRate (curOrbit.GetRotationRate () * 2f);
			}
			for (int numMid = 0; numMid < midShields.childCount; ++numMid) {
				curOrbit = midShields.GetChild (numMid).GetComponent<Orbit> ();
				curOrbit.SetRotationRate (curOrbit.GetRotationRate () * 2f);
			}
			for (int numOuter = 0; numOuter < outerShields.childCount; ++numOuter) {
				curOrbit = outerShields.GetChild (numOuter).GetComponent<Orbit> ();
				curOrbit.SetRotationRate (curOrbit.GetRotationRate () * 2f);
			}

			/*for (int numRake = 0; numRake < endRakingTurrets.childCount; ++numRake) {
				endRakingTurrets.GetChild (numRake).gameObject.GetComponent<Turret> ().TurnOff ();
				endRakingTurrets.GetChild (numRake).GetChild (0).gameObject.GetComponent<Turret> ().TurnOff ();
				endRakingTurrets.GetChild (numRake).gameObject.SetActive (false);
				endRakingTurrets.GetChild (numRake).GetChild (0).gameObject.SetActive (false);
			}
			triangleTurrets.gameObject.SetActive (true);
			for (int numTri = 0; numTri < triangleTurrets.childCount; ++numTri) {
				triangleTurrets.GetChild (numTri).gameObject.GetComponent<Turret> ().TurnOn ();
				triangleTurrets.GetChild (numTri).GetChild (0).gameObject.GetComponent<Turret> ().TurnOn ();
			}*/

			reactor.GetComponent<SphereCollider>().enabled = true;
			Destroy(reactorCore);
			/*if(!activatedDoor){
				escapeDoor.GetComponent<ShatterWhenHit>().enabled = true;
				activatedDoor = true;
			}*/
			reactor.transform.GetChild(0).GetComponent<Light>().color = Color.red;
		}
	}
    public bool isVulnerable()
    {
        return reactor.GetComponent<SphereCollider>().enabled;
    }

	public void StartBattle() {
		for (int numSys = 0; numSys < lightning.childCount; ++numSys) {
			lightning.GetChild (numSys).gameObject.GetComponent<ParticleSystem> ().Stop ();
		}

		for (int numRake = 0; numRake < endRakingTurrets.childCount; ++numRake) {
			endRakingTurrets.GetChild (numRake).gameObject.GetComponent<Turret> ().TurnOn ();
			endRakingTurrets.GetChild (numRake).GetChild (0).gameObject.GetComponent<Turret> ().TurnOn ();
			midRakingTurrets.GetChild (numRake).gameObject.GetComponent<Turret> ().TurnOn ();
			midRakingTurrets.GetChild (numRake).GetChild (0).gameObject.GetComponent<Turret> ().TurnOn ();
		}

		for (int numT = 0; numT < T_Turrets.childCount; ++numT) {
			T_Turrets.GetChild (numT).gameObject.GetComponent<Turret> ().TurnOn ();
		}
	}
}