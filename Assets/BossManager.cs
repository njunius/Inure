using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour {
	public GameObject reactor;
	public GameObject[] shieldPieces;
	public int turretInterval = 4;

	private int shieldCount = 32;
	// Use this for initialization
	void Start () {
		shieldCount = shieldPieces.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
