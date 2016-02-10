using UnityEngine;
using System.Collections;

public class RadarBlipControl : MonoBehaviour {

	private Renderer radarRenderer;
	private float colorScale;

	// Use this for initialization
	void Start () {
		radarRenderer = GameObject.FindGameObjectWithTag("Radar3D").GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		colorScale = (transform.localPosition.z + radarRenderer.bounds.extents.x) / radarRenderer.bounds.size.x;
		//Debug.Log (transform.localPosition.z +", " + radarRenderer.bounds.extents.x + ", " + radarRenderer.bounds.size.x);
		transform.LookAt (GameObject.FindGameObjectWithTag ("MainCamera").transform);
		float redVal = ((colorScale < 0.5) ? 1f : 2f);
		if (colorScale <= 0.5) {
			gameObject.GetComponent<Renderer> ().material.color = new Color (1f, 1f - 2 * colorScale, 1f - 2 * colorScale);
		} else {
			colorScale = transform.localPosition.z / radarRenderer.bounds.extents.x;
			gameObject.GetComponent<Renderer> ().material.color = new Color (1f - colorScale, 0f, 0f);
		}
	}
}
