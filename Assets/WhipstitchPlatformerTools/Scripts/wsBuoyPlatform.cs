using UnityEngine;
using System.Collections;

public class wsBuoyPlatform : wsGroundableObject {

	public float gravityForce = 1.50f;
	public float maxDropLength = 0.0f;	//	If zero, drop indefinitely

	private int motionDir;	//	1 = up, -1 = down, 0 = no motion
	private Vector3 startingPosition;
	private Rigidbody2D body2D;
	private Rigidbody body3D;

	// Use this for initialization
	void Awake() {
		startingPosition = transform.position;
		motionDir = 0;
		body2D = GetComponent<Rigidbody2D>();
		if (body2D == null) {
			body3D = GetComponent<Rigidbody>();
		}
	}//	End Unity method Awake

	void FixedUpdate() {
		if (motionDir < 0) {	//	Move platform Downwards
			if (maxDropLength == 0.0f || (startingPosition.y - transform.position.y) < maxDropLength) {
				if (body2D != null) { body2D.velocity -= Vector2.up *gravityForce; }
				else if (body3D != null) { body3D.velocity -= Vector3.up *gravityForce; }
			}
			else if (maxDropLength > 0.0f && (startingPosition.y - transform.position.y) > maxDropLength) {
				transform.position = startingPosition - Vector3.up*maxDropLength;
				if (body2D != null) { body2D.velocity = Vector2.zero; }
				else if (body3D != null) { body3D.velocity = Vector3.zero; }
			}
		}
		else if (motionDir > 0) {	//	Move platform Upwards
			if (transform.position.y < startingPosition.y) {
				if (body2D != null) { body2D.velocity += Vector2.up * gravityForce; }
				else if (body3D != null) { body3D.velocity += Vector3.up * gravityForce; }
			}
			else {
				motionDir = 0;
			}
		}
		else {//	Hold us still
			if (body2D != null) { body2D.velocity = Vector2.zero; }
			else if (body3D != null) { body3D.velocity = Vector3.zero; }
			if (transform.position.y > startingPosition.y) {
				transform.position = startingPosition;
			}
		}
	}//	End Unity method FixedUpdate

	public override void wsOnGroundEnter() {
		motionDir = -1;	//	Begin Going Down
	}//	End wsGroundableObject method wsOnGroundEnter

	public override void wsOnGroundExit() {
		motionDir = 1;	//	Begin Going Up
	}//	End wsGroundableObject method wsOnGroundExit

	public override Vector3 getVelocity() {
		if (body2D != null) {
			if (motionDir >= 0) {
				body2D.velocity = Vector2.zero;
			}
			return body2D.velocity;
		}
		if (motionDir >= 0) {
			body3D.velocity = Vector3.zero;
		}
		return body3D.velocity;
	}//	End wsGroundableObject method getVelocity

}//	End class wsBuoyPlatform
