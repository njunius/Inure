/*
 * Bullet.cs
 * 
 * Defines functionality of individual bullet object:
 * - setting of color and velocity on instantiation
 * - reaction to collision with other GameObjects
 */

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//private Vector3 velocity;

    private int absorbValue;
	private float lifeTime;
    private AudioSource bulletHum;

    public Renderer brackets;

	// Use this for initialization
	void Awake () {
        absorbValue = 1;
        bulletHum = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    /*
	 * Description: Sets values for empty vars
	 * post: color and velocity of bullet are set
	 */
    public void setVars (Color bColor, Vector3 newVel) {
		if (bColor != Color.clear)
			gameObject.GetComponent<Renderer> ().material.color = bColor;
		if(newVel != null)
        	gameObject.GetComponent<Rigidbody> ().velocity = newVel;
    }

	public void setLifeTime (float time) {
		lifeTime = time;
	}

    void OnCollisionEnter (Collision hit) {
        PlayerController hitScript = hit.gameObject.GetComponent<PlayerController>();
		if (hit.gameObject.CompareTag ("Player")) {
            if (hit.collider.GetComponent<CollisionHitDetector>())
            {
                hit.collider.GetComponent<CollisionHitDetector>().updateIndicators();
            }
            if (!hitScript.isShielded ()) {
				hitScript.takeDamage ();
			}
		} else if (!hit.gameObject.CompareTag ("Projectile")) {
			ContactPoint contact = hit.contacts [0];
			GameObject particles = ObjectPooler.current.GetPooledObject2 ();
			particles.transform.position = contact.point + contact.normal * 2;
			particles.transform.rotation = Quaternion.FromToRotation (Vector3.forward, contact.normal);
			//GameObject particles = (GameObject) Instantiate (Resources.Load ("Particle Systems/Bullet Collision"), contact.point + contact.normal * 2, Quaternion.FromToRotation (Vector3.forward, contact.normal));
			//particles.transform.GetChild (0).gameObject.transform.rotation = particles.transform.rotation;
			particles.GetComponent<ParticleSystem> ().startColor = GetComponent<Renderer> ().material.color;
			particles.transform.GetChild(0).GetComponent<ParticleSystem> ().startColor = GetComponent<Renderer> ().material.color;
			particles.SetActive (true);
			particles.GetComponent<ParticleSystem> ().Play ();
		}
        //Destroy (gameObject);
		Destroy();
		//gameObject.SetActive(false);
	}

    void OnTriggerEnter(Collider volume)
    {
        if (volume.gameObject.CompareTag("Warning Radius"))
        {
            brackets.enabled = true;
            bulletHum.enabled = true;
        }

    }

    void OnTriggerExit(Collider volume)
    {
        if (volume.gameObject.CompareTag("Warning Radius"))
        {
            brackets.enabled = false;
            bulletHum.enabled = false;
        }
    }

    public int getAbsorbValue()
    {
        return absorbValue;
    }

	void OnEnable() {
		Invoke ("Destroy", lifeTime);
		//GameObject.FindGameObjectWithTag ("Object Pooler").GetComponent<ObjectPooler> ().numActiveObj++;
	}

	public void Destroy() {
		brackets.enabled = false;
		GameObject radar = GameObject.FindGameObjectWithTag ("Radar3D");
		if (radar) {
				radar.GetComponent<Radar3D> ().RemoveBlip (gameObject);
		}
        GameObject[] threatQuadrants = GameObject.FindGameObjectsWithTag("Threat Quadrant");
        for(int i = 0; i < threatQuadrants.Length; ++i)
        {
            threatQuadrants[i].GetComponent<ThreatTriggerController>().removeListElement(gameObject);
        }
        gameObject.SetActive (false);
	}

	void OnDisable() {
		CancelInvoke ();
		//GameObject.FindGameObjectWithTag ("Object Pooler").GetComponent<ObjectPooler> ().numActiveObj--;
	}

	public void SlowTime (float timeScale) {
		gameObject.GetComponent<Rigidbody> ().velocity *= timeScale;
	}

	public void QuickTime (float timeScale) {
		gameObject.GetComponent<Rigidbody> ().velocity /= timeScale;
	}
}