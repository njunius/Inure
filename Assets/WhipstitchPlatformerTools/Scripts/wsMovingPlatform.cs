using UnityEngine;
using System.Collections;

public class wsMovingPlatform : wsGroundableObject {

	[SerializeField] private Transform startPos;
	[SerializeField] private Transform endPos;
	[SerializeField] private float movementInterval;
	private Vector3 movementVelocity;
	private float timeCounter;
	bool movingToEnd = true;

	void Awake() {
		if (movementInterval <= 0.0f) {
			movementVelocity = Vector3.zero;
		}
		else {
			movementVelocity = (endPos.position - startPos.position) / movementInterval;
		}
		timeCounter = 0.0f;
	}//	End method Awake

	void Update() {
		transform.position += movementVelocity * Time.deltaTime;
		timeCounter += Time.deltaTime;
		if (timeCounter >= movementInterval) {
			if (movingToEnd) { transform.position = endPos.position; }
			else { transform.position = startPos.position; }
			movingToEnd = !movingToEnd;
			movementVelocity *= -1.0f;
			timeCounter = 0.0f;
		}
	}//	End method Update

	public override Vector3 getVelocity() { return movementVelocity; }
	public override void wsOnGroundEnter() { }
	public override void wsOnGroundExit() { }

}//	End class wsMovingPlatform
