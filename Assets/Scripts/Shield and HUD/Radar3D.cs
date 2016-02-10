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
		triggerRadius = GameObject.FindGameObjectWithTag ("Warning Radius").GetComponent<SphereCollider> ().radius;
		radarRadius = gameObject.GetComponent<Renderer> ().bounds.extents.x;
		Debug.Log (radarRadius);
	}
	
	// Update is called once per frame
	void Update () {
		for (int numBlip = 0; numBlip < blipList.Count; ++numBlip) {
			blipList[numBlip].GetBlip().transform.position = transform.position + ((blipList[numBlip].GetOrig().transform.position - GameObject.FindGameObjectWithTag("Player").transform.position) / triggerRadius * radarRadius);
		}
	}

	public void AddBlip (GameObject obj) {
		Vector3 position = transform.position + ((obj.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position) / triggerRadius * radarRadius);

		blipList.Add (new RadarBlip(obj, (GameObject) Instantiate(blipPrefab, position, new Quaternion(0f, 0f, 0f, 0f))));
		blipList [blipList.Count - 1].GetBlip ().transform.parent = transform;
	}

	//REDO
	public void RemoveBlip (GameObject obj) {
		for (int numBlip = 0; numBlip < blipList.Count; ++numBlip) {
			if (blipList [numBlip].isEqualTo (obj)) {
				Destroy (blipList[numBlip].GetBlip());
				blipList.Remove (blipList[numBlip]);
				break;
			}
		}
	}
}
