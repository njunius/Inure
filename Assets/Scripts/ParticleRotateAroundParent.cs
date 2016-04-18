using UnityEngine;
using System.Collections;

public class ParticleRotateAroundParent : MonoBehaviour {

	public Transform target;
	public float RotationSpeed = 10f;
	public float OrbitDegrees = 1f;

	void Update () {
		transform.Rotate (Vector3.up, RotationSpeed * Time.deltaTime);
		transform.RotateAround (target.position, transform.forward, OrbitDegrees);
	}
}
