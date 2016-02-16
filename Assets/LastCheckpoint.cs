using UnityEngine;
using System.Collections;

public class LastCheckpoint : MonoBehaviour {

	public Vector3 savedPOS;
	public float shieldCharge;
	public int bombCharge;
	public float hullHealth;

	private PlayerController cont;
	private BombController bomb;

	// Use this for initialization
	void Start () {
		savedPOS = gameObject.transform.position;
		cont = gameObject.GetComponent<PlayerController> ();
		bomb = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombController>();
		bombCharge = bomb.currBombCharge;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
