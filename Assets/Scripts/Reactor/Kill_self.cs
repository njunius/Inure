using UnityEngine;
using System.Collections;

public class Kill_self : MonoBehaviour {
	public float lifeTime = 4; // The ammount of time left before self destruction in seconds

	private float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(timer >= lifeTime)
			Destroy(gameObject);
	}
}
