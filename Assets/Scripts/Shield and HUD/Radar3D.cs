/*
 * Radar3D.cs
 * 
 * Generates "blips" inside of a 3D radar system (a sphere) according to
 * the existence of bullets within the vicinity of the player, with color
 * of the blips indicative of distance behind or in front of the player
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar3D : MonoBehaviour {

	public GameObject blipPrefab;
	List<RadarBlip> blipList = new List<RadarBlip> ();
	float triggerRadius;
	float radarRadius;

	// Use this for initialization
	void Start () {
		triggerRadius = GameObject.FindGameObjectWithTag ("Radar Trigger").GetComponent<SphereCollider> ().radius;
		radarRadius = gameObject.GetComponent<Renderer> ().bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int numBlip = 0; numBlip < blipList.Count; ++numBlip) {
			blipList [numBlip].GetBlip ().transform.position = WorldToLocal (blipList [numBlip].GetOrig ());
		}
	}

	public void AddBlip (GameObject obj) {
		//convert word space to local space
		Vector3 position = WorldToLocal(obj);

		//add object to list of tracked objects
		blipList.Add (new RadarBlip(obj, (GameObject) Instantiate(blipPrefab, position, new Quaternion(0f, 0f, 0f, 0f))));
		blipList [blipList.Count - 1].GetBlip ().transform.parent = transform;
	}

	/*
	 * Description: find a particular object in the list of tracked objects,
	 * and remove it from the list
	 * 
	 * Pre: The list is not empty
	 * Post: One fewer objects are in the list
	 */
	public void RemoveBlip (GameObject obj) {
		for (int numBlip = 0; numBlip < blipList.Count; ++numBlip) {
			if (blipList [numBlip].isEqualTo (obj)) {
				Destroy (blipList[numBlip].GetBlip());
				blipList.Remove (blipList[numBlip]);
				break;
			}
		}
	}

	private Vector3 WorldToLocal (GameObject obj) {
		return transform.position + ((obj.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position) / triggerRadius * radarRadius);
	}
}
