/*
 * ParticlePooler.cs
 * 
 * Creates custom-sized pool of given particle system on startup in order to save processing power at runtime
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePooler : MonoBehaviour {

	public static ParticlePooler current;
	public GameObject pooledParticle;
	public int pooledAmount = 10;
	public bool willGrow = false;
	public int numActiveObj = 0;

	public List<GameObject> pooledParticles;
	public List<GameObject> particleCopies;

	void Awake () {
		current = this;
	}

	// Use this for initialization
	void Start () {
		pooledParticles = new List<GameObject> ();
		particleCopies = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; ++i) {
			GameObject obj = (GameObject)Instantiate (pooledParticle);
			GameObject obj2 = (GameObject)Instantiate (pooledParticle);
			obj.SetActive (false);
			pooledParticles.Add (obj);
			particleCopies.Add (obj2);
		}
	}

	/*
	 * Description: Searches through the object pool, looking for one that is not being used
	 * Post: If pool contains unused object -> return unused object
	 *       If pool does not contain unused object -> If pool is allowed to grow -> Increase size of pool by one, return new object
	 *                                                 If pool is not allowed to grow -> return nothing
	 */
	public GameObject GetPooledParticle() {
		for (int i = 0; i < pooledParticles.Count; ++i) {
			if (!pooledParticles [i].activeInHierarchy) {
				return pooledParticles [i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledParticle);
			GameObject obj2 = (GameObject)Instantiate (pooledParticle);
			pooledParticles.Add (obj);
			particleCopies.Add (obj2);
			return obj;
		}

		return null;
	}

	public void ReassignTransform(GameObject particles) {
		int index = pooledParticles.IndexOf(particles);
		particles = particleCopies [index];
	}
}
