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
    public bool finalCheckpoint = false;

	private LastCheckpoint pData;
	private PlayerController pController;
	private bool hasHealed = false;


    public AudioClip standard;
    public AudioClip final;

    private AudioSource audioSource;
    private bool soundHasPlayed = false;

	private Transform particles;

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
		particles = transform.FindChild ("Particles");
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player Collider")){
            
			if(!hasHealed && pController.getCurrHullIntegrity() < pController.getMaxHullIntegrity()){
				pController.setHullIntegrity(pController.getCurrHullIntegrity() + heal_player);
				hasHealed = true;
			}
            if (!tutorialCheckpoint && !soundHasPlayed)
            {
                if (!finalCheckpoint)
                {
                    audioSource.PlayOneShot(standard);
                }
                else
                {
                    audioSource.PlayOneShot(final);
                }
                soundHasPlayed = true;
            }
            savedROT = GameObject.FindGameObjectWithTag("Player").transform.rotation;
			shieldCharge = pController.getShieldCharge();
			hullHealth = pController.getCurrHullIntegrity();
			pData.setCheckPoint(shieldCharge, bombCharge, hullHealth, savedPOS, savedROT);

            if (!tutorialCheckpoint) transform.GetChild(1).GetComponent<Light>().color = Color.red;
            if (!tutorialCheckpoint) transform.GetChild(2).GetComponent<Light>().color = Color.red;
			bombCharge = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>().currBombCharge;

			for (int numSys = 0; numSys < particles.childCount; ++numSys) {
				particles.GetChild (numSys).GetComponent<ParticleSystem> ().startColor = Color.red;
			}
		}
	}
}
