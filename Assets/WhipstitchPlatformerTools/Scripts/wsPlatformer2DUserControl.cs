using UnityEngine;

public enum ButtonFlags : int {
	Crouch = 1,
	Jump = 2,
	DashLeft = 4,
	DashRight = 8,
	Run = 16,
	Grapple = 32,
	ReleaseJump = 64	//	Feature Added by Vincent Chevalier
}//	End enum ButtonFlags

[RequireComponent(typeof(wsPlatformCharacter))]
public class wsPlatformer2DUserControl : MonoBehaviour {

	public bool toggleCrouch = false;
	public bool cameraRelativeMovement = false;

	private wsPlatformCharacter character;
	private ButtonFlags buttonFlags;
	private bool crouchActive = false;
	private bool crouchReleased = true;

	void Awake() {
		character = GetComponent<wsPlatformCharacter>();
	}//	End method Awake

	void Update () {
		// Read the button inputs in Update so button presses aren't dropped
		if (Input.GetButtonDown("Jump")) { buttonFlags |= ButtonFlags.Jump; }
		if (Input.GetButtonUp("Jump")) { buttonFlags |= ButtonFlags.ReleaseJump; }//	Feature added by Vincent Chevalier
		if (Input.GetButtonDown("LeftBumper")) { buttonFlags |= ButtonFlags.DashLeft; }
		if (Input.GetButtonDown("RightBumper")) { buttonFlags |= ButtonFlags.DashRight; }
		if (Input.GetButton("Fire1")) { buttonFlags |= ButtonFlags.Run; }
		if (Input.GetButtonDown("Fire2")) { buttonFlags |= ButtonFlags.Grapple; }
		// if (Input.GetKeyDown(KeyCode.Alpha0)) { character.respawn(); }	//	Useful function for debugging
	}//	End method Update

	void FixedUpdate() {
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (cameraRelativeMovement) { movement = Quaternion.Inverse(transform.rotation) * movement; }
		if (movement.y < -0.1f) {
			if (toggleCrouch) {
				if (crouchReleased) {
					crouchActive = !crouchActive;
					crouchReleased = false;
				}
			}
			else { buttonFlags |= ButtonFlags.Crouch; }
		}
		else {
			crouchReleased = true;
		}
		if (toggleCrouch && crouchActive) { buttonFlags |= ButtonFlags.Crouch; }

		// Pass all parameters to the character control script.
		character.move(movement, buttonFlags);

		character.setMusicState(buttonFlags);

		// Reset the inputs once they've been used
		buttonFlags = 0;
	}//	End method FixedUpdate

}//	End method wsPlatformer2DUserControl
