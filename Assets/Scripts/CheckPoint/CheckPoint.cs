using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	public Vector3 savedPOS;
	public Quaternion savedROT;
	public float shieldCharge;
	public int bombCharge;
	public int hullHealth;
	public int heal_player = 1;

    public bool tutorialCheckpoint = false;

	private LastCheckpoint pData;
	private PlayerController pController;
	private bool hasHealed = false;



    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        if (!tutorialCheckpoint) audioSource = GetComponent<AudioSource>();
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
            if (!tutorialCheckpoint) audioSource.Play();
			if(!hasHealed && pController.getCurrHullIntegrity() < pController.getMaxHullIntegrity()){
				pController.setHullIntegrity(pController.getCurrHullIntegrity() + heal_player);
				hasHealed = true;
			}

			savedROT = GameObject.FindGameObjectWithTag("Player").transform.rotation;
			shieldCharge = pController.getShieldCharge();
			hullHealth = pController.getCurrHullIntegrity();
			pData.setCheckPoint(shieldCharge, bombCharge, hullHealth, savedPOS, savedROT);

            if (!tutorialCheckpoint) transform.GetChild(1).GetComponent<Light>().color = Color.red;
            if (!tutorialCheckpoint) transform.GetChild(2).GetComponent<Light>().color = Color.red;
			bombCharge = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>().currBombCharge;
		}
	}
}
