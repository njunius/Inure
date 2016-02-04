/*
 * ObjectPooler.cs
 * 
 * Creates custom-sized pool of given object on startup in order to save processing power at runtime
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler current;
	public GameObject pooledObject;
	public int pooledAmount = 100;
	public bool willGrow = false;

	public List<GameObject> pooledObjects;

	void Awake () {
		current = this;
	}

	// Use this for initialization
	void Start () {
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; ++i) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}

	/*
	 * Description: Searches through the object pool, looking for one that is not being used
	 * Post: If pool contains unused object -> return unused object
	 *       If pool does not contain unused object -> If pool is allowed to grow -> Increase size of pool by one, return new object
	 *                                                 If pool is not allowed to grow -> return nothing
	 */
	public GameObject GetPooledObject() {
		for (int i = 0; i < pooledObjects.Count; ++i) {
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pooledObjects.Add (obj);
			return obj;
		}

		return null;
	}
}
