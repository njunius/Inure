using UnityEngine;
using System.Collections;

public class ScaleSlowTimeField : MonoBehaviour {

	private Transform clockwiseCenter;
	private Transform clockwiseUp;
	private Transform clockwiseDown;
	private Transform counterUp;
	private Transform counterDown;

	private float centerScale = 5f / 3f;
	private float[] clockUps = new float[11];
	private float[] clockDowns = new float[11];
	private float[] counterUps = new float[12];
	private float[] counterDowns = new float[12];

	// Use this for initialization
	void Start () {
		clockwiseCenter = transform.FindChild ("Clockwise Center");
		clockwiseUp = transform.FindChild ("Clockwise Up");
		clockwiseDown = transform.FindChild ("Clockwise Down");
		counterUp = transform.FindChild ("Counter Clockwise Up");
		counterDown = transform.FindChild ("Counter Clockwise Down");

		for (int numObj = 0; numObj < 11; ++numObj) {
			clockUps [numObj] = clockwiseUp.FindChild ("Clockwise Up (" + numObj + ")").localScale.x / 3f;
		}
		for (int numObj = 0; numObj < 11; ++numObj) {
			clockDowns [numObj] = clockwiseDown.FindChild ("Clockwise Down (" + numObj + ")").localScale.x / 3f;
		}
		for (int numObj = 0; numObj < 12; ++numObj) {
			counterUps [numObj] = counterUp.FindChild ("Counter Clockwise Up (" + numObj + ")").localScale.x / 3f;
		}
		for (int numObj = 0; numObj < 12; ++numObj) {
			counterDowns [numObj] = counterDown.FindChild ("Counter Clockwise Down (" + numObj + ")").localScale.x / 3f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		clockwiseCenter.localScale = transform.localScale * centerScale;
		clockwiseCenter.GetChild (0).localScale = transform.localScale;
		for (int numSystem = 0; numSystem < 11; ++numSystem) {
			Transform particles = clockwiseUp.FindChild ("Clockwise Up (" + numSystem + ")");
			particles.localScale = transform.localScale * clockUps [numSystem];
			particles.GetChild (0).localScale = transform.localScale;
		}
		for (int numSystem = 0; numSystem < 11; ++numSystem) {
			Transform particles = clockwiseDown.FindChild ("Clockwise Down (" + numSystem + ")");
			particles.localScale = transform.localScale * clockDowns[numSystem];
			particles.GetChild (0).localScale = transform.localScale;
		}
		for (int numSystem = 0; numSystem < 12; ++numSystem) {
			Transform particles = counterUp.FindChild ("Counter Clockwise Up (" + numSystem + ")");
			particles.localScale = transform.localScale * counterUps[numSystem];
			particles.GetChild (0).localScale = transform.localScale;
		}
		for (int numSystem = 0; numSystem < 12; ++numSystem) {
			Transform particles = counterDown.FindChild ("Counter Clockwise Down (" + numSystem + ")");
			particles.localScale = transform.localScale * counterDowns[numSystem];
			particles.GetChild (0).localScale = transform.localScale;
		}
	}
}
