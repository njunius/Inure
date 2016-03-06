using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	protected bool isOn = false;
	protected bool isEMP = false;
	protected bool isFiring = false;
	protected bool isSlowed = false;
	protected float health = 100;
	public float healthDecrement = 10f;
	protected bool isDead = false;
	public float fireRate;
	protected float fireDelay = 0f;
	private float effectDuration = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOn () {
		if(!isDead)
			isOn = true;
	}

	public void TurnOff () {
		isOn = false;
	}

	public bool GetIsEMP () {
		return isEMP;
	}

	public void EMP () {
		isEMP = true;
		Invoke ("UnEMP", effectDuration);
	}

	private void UnEMP () {
		isEMP = false;
	}

	private void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.name == "PlayerBullet(Clone)") {
			health = Mathf.Max (health - healthDecrement, 0f);
		}
	}
}
