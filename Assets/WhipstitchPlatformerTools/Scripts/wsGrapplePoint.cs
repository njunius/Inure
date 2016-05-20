using UnityEngine;
using System.Collections;

public class wsGrapplePoint : MonoBehaviour {

	public bool hilight = true;
	public Color hilightColor = Color.red;
	public bool blink = true;
	[Range(0.01f, 5.0f)]
	public float blinkLength = 0.5f;

	private IEnumerator blinkTargetCoroutine;
	private Material myMaterial;
	private Color startColor;

	private void Awake() {
		myMaterial = GetComponent<Renderer>().material;
		startColor = myMaterial.color;
		blinkTargetCoroutine = blinkTarget();
	}//	End Unity method Awake

	void OnTriggerEnter2D(Collider2D coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.addGrappleTarget(transform);
	}//	End method OnTriggerEnter2D

	void OnTriggerExit2D(Collider2D coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.removeGrappleTarget(transform);
	}//	End method OnTriggerExit2D

	void OnTriggerEnter(Collider coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.addGrappleTarget(transform);
	}//	End method OnTriggerEnter

	void OnTriggerExit(Collider coll) {
		wsPlatformCharacter character = coll.gameObject.GetComponentInChildren<wsPlatformCharacter>();
		if (character == null) { return; }
		character.removeGrappleTarget(transform);
	}//	End method OnTriggerExit

	public void setHilite(bool onOff) {
		if (hilight) {
			if (blink) {
				StopCoroutine(blinkTargetCoroutine);
				blinkTargetCoroutine = blinkTarget();
				myMaterial.color = startColor;
				if (onOff) {
					StartCoroutine(blinkTargetCoroutine);
				}
			}//	End if we blink our hilighting on and off
			else {
				myMaterial.color = onOff ? hilightColor : startColor;
			}
		}//	End if we have activated object hilighting
	}//	End public method setHilite

	private IEnumerator blinkTarget() {
		float blinkCount = 0.0f;
		bool goingUp = true;
		while (blinkCount <= blinkLength && blinkCount >= 0.0f && blinkLength > 0.0f) {
			myMaterial.color = Color.Lerp(startColor, hilightColor, blinkCount / blinkLength);
			if (goingUp) {
				blinkCount += 0.05f;
				if (blinkCount > blinkLength) {
					blinkCount = blinkLength;
					goingUp = false;
				}
			}//	End if we are counting up
			else {
				blinkCount -= 0.05f;
				if (blinkCount < 0.0f) {
					blinkCount = 0.0f;
					goingUp = true;
				}
			}//	End if we are counting down
			yield return new WaitForSeconds(0.05f);
		}//	End while the coroutine is running
	}//	End private method blinkTarget

}//	End class wsGrapplePoint
