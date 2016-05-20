using UnityEngine;
using System.Collections;

public class wsLibraPlatform : wsGroundableObject {

	public float gravityForce = 1.50f;
	public float maxDropLength = 0.0f;	//	If zero, drop indefinitely
	public wsLibraPlatform twinPlatform;

	private int motionDir;	//	1 = up, -1 = down, 0 = no motion
	private Vector3 startingPosition;
	private Rigidbody2D body2D;
	private Rigidbody body3D;
	private bool mirroring = false;

	// Use this for initialization
	private void Awake() {
		startingPosition = transform.position;
		motionDir = 0;
		body2D = GetComponent<Rigidbody2D>();
		if (body2D == null) {
			body3D = GetComponent<Rigidbody>();
		}
	}//	End Unity method Awake

	private void FixedUpdate() {
		if (mirroring) { return; }
		if (motionDir < 0) {	//	Move platform Downwards
			if (maxDropLength == 0.0f || (startingPosition.y - transform.position.y) < maxDropLength) {
				if (body2D != null) { setVelocity(body2D.velocity - Vector2.up * gravityForce); }
				else if (body3D != null) { setVelocity3D(body3D.velocity - Vector3.up * gravityForce); }
			}
			else if (maxDropLength > 0.0f && (startingPosition.y - transform.position.y) > maxDropLength) {
				maxPosition();
				if (body2D != null) { setVelocity(Vector2.zero); }
				else if (body3D != null) { setVelocity3D(Vector3.zero); }
			}
		}
		else if (motionDir > 0) {	//	Move platform Upwards
			if (transform.position.y < startingPosition.y) {
				if (body2D != null) { setVelocity(body2D.velocity + Vector2.up * gravityForce); }
				else if (body3D != null) { setVelocity3D(body3D.velocity + Vector3.up * gravityForce); }
			}
			else {
				motionDir = 0;
			}
		}
		else if (body2D != null && body2D.velocity.y != 0.0f) {//	Hold us still
			setVelocity(Vector2.zero);
			if (transform.position.y > startingPosition.y) {
				resetPosition();
			}
		}
		else if (body3D != null && body3D.velocity.y != 0.0f) {//	Hold us still
			setVelocity3D(Vector3.zero);
			if (transform.position.y > startingPosition.y) {
				resetPosition();
			}
		}
	}//	End Unity method FixedUpdate

	public void setMirroring(bool newMirroring) { mirroring = newMirroring; }

	public void maxPosition(bool notTwin = true) {
		if (notTwin) {
			transform.position = startingPosition - Vector3.up * maxDropLength;
			if (twinPlatform != null) {
				twinPlatform.maxPosition(false);
			}
		}
		else {
			transform.position = startingPosition + Vector3.up * maxDropLength;
		}
	}//	End method maxPosition

	public void resetPosition(bool notTwin = true) {
		transform.position = startingPosition;
		mirroring = false;
		if (notTwin && twinPlatform != null) {
			twinPlatform.resetPosition(false);
		}
	}//	End method resetPosition

	public void setVelocity(Vector2 newVel, bool notTwin = true) {
		body2D.velocity = newVel;
		if (notTwin && twinPlatform != null) {
			twinPlatform.setVelocity(-newVel, false);
		}
	}//	End method setVelocity

	public void setVelocity3D(Vector3 newVel, bool notTwin = true) {
		body3D.velocity = newVel;
		if (notTwin && twinPlatform != null) {
			twinPlatform.setVelocity3D(-newVel, false);
		}
	}//	End method setVelocity3D

	public override void wsOnGroundEnter() {
		motionDir = -1;	//	Begin Going Down
		mirroring = false;
		twinPlatform.setMirroring(true);
	}//	End wsGroundableObject method wsOnGroundEnter

	public override void wsOnGroundExit() {
		motionDir = 1;	//	Begin Going Up
		mirroring = false;
		twinPlatform.setMirroring(true);
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

}//	End class wsLibraPlatform
