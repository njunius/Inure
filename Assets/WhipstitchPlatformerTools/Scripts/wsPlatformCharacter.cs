using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using DpsInterpreter;

public class BitMaskAttribute : PropertyAttribute {
	public System.Type propType;
	public BitMaskAttribute(System.Type aType) {
		propType = aType;
	}
}

[Flags]
public enum Abilities : int {
	Jump = 1,
	DoubleJump = 2,
	LedgeGrab = 4,
	Crouch = 8,
	WallJump = 16,
	AirControl = 32,
	JumpHeightControl = 64,
	LedgeJump = 128,	//	Jump out of a ledge grab (otherwise jump button just pulls character up)
	PassThroughPlatforms = 256,	//	Down + Jump lets character drop through one-way platforms
	AirDash = 512,
	Run = 1024,
	WallSlide = 2048,	//	Slows character down and provides grop when next to a wall-jump trigger
	Grapple = 4096,
	GravitySwitch = 8192
	//SlideDownSlopes = 4096,	//	A la Mario 3. To Be Implemented
	//ClimbLadders = 8192	//	To Be Implemented
}//	End enum Abilities

public class wsPlatformCharacter : MonoBehaviour {

	public float maxSpeed = 10.0f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1400.0f;			// Amount of force added when the player jumps.
	public float wallJumpForce = 1000.0f;
	public float verticalWallJumpForce = 1000.0f;	//	Feature added by Vincent Chevalier
	public float doubleJumpForce = 1400.0f;
	public float gravityForce = 1.25f;
	public float airDrag = 15.0f;
	public float stepHeight = 0.25f;
	[BitMask(typeof(Abilities))] public Abilities abilities = Abilities.Jump | Abilities.DoubleJump | Abilities.LedgeGrab;
	[Range(0, 1)] public float crouchSpeed = 0.36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	public LayerMask whatIsGround;			// A mask determining what is ground to the character
	public float runSpeedScalar = 2.0f;
	public float wallSlideScalar = 2.0f;
	public float turnLength = 0.1f;
	public GameObject ropePrefab;
	public Transform ropeAttachPoint;//	Point on the character the rope attaches to
	// public bool wallJumpOnAnySurface = false;

	private enum ColliderType {
		box2d,
		box3d,
		capsule
	}//	End enum ColliderType

	private Vector3 respawnPos;
	private Transform groundCheck;								// A position marking where to check if the player is grounded.
	private Transform ceilingCheck;								// A position marking where to check for ceilings
	[SerializeField] private float groundedRadius = 0.1f;	// Radius of the overlap circle to determine if grounded
	private float ceilingRadius = 0.3f;							// Radius of the overlap circle to determine if the player can stand up
	private Animator anim;										// Reference to the player's animator component.
	[SerializeField] private Transform modelTransform;
	private Transform ledgeGrabLeft;
	private Transform ledgeGrabRight;
	private Transform grabbedLedge;
	private Transform grabbedLedgePullup;
	private wsMovingPlatform attachedPlatform = null;
	private wsWallJumpTrigger attachedWallTrigger = null;
	private BoxCollider2D myCollider = null;
	private BoxCollider myCollider3D = null;
	private CapsuleCollider myCapsule = null;
	private ColliderType colliderType;
	private Rigidbody2D body2D = null;
	private Rigidbody body3D = null;
	private Vector3 prevColliderCenter;
	private Vector3 prevColliderSize;
	private wsGroundableObject currentGround = null;
	private wsGrapplePoint grappleTarget = null;
	private wsGrapplePoint currentGrapple = null;
	private LinkedList<Transform> allGrappleTargets = new LinkedList<Transform>();
	private GameObject ropeObject;
	private float swingSpeed = 0.0f;
	private Vector3 gravityDir = -Vector3.up;

	//	Movement states
	private bool facingRight = true;
	private bool inputReady = true;
	private bool snappingToPosition = false;
	private bool groundTestDisabled = false;
	private int numJumps = 0;
	private int maxJumps = 2;
	private enum MovementState : int {
		Grounded,
		AirDashing,
		GrabbingLedge,
		KnockedBack,
		FreeFalling,
		WallSliding,
		PullingFromLedge,
		Grappling
	}//	End enum MovementState

	private MovementState state;

	private enum AnimState : int {
		Grounded,
		AirDashing,
		Hanging,
		Jumping,
		Falling,
		DoubleJumping,
		WallSliding,
		PullingUp,
		Swinging
	}//	End enum AnimState

	private AnimState animState;

	void Start() {

		// set up DPS
		setUpDPS();

		// Setting up references.
		facingRight = true;
		inputReady = true;
		state = MovementState.FreeFalling;
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		ledgeGrabLeft = transform.Find("LeftLedgeGrab");
		ledgeGrabRight = transform.Find("RightLedgeGrab");
		myCollider = transform.GetComponentInChildren<BoxCollider2D>();
		if (myCollider != null) {
			colliderType = ColliderType.box2d;
			body2D = GetComponent<Rigidbody2D>();
		}
		else {
			myCollider3D = transform.GetComponentInChildren<BoxCollider>();
			if (myCollider3D != null) {
				colliderType = ColliderType.box3d;
			}
			else {
				colliderType = ColliderType.capsule;
				myCapsule = transform.GetComponentInChildren<CapsuleCollider>();
			}
			body3D = GetComponent<Rigidbody>();
		}
		prevColliderSize = getColliderSize();
		prevColliderCenter = getColliderCenter();
		anim = GetComponentInChildren<Animator>();
		faceRight();
		respawnPos = transform.position;
	}//	End Unity method Start

	void FixedUpdate() {
		if (hasAbility(Abilities.Grapple)) {
			seekGrappleTarget();
		}//	End if the character can use a grappling hook
		if (!snappingToPosition && (state == MovementState.FreeFalling || state == MovementState.KnockedBack || state ==MovementState.WallSliding)) {
			//	Apply gravitational force
			applyGravity();
		}
		//	Snap to the ground
		performGroundTest();
		if (state == MovementState.Grounded) {
			AgnosticHit hit = castRay(transform.position + transform.up, -transform.up, 1.0f+groundedRadius);
			if (hit != null && getDirectionSide(hit.normal) == 0.0f && hit.distance > 1.0f) {
				//	We hit a perpendicular ground; snap to the ground
				if (Vector3.Dot(transform.up, getVelocity()) <= 0.0f) {
					transform.position -= transform.up*(hit.distance-1.0f);
					haltUpwardMovement();
				}
			}
		}//	End if the character is grounded
	}//	End Unity method FixedUpdate

	void OnCollisionEnter2D() {
		if (state == MovementState.AirDashing) {
			state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
		}
	}//	End Unity method OnCollisionEnter2D

	void OnCollisionEnter() {
		if (state == MovementState.AirDashing) {
			state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
		}
	}//	End Unity method OnCollisionEnter

	//	Update animation states
	void Update() {
		setAnimState();
	}//	End Unity method Update

	private AgnosticHit groundData = null;

	private void performGroundTest() {
		if (groundTestDisabled) { return; }
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		bool wasGrounded = (state == MovementState.Grounded);
		//float prevDistance = groundData != null ? groundData.distance : Mathf.Infinity;
		groundData = castRay(transform.position + transform.up, -transform.up, 50.0f);
		if (groundData != null && groundData.distance < 1.0f+groundedRadius) {	//	The player is grounded
			numJumps = 0;
			state = MovementState.Grounded;
			if (!wasGrounded || currentGround == null) {
				currentGround = groundData.transform.GetComponent<wsGroundableObject>();
				if (currentGround != null) { currentGround.wsOnGroundEnter(); }
			}
			else if (currentGround != null && currentGround.transform != groundData.transform) {
				currentGround.wsOnGroundExit();
				currentGround = groundData.transform.GetComponent<wsGroundableObject>();
				if (currentGround != null) { currentGround.wsOnGroundEnter(); }
			}
		}
		else {
			groundData = null;
			if (wasGrounded) { state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding; }
		}
		if (state == MovementState.Grounded) { detachGrapple(); }
	}//	End method performGroundTest

	private void applyGravity() {
		Vector3 newVelocity = getVelocity();
		if (Vector3.Dot(newVelocity, transform.up) < 0.0f && hasAbility(Abilities.WallSlide) && attachedWallTrigger != null) {
			newVelocity = gravityDir*gravityForce*wallSlideScalar;
		}
		else {
			newVelocity += gravityDir*gravityForce;
		}
		setVelocity(newVelocity);
	}//	End method applyGravity

	public void setAnimState() {
		switch (state) {
			case MovementState.Grounded:
				animState = AnimState.Grounded;
				break;
			case MovementState.AirDashing:
				animState = AnimState.AirDashing;
				break;
			case MovementState.GrabbingLedge:
				animState = AnimState.Hanging;
				break;
			case MovementState.FreeFalling:
				//animState = AnimState.Grounded;
				if (numJumps > 1) { animState = AnimState.DoubleJumping; }
				else if (numJumps > 0) { animState = AnimState.Jumping; }
				else { animState = AnimState.Falling; }
				break;
			case MovementState.WallSliding:
				animState = AnimState.WallSliding;
				break;
			case MovementState.PullingFromLedge:
				animState = AnimState.PullingUp;
				break;
			case MovementState.Grappling:
				animState = AnimState.Swinging;
				break;
		}//	End switch movement state
		updateAnimator(animState);
	}//	End method setAnimState

	private void updateAnimator(AnimState myState) {
		anim.SetBool("Grounded", false);
		anim.SetBool("AirDashing", false);
		anim.SetBool("Hanging", false);
		anim.SetBool("Jumping", false);
		anim.SetBool("DoubleJumping", false);
		anim.SetBool("WallSliding", false);
		anim.SetBool("PullingUp", false);
		anim.SetBool("Swinging", false);
		switch (myState) {
			case AnimState.Grounded:
				anim.SetBool("Grounded", true);
				break;
			case AnimState.AirDashing:
				anim.SetBool("AirDashing", true);
				break;
			case AnimState.Hanging:
				anim.SetBool("Hanging", true);
				break;
			case AnimState.Jumping:
				anim.SetBool("Jumping", true);
				break;
			case AnimState.DoubleJumping:
				anim.SetBool("DoubleJumping", true);
				break;
			case AnimState.WallSliding:
				anim.SetBool("WallSliding", true);
				break;
			case AnimState.PullingUp:
				anim.SetBool("PullingUp", true);
				break;
			case AnimState.Swinging:
				anim.SetBool("Swinging", true);
				break;
		}//	End switch anim state
	}//	End method updateAnimator

	public void addGrappleTarget(Transform newPoint) {
		allGrappleTargets.AddLast(newPoint);
	}//	End public method setGrappleTarget

	public void removeGrappleTarget(Transform newPoint) {
		if (currentGrapple != null && newPoint == currentGrapple.transform) {
			detachGrapple();
		}
		if (grappleTarget != null && newPoint == grappleTarget.transform) {
			grappleTarget.setHilite(false);
			grappleTarget = null;
		}
		allGrappleTargets.Remove(newPoint);
	}//	End method removeGrappleTarget

	public void seekGrappleTarget() {
		int numTargets = allGrappleTargets.Count;
		if (numTargets > 0 && state == MovementState.FreeFalling) {
			float closestDist = Mathf.Infinity;
			bool targetChanged = false;
			for (LinkedListNode<Transform> node = allGrappleTargets.First; node != null; node = node.Next) {
				Transform target = node.Value;
				if (currentGrapple != null && target == currentGrapple.transform) { continue; }
				float myDist = (target.position - transform.position).magnitude;
				if (myDist < closestDist) {
					closestDist = myDist;
					if (grappleTarget != null) { grappleTarget.setHilite(false); }
					grappleTarget = target.GetComponent<wsGrapplePoint>();
					targetChanged = true;
				}
			}//	End for each grapple target
			if (targetChanged && grappleTarget != null) {
				grappleTarget.setHilite(true);
			}
		}//	End if we have one or more grapple targets
		else if (grappleTarget != null) {
			grappleTarget.setHilite(false);
			grappleTarget = null;
		}
	}//	End method seekGrappleTarget

	public void attachGrapple() {
		if (grappleTarget == null) { return; }
		if (ropePrefab == null) { Debug.LogError("Please assign a rope prefab."); return; }
		if (ropeObject == null) {
			ropeObject = GameObject.Instantiate(ropePrefab);
		}
		else {
			ropeObject.SetActive(true);
		}
		haltMovement();
		swingSpeed = 0.0f;
		grappleTarget.setHilite(false);
		currentGrapple = grappleTarget;
		updateRopePosition();
		state = MovementState.Grappling;
	}//	End method attachGrapple

	public void updateRopePosition() {
		if (ropeObject == null) { return; }
		if (ropeAttachPoint == null) { Debug.LogError("Please assign an attach point on your character for the rope object."); return; }
		if (currentGrapple == null) { return; }
		LineRenderer ropeLineRenderer = ropeObject.GetComponent<LineRenderer>();
		if (ropeLineRenderer == null) {
			Debug.LogError("Line Renderer expected. If you are creating your own rope object, you may need to edit some code here.");
			return;
		}
		ropeLineRenderer.SetVertexCount(2);//	Just to be sure
		ropeLineRenderer.SetPosition(0, ropeAttachPoint.position);
		ropeLineRenderer.SetPosition(1, currentGrapple.transform.position);
	}//	End method updateRopePosition

	public void detachGrapple() {
		if (state == MovementState.Grappling) {
			state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
		}
		swingSpeed = 0.0f;
		currentGrapple = null;
		if (ropeObject != null) { ropeObject.SetActive(false); }
	}//	End method detachGrapple

	public void setGravityToGroundPlane() {
		if (state == MovementState.Grounded) {
			if (groundData != null) { setGravityDirection(-groundData.normal); }
		}//	End if the character is grounded
	}//	End method setGravityToGroundPlane

	public void setGravityDirection(Vector3 newGravityDir) {
		if (hasAbility(Abilities.GravitySwitch) && gravityDir != newGravityDir) {
			haltMovement();
			gravityDir = newGravityDir;
			StartCoroutine(rotatePlayer(-gravityDir, turnLength));
		}//	End if the character can switch gravity
	}//	End method setGravityDirection

	private IEnumerator rotatePlayer(Vector3 newUp, float duration = 0.0f) {
		float count = 0.0f;
		Quaternion startRotation = transform.rotation;
		Quaternion endRotation = Quaternion.LookRotation(transform.forward, newUp);
		while (count < duration && duration > 0.0f) {
			transform.rotation = Quaternion.Slerp(startRotation, endRotation, count / duration);
			count += Time.deltaTime;
			yield return null;
		}
		transform.rotation = endRotation;
	}//	End coroutine method rotatePlayer

	public Vector3 getVelocity() {
		if (body2D != null) { return body2D.velocity; }
		if (body3D != null) { return body3D.velocity; }
		return Vector3.zero;
	}//	End method getVelocity

	public void setVelocity(Vector3 myVelocity) {
		if (body2D != null) { body2D.velocity = myVelocity; }
		if (body3D != null) { body3D.velocity = myVelocity; }
	}//	End method getVelocity

	public void haltMovement() {
		if (body2D != null) { body2D.velocity = Vector2.zero; }
		if (body3D != null) { body3D.velocity = Vector3.zero; }
	}//	End method haltMovement

	public void addForce(Vector3 myForce) {
		if (body2D != null) { body2D.AddForce(myForce); }
		if (body3D != null) { body3D.AddForce(myForce); }
	}//	End method addForce

	public void addForwardForce(float myForce) {
		if (body2D != null) { body2D.AddForce(transform.right * myForce); }
		if (body3D != null) { body3D.AddForce(transform.right * myForce); }
	}//	End method addForwardForce

	public void addUpwardForce(float myForce) {
		if (body2D != null) { body2D.AddForce(transform.up * myForce); }
		if (body3D != null) { body3D.AddForce(transform.up * myForce); }
	}//	End method addUpwardForce

	public void haltUpwardMovement() {
		if (body2D != null) { body2D.velocity = Vector3.Project(body2D.velocity, transform.right); }
		if (body3D != null) { body3D.velocity = Vector3.Project(body3D.velocity, transform.right); }
	}//	End method haltUpwardMovement

	public void haltForwardMovement() {
		if (body2D != null) { body2D.velocity = Vector3.Project(body2D.velocity, transform.up); }
		if (body3D != null) { body3D.velocity = Vector3.Project(body3D.velocity, transform.up); }
	}//	End method haltForwardMovement

	public IEnumerator airDash(float dashScalar, float dashTime, float dashCooldown) {
		if (hasAbility(Abilities.AirDash)) {
			if (dashScalar < 0.0f) {
				faceLeft();
			}
			else {
				faceRight();
			}
			if (state == MovementState.GrabbingLedge) { dropFromLedge(); }
			if (state == MovementState.Grappling) { detachGrapple(); }
			state = MovementState.AirDashing;
			haltMovement();
			addForwardForce(dashScalar*jumpForce);
			yield return new WaitForSeconds(dashTime);
			haltMovement();
			//	If we're still air-dashing, cooldown inputs
			if (state == MovementState.AirDashing) {
				state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
				if (numJumps == 0) { numJumps = 1; }
				inputReady = false;
				yield return new WaitForSeconds(dashCooldown);
				inputReady = true;
			}
		}//	End if the character can airdash
	}//	End method airDash

	public void bounce(float bounceForce) {
		haltUpwardMovement();
		addUpwardForce(bounceForce);
		state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
		numJumps = 1;
	}//	End method bounce

	public void detachFromWall(wsWallJumpTrigger target) {
		if (attachedWallTrigger == target) {
			attachedWallTrigger = null;
			if (state == MovementState.WallSliding) { state = MovementState.FreeFalling; numJumps = 1; }
		}
	}//	End method detachFromWall

	public void dropFromLedge() {
		if (state == MovementState.GrabbingLedge) { state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding; }
		anim.ResetTrigger("PullingUp");
		attachedPlatform = null;
		updateDirection();
	}//	End method dropFromLedge

	private void flipDirection() {
		if (facingRight) {
			faceLeft();
		}
		else {
			faceRight();
		}
	}//	End method flipDirection

	private void updateDirection() {
		if (facingRight) {
			faceRight();
		}
		else {
			faceLeft();
		}
	}//	End method updateDirection

	void faceRight() {
		if (turnLength > 0.0f) {
			StartCoroutine(turnFromTo(modelTransform.localRotation, Quaternion.Euler(0.0f, 120.0f, 0.0f)));
		}
		else {
			modelTransform.localRotation = Quaternion.Euler(0.0f, 120.0f, 0.0f);
		}
		facingRight = true;
	}//	End method faceRight

	void faceLeft() {
		if (turnLength > 0.0f) {
			StartCoroutine(turnFromTo(modelTransform.localRotation, Quaternion.Euler(0.0f, 240.0f, 0.0f)));
		}
		else {
			modelTransform.localRotation = Quaternion.Euler(0.0f, 240.0f, 0.0f);
		}
		facingRight = false;
	}//	End method faceLeft

	void faceRightFully() {
		if (turnLength > 0.0f) {
			StartCoroutine(turnFromTo(modelTransform.localRotation, Quaternion.Euler(0.0f, 90.0f, 0.0f)));
		}
		else {
			modelTransform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
		}
		facingRight = true;
	}//	End method faceRight

	void faceLeftFully() {
		if (turnLength > 0.0f) {
			StartCoroutine(turnFromTo(modelTransform.localRotation, Quaternion.Euler(0.0f, 270.0f, 0.0f)));
		}
		else {
			modelTransform.localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
		}
		facingRight = false;
	}//	End method faceLeft

	private IEnumerator turnFromTo(Quaternion startRotation, Quaternion endRotation) {
		float count = 0.0f;
		while (count < turnLength && turnLength > 0.0f) {
			modelTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, count / turnLength);
			count += Time.deltaTime;
			yield return null;
		}
		modelTransform.localRotation = endRotation;
	}//	End coroutine method turnFromTo

	public void grabLedge(Transform ledgePosition, bool leftSide, Transform ledgePullUpPosition) {
		if (!hasAbility(Abilities.LedgeGrab)) { return; }
		if (leftSide && !facingRight) {
			faceLeft();
			state = MovementState.GrabbingLedge;
			inputReady = false;
			grabbedLedge = ledgePosition;
			grabbedLedgePullup = ledgePullUpPosition;
			attachedPlatform = grabbedLedgePullup.root.GetComponentInChildren<wsMovingPlatform>();
			faceLeftFully();
			StartCoroutine(snapToPosition(ledgePosition.position, ledgeGrabLeft, 0.25f));
		}
		else if (!leftSide && facingRight) {
			faceRight();
			state = MovementState.GrabbingLedge;
			inputReady = false;
			grabbedLedge = ledgePosition;
			grabbedLedgePullup = ledgePullUpPosition;
			attachedPlatform = grabbedLedgePullup.root.GetComponentInChildren<wsMovingPlatform>();
			faceRightFully();
			StartCoroutine(snapToPosition(ledgePosition.position, ledgeGrabRight, 0.25f));
		}
	}//	End method grabLedge

	public bool hasAbility(Abilities testAbility) {
		return ((abilities & testAbility) != 0);
	}//	End method hasAbility

	public void jump(float force = 0.0f) {
		if (hasAbility(Abilities.Jump) && numJumps < maxJumps) {
			if (force == 0.0f) { force = jumpForce; }
			haltMovement();
			addUpwardForce(force);
			state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
			++numJumps;
			StartCoroutine(sleepGroundTest(0.1f));
		}
	}//	End method jump

	public IEnumerator sleepGroundTest(float duration) {
		disableGroundTest();
		yield return new WaitForSeconds(duration);
		enableGroundTest();
	}//	End method sleepGroundTest

	public void disableGroundTest() {
		groundTestDisabled = true;
	}//	End public method disableGroundTest

	public void enableGroundTest() {
		groundTestDisabled = false;
	}//	End public method enableGroundTest


	public IEnumerator knockBack(Vector3 knockBackVelocity, float duration = 0.5f) {
		state = MovementState.KnockedBack;
		addForce(knockBackVelocity);
		yield return new WaitForSeconds(duration);
		if (state == MovementState.KnockedBack) {
			state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
		}
	}//	End coroutine method knockBack

	private Vector3 getColliderSize() {
		switch (colliderType) {
			case ColliderType.box2d:
				return myCollider.size;
			case ColliderType.box3d:
				return myCollider3D.size;
			case ColliderType.capsule:
				return new Vector3(myCapsule.radius, myCapsule.height, (float)myCapsule.direction);
		}//	End switch collider type
		return Vector3.zero;
	}//	End method getColliderSize

	private Vector3 getColliderCenter() {
		switch (colliderType) {
			case ColliderType.box2d:
				return myCollider.offset;
			case ColliderType.box3d:
				return myCollider3D.center;
			case ColliderType.capsule:
				return myCapsule.center;
		}//	End switch collider type
		return Vector3.zero;
	}//	End method getColliderCenter

	private Vector3 upperRightColliderEdge() {
		return transform.position + getColliderCenter() + (transform.right*getColliderSize().x + transform.up*getColliderSize().y)*0.5f;
	}//	End method upperRightColliderEdge

	private Vector3 upperLeftColliderEdge() {
		return transform.position + getColliderCenter() - (transform.right*getColliderSize().x + transform.up*getColliderSize().y)*0.5f;
	}//	End method upperLeftColliderEdge

	private void setColliderValues(Vector3 mySize, Vector3 myCenter) {
		switch (colliderType) {
			case ColliderType.box2d:
				myCollider.size = mySize;
				myCollider.offset = myCenter;
				break;
			case ColliderType.box3d:
				myCollider3D.size = mySize;
				myCollider3D.center = myCenter;
				break;
			case ColliderType.capsule:
				myCapsule.radius = mySize.x;
				myCapsule.height = mySize.y;
				myCapsule.center = myCenter;
				break;
		}//	End switch collider type
	}//	End method setColliderValues

	private void setColliderHeight(float myHeight) {
		switch (colliderType) {
			case ColliderType.box2d:
				myCollider.size = new Vector2(myCollider.size.x, myHeight);
				break;
			case ColliderType.box3d:
				myCollider3D.size = new Vector3(myCollider3D.size.x, myHeight, myCollider3D.size.z);
				break;
			case ColliderType.capsule:
				myCapsule.height = myHeight;
				break;
		}//	End switch collider type
	}//	End method setColliderHeight

	private void centerColliderOnHeight() {
		switch (colliderType) {
			case ColliderType.box2d:
				myCollider.offset = new Vector2(myCollider.offset.x, groundCheck.localPosition.y + (myCollider.size.y * 0.5f));
				break;
			case ColliderType.box3d:
				myCollider3D.center = new Vector3(myCollider3D.center.x, groundCheck.localPosition.y + (myCollider3D.size.y * 0.5f), myCollider3D.center.z);
				break;
			case ColliderType.capsule:
				myCapsule.center = new Vector3(myCapsule.center.x, groundCheck.localPosition.y + (myCapsule.height * 0.5f), myCapsule.center.z);
				break;
		}//	End switch collider type
	}//	End method centerColliderOnHeight

	private bool checkCrouch(ButtonFlags buttons) {
		bool crouch = false;
		if (hasAbility(Abilities.Crouch)) {
			crouch = ((buttons & ButtonFlags.Crouch) != 0);
			if(!crouch && anim.GetBool("Crouch")) {
				// If the character has a ceiling preventing them from standing up, keep them crouching
				switch (colliderType) {
					case ColliderType.box2d:
						if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround)) {
							crouch = true;
						}
						break;
					case ColliderType.box3d:
					case ColliderType.capsule:
						if (Physics.OverlapSphere(ceilingCheck.position, ceilingRadius, whatIsGround).Length > 0) {
							crouch = true;
						}
						break;
				}//	End collider type switch
			}//	End if we should not be crouching
		}//	End if player can crouch

		// Set whether or not the character is crouching in the animator
		if (crouch && !anim.GetBool("Crouch")) {	//	Adjust the collider to ceiling check height
			prevColliderSize = getColliderSize();
			prevColliderCenter = getColliderCenter();
			setColliderHeight(ceilingCheck.localPosition.y - groundCheck.localPosition.y);
			centerColliderOnHeight();
		}
		else if (!crouch && anim.GetBool("Crouch")) {	//	Reset collider to previous values
			setColliderValues(prevColliderSize, prevColliderCenter);
		}
		anim.SetBool("Crouch", crouch);
		return crouch;
	}//	End method checkCrouch

	public float getSide(Vector3 myPos) {
		float dotProduct = Vector3.Dot(transform.right, myPos-transform.position);
		if (dotProduct > 0.0f) { return 1.0f; }
		else if (dotProduct < 0.0f) { return -1.0f; }
		return 0.0f;
	}//	End method getSide

	public float getDirectionSide(Vector3 myDir) {
		float dotProduct = Vector3.Dot(transform.right, myDir);
		if (dotProduct > 0.0f) { return 1.0f; }
		else if (dotProduct < 0.0f) { return -1.0f; }
		return 0.0f;
	}//	End method getSide

	private void swingOnGrapple(Vector2 move) {
		if (currentGrapple == null) { return; }
		Vector3 grapplePos = currentGrapple.transform.position;
		float grappleAngle = Vector3.Angle(gravityDir, transform.position - grapplePos);
		if (getSide(grapplePos) < 0.0f) { grappleAngle *= -1.0f; }
		swingSpeed += grappleAngle*0.15f + move.x;
		if (swingSpeed < 0.0f) { faceLeft(); }
		else { faceRight(); }
		transform.RotateAround(grapplePos, transform.forward, swingSpeed*Time.deltaTime);
		//transform.rotation = Quaternion.identity;
		transform.rotation = Quaternion.LookRotation(transform.forward, -gravityDir);
		//rotatePlayer(-gravityDir);
		if (move.y != 0.0f) {
			//	Lower or raise the character on the rope
			transform.position -= (transform.position - grapplePos).normalized * move.y * 0.1f;
		}
		updateRopePosition();
	}//	End method swingOnGrapple

	private Vector3 testSlopesAndSteps(AgnosticHit hit, ref Vector2 move) {
		Vector3 velOffset = Vector3.zero;
		//	Handle Slopes
		if (hit != null) {
			if (getDirectionSide(hit.normal) != 0.0f) {//	If we're on a slope
				if (move.x < -0.1f) {	//	If we're moving to the left
					//	Rotate 90 degrees counter-clockwise from the surface normal
					move.Set(-move.y, move.x);
				}
				else if (move.x > 0.1f) {	//	If we're moving to the right
					//	Rotate 90 degrees clockwise from the surface normal
					move.Set(move.y, -move.x);
				}
			}//	End if we are on a slope
			//	Check if we're on a moving platform
			wsGroundableObject mover = hit.transform.GetComponent<wsGroundableObject>();
			if (mover != null) {
				velOffset = mover.getVelocity();
			}//	End if we are grounded on a moving platform
		}//	End if we connected with a slope

		//	Test for steps
		float colliderHeight = getColliderSize().y;
		if (move.x > 0.0f) {//	If the player should be moving forward
			AgnosticHit hit2 = castRay(upperRightColliderEdge()+transform.right*0.1f, -transform.up, colliderHeight+0.1f);
			if (hit2 != null && hit2.distance >= colliderHeight - stepHeight) {
				//	Step up
				transform.position += transform.up*(colliderHeight-hit2.distance)+transform.right*0.1f;
			}
		}//	End if we should be moving forward
		else if (move.x < 0.0f) {
			AgnosticHit hit2 = castRay(upperLeftColliderEdge() - transform.right*0.1f, -transform.up, colliderHeight+0.1f);
			if (hit2 != null && hit2.distance < colliderHeight && hit2.distance >= colliderHeight - stepHeight) {
				//	Step up
				transform.position += transform.up*(colliderHeight-hit2.distance)-transform.right*0.1f;
			}
		}//	End if we should be moving backward
		return velOffset;
	}//	End method testSlopesAndSteps

	private Bounds getColliderBounds() {
		if (myCollider != null) { return myCollider.bounds; }
		if (myCollider3D != null) { return myCollider3D.bounds; }
		if (myCapsule != null) { return myCapsule.bounds; }
		return new Bounds();
	}//	End method getColliderBounds

	//	Raycast hit struct for physics2d or physics3d
	private class AgnosticHit {
		public Vector3 normal;
		public Transform transform;
		public float distance;
		public AgnosticHit(Vector3 _normal, Transform _transform, float _distance) {
			this.normal = _normal;
			this.transform = _transform;
			this.distance = _distance;
		}//	End Constructor
	}//	End class AgnosticHit

	private AgnosticHit castRay(Vector3 fromPos, Vector3 direction, float maxLength) {
		if (body2D != null) {	//	2D physics
			RaycastHit2D hit2d = Physics2D.Raycast(fromPos, direction, maxLength, whatIsGround);
			if (hit2d) {
				return new AgnosticHit(hit2d.normal, hit2d.transform, hit2d.distance);
			}
		}
		else if (body3D != null) {	//	3D physics
			RaycastHit hit3d;
			bool hit = Physics.Raycast(fromPos, direction, out hit3d, maxLength, whatIsGround);
			if (hit) {
				return new AgnosticHit(hit3d.normal, hit3d.transform, hit3d.distance);
			}
		}
		return null;
	}//	End method castRay

	public void move(Vector2 move, ButtonFlags buttons) {
		// If crouching, check to see if the character can stand up
		anim.SetFloat("HorizMovement", move.x);
		bool crouch = false;
		if (state == MovementState.Grounded) { crouch = checkCrouch(buttons); }

		bool jumpPressed = inputReady && ((buttons & ButtonFlags.Jump) != 0);

		if (inputReady && !jumpPressed && state != MovementState.AirDashing) {
			if ((buttons & ButtonFlags.DashLeft) != 0) {
				StartCoroutine(airDash(-0.8f, 0.25f, 0.25f));
			}
			else if ((buttons & ButtonFlags.DashRight) != 0) {
				StartCoroutine(airDash(0.8f, 0.25f, 0.25f));
			}
			else if ((buttons & ButtonFlags.Grapple) != 0) {
				if (grappleTarget != null) {
					attachGrapple();
				}
				else {
					detachGrapple();
				}
			}
		}//	End if we should check for special inputs

		switch (state) {
			case MovementState.Grappling:
				if (snappingToPosition || currentGrapple == null || (buttons & ButtonFlags.Jump) != 0) {
					detachGrapple();
					jump();
				}
				else {
					swingOnGrapple(move);
				}//	End if we don't need to detach our grapple
				break;
			case MovementState.GrabbingLedge:
				if (!snappingToPosition) {
					haltMovement();
				}
				if (inputReady) {
					if (hasAbility(Abilities.LedgeJump) && jumpPressed) {
						jump();
					}//	End if jumping from ledge
					if (facingRight) {
						if (move.y < -0.1f) {	//	Down
							dropFromLedge();
						}
						else if (move.x > 0.1f || move.y > 0.1f) {	//	Right or Up
							StartCoroutine(pullUpFromLedge(0.3f));
						}
					}//	End if facing Right
					else {	//	Facing Left
						if (move.y < -0.1f) {	//	Down
							dropFromLedge();
						}
						else if (move.x < -0.1f || move.y > 0.1f) {	//	Left or Up
							StartCoroutine(pullUpFromLedge(0.3f));
						}
					}//	End if facing Left
				}//	End if we are ready for input
				else {
					float magnitude = move.magnitude;
					if (magnitude < 0.1f) {
						inputReady = true;
					}
				}//	End if we are not ready for input
				if (!snappingToPosition && attachedPlatform != null) {
					Transform ledgeGrab = facingRight ? ledgeGrabRight : ledgeGrabLeft;
					transform.position += (grabbedLedge.position - ledgeGrab.position);//attachedPlatform.getVelocity();
				}
				break;
			case MovementState.WallSliding:
				inputReady = true;
				if (attachedWallTrigger == null) {
					if(move.x > 0.1f && !facingRight) {
						// ... flip the player.
						faceRight();
					}	// Otherwise if the input is moving the player left and the player is facing right...
					else if (move.x < -0.1f && facingRight) {
						// ... flip the player.
						faceLeft();
					}
				}
				else if (jumpPressed && hasAbility(Abilities.WallJump)) {
					wallJump();
				}
				break;
			default:
				if (snappingToPosition || state == MovementState.AirDashing) { break; }
				inputReady = true;
				if (move.x > 0.0f) { faceRight(); }
				else if (move.x < 0.0f) { faceLeft(); }
				//only control the player if grounded or airControl is turned on
				if (state == MovementState.Grounded || (state != MovementState.KnockedBack && (hasAbility(Abilities.AirControl) || hasAbility(Abilities.DoubleJump)))) {
					// Reduce the speed if crouching by the crouchSpeed multiplier
					if (crouch) {
						move.x *= crouchSpeed;
					}
					else if ((buttons & ButtonFlags.Run) != 0 && hasAbility(Abilities.Run)) {
						move.x *= runSpeedScalar;
					}
					Vector3 newVelocity = Vector3.zero;
					if (state == MovementState.Grounded) {
						newVelocity = transform.right*move.x*maxSpeed;// + Vector3.Project(getVelocity(), transform.up);
						AgnosticHit hit = castRay(transform.position+transform.up, -transform.up, 5.0f);
						Vector3 nonAnimatedVelocity = testSlopesAndSteps(hit, ref move);
						setVelocity(newVelocity + nonAnimatedVelocity);
						//	If the player should jump...
						if (jumpPressed) {
							if (move.y < -0.1f && hasAbility(Abilities.PassThroughPlatforms) && hit != null) {
								//	Drop through one-way platform
								wsOneWayPlatform platform = hit.transform.GetComponentInChildren<wsOneWayPlatform>();
								if (platform != null) {
									platform.allowPassthrough(gameObject);
									state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
								}
								else {
									jump();
								}
							}//	End if we are holding down while jumping
							else {
								jump();
							}
						}//	End if the jump button was pressed
					}//	End if the character is on the ground
					else {
						if (hasAbility(Abilities.AirControl)) {
							Vector3 myVelocity = getVelocity();
							if (hasAbility(Abilities.JumpHeightControl) && (buttons & ButtonFlags.ReleaseJump) != 0) {
								//	Remove jump velocity
								newVelocity = transform.right*move.x*maxSpeed + Vector3.Project(myVelocity, transform.up)*0.25f;
							}
							else {
								newVelocity = Vector3.Project(myVelocity, transform.right)*0.75f + transform.right*move.x*maxSpeed*0.25f + Vector3.Project(myVelocity, transform.up);
							}
							setVelocity(newVelocity);
						}
						if (hasAbility(Abilities.DoubleJump) && jumpPressed && numJumps < maxJumps) {
							jump(doubleJumpForce);
						}
					}//	End if the character is in the air
				}//	End if we need to move the player
				break;
		}//	End switch state
	}//	End method move

	public bool isPullingFromLedge() { return (state == MovementState.PullingFromLedge); }

	public IEnumerator pullUpFromLedge(float animDuration) {
		if (!snappingToPosition) {
			snappingToPosition = true;
			state = MovementState.PullingFromLedge;
			Vector3 snapVector = grabbedLedgePullup.position - transform.position;
			haltMovement();
			if (animDuration <= 0.0f) {
				transform.position += snapVector;
			}
			else {
				float totalDuration = 0.0f;
				float snapDuration = animDuration * 0.5f;
				Vector3 beginningPos = transform.position;
				anim.SetTrigger("PullingUp");
				float vertDistance = Vector3.Project(snapVector, transform.up).magnitude;
				float horizDistance = Vector3.Project(snapVector, transform.right).magnitude;
				horizDistance *= getDirectionSide(snapVector);
				while (totalDuration < snapDuration) {
					haltMovement();
					transform.position = beginningPos + transform.up*Mathf.Lerp(0.0f, vertDistance, totalDuration / snapDuration);
					totalDuration += Time.deltaTime;
					yield return null;
				}
				beginningPos = transform.position;
				totalDuration = 0.0f;
				while (totalDuration < snapDuration) {
					haltMovement();
					transform.position = beginningPos + transform.right*Mathf.Lerp(0.0f, horizDistance, totalDuration / snapDuration);
					totalDuration += Time.deltaTime;
					yield return null;
				}
				transform.position = grabbedLedgePullup.position;
			}//	End if we have a positive snap duration
			snappingToPosition = false;
			dropFromLedge();
			state = MovementState.FreeFalling;
		}//	End if we are not snapping to a position
	}//	End method pullUpFromLedge

	public void respawn() {
		transform.position = respawnPos;
	}//	End method respawn

	private void setGrounded(bool newValue) {
		if (newValue) { state = MovementState.Grounded; }
		else {
			state = (attachedWallTrigger == null) ? MovementState.FreeFalling : MovementState.WallSliding;
			if (currentGround != null) {
				currentGround.wsOnGroundExit();
				currentGround = null;
			}
		}
	}//	End method setGrounded

	public void setRespawnPosition(Vector3 newPosition) {
		respawnPos = newPosition;
	}//	End method setRespawnPosition

	private IEnumerator snapToPosition(Vector3 targetPoint, Transform snapPoint, float snapDuration = 0.5f) {
		snappingToPosition = true;
		Vector3 snapVector = targetPoint - snapPoint.position;
		haltMovement();
		anim.SetFloat("HorizMovement", 0.0f);
		if (snapDuration <= 0.0f) {
			transform.position += snapVector;
		}
		else {
			float totalDuration = 0.0f;
			Vector3 beginningPos = transform.position;
			Vector3 endingPos = transform.position + snapVector;
			while (totalDuration < snapDuration) {
				transform.position = Vector3.Lerp(beginningPos, endingPos, totalDuration / snapDuration);
				totalDuration += Time.deltaTime;
				yield return null;
			}
			transform.position += targetPoint - snapPoint.position;
		}
		snappingToPosition = false;
	}//	End coroutine method snapToPosition

	public void wallJump() {
		Vector3 jumpVector = transform.up*verticalWallJumpForce + transform.right*wallJumpForce*attachedWallTrigger.getDirection();
		state = MovementState.FreeFalling;
		numJumps = 1;
		if (attachedWallTrigger.getDirection() < 0.0f) { faceLeft(); }
		else { faceRight(); }
		StartCoroutine(knockBack(jumpVector, 0.25f));
	}//	End method wallJump

	public void wallSlide(wsWallJumpTrigger wallTrigger) {
		if (state != MovementState.GrabbingLedge && state != MovementState.PullingFromLedge && !snappingToPosition) {
			attachedWallTrigger = wallTrigger;
			state = MovementState.WallSliding;
			if (attachedWallTrigger.getDirection() < 0.0f) { faceRightFully(); }
			else { faceLeftFully(); }
		}
	}//	End method wallSlide

	
	// DPS methods/members

	private	DpsInterpreter	dpsInterpreter;
	public string			characterState;
	private float			lastJumpTime = -1f;
	private float			jumpTimeCutoff = 1.0f;


	private void setUpDPS()
	{
		this.dpsInterpreter = FindObjectOfType<DpsInterpreter>();
	}

	public void setMusicState(ButtonFlags buttons)
	{
		if ((buttons & ButtonFlags.Jump) != 0) {
			lastJumpTime = Time.time;
		}

		Rigidbody rb = GetComponent<Rigidbody>();
		float xSpeed = Math.Abs(rb.velocity.x);
		float yPos	 = rb.position.y;

		bool isMovingFast		= (((buttons & ButtonFlags.Run) != 0) && (xSpeed > maxSpeed));
		bool isJumping			= (Time.time - lastJumpTime < jumpTimeCutoff);  //jumpRate > jumpThresh;
		bool isWaiting			= (xSpeed == 0);
		bool isFallingToDeath	= (yPos < -70f);

		if (isFallingToDeath) {
			this.characterState = "Falling To Death";
			float sVal = (yPos + 70f) / -1930f;
			this.dpsInterpreter.setSliderValue(sVal);
		}
		else if (isJumping)
		{
			this.characterState = "Jumping";
			//sliderValue = jumpRate;
		}
		else if (isMovingFast) {
			this.characterState = "Moving Fast";
			//sliderValue = (xSpeed - maxSpeed) / (2 * maxSpeed);
		}
		else if (!isWaiting) {
			this.characterState = "Moving";
			//sliderValue = xSpeed / maxSpeed;
		}
		else {
			this.characterState = "Waiting";
		}
		this.dpsInterpreter.setState(this.characterState);
	}



}//	End class wsPlatformCharacter
