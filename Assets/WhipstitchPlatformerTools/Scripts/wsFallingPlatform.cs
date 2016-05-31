using UnityEngine;
using System.Collections;

public class wsFallingPlatform : wsGroundableObject {

	public float fallDelay = 1.0f;
	public float gravityForce = 1.50f;
	public float killTime = 10.0f;	//	Destroy this gameObject this many seconds after falling
	public float respawnTime = -1.0f;	//	Respawn this gameObject this many seconds after killing it. Negative values mean no respawn.
	public bool tintObject = true;
	public Color tintColor;

	private Color startColor;
	private MeshRenderer meshRenderer;
	private Material myMaterial;
	private float timeStep = 0.05f;	//	Percentage of fallDelay to update color tint
	private bool falling = false;
	private bool fallCycleStarted = false;
	private float killTimer = 0.0f;
	private Vector3 startPos;
	private Rigidbody2D body2D;
	private Rigidbody body3D;

	// Use this for initialization
	void Start() {
		startPos = transform.position;
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer != null) {
			myMaterial = meshRenderer.GetComponent<Renderer>().material;
		}
		startColor = myMaterial.color;
		body2D = GetComponent<Rigidbody2D>();
		if (body2D == null) {
			body3D = GetComponent<Rigidbody>();
		}
	}//	End Unity method Start

	void FixedUpdate() {
		if (falling) {
			if (body2D != null) {
				Vector2 newVelocity = body2D.velocity;
				newVelocity.y -= gravityForce;
				body2D.velocity = newVelocity;
			}
			else if (body3D != null) {
				Vector3 newPosition = transform.position;
				newPosition.y -= gravityForce;
				// body3D.velocity = newVelocity3D;
				body3D.MovePosition(newPosition);
			}
			killTimer += Time.deltaTime;
			if (killTime > 0.0f && killTimer >= killTime) {
				if (respawnTime < 0.0f) {
					GameObject.Destroy(gameObject);
				}
				else if (respawnTime == 0.0f) {
					falling = false;
					fallCycleStarted = false;
					if (body2D != null) { body2D.velocity = Vector2.zero; }
					else if (body3D != null) { body3D.velocity = Vector3.zero; }
					myMaterial.color = startColor;
					transform.position = startPos;
					killTimer = 0.0f;
				}
				else {
					StartCoroutine(killAndRespawn());
				}
			}
		}//	End if we are falling
	}//	End Unity Method FixedUpdate

	public IEnumerator killAndRespawn() {
		Collider2D collider = GetComponent<Collider2D>();
		Collider collider3D = GetComponent<Collider>();
		if (collider != null) {
			collider.enabled = meshRenderer.enabled = false;
		}
		else {
			if (collider3D != null) {
				collider3D.enabled = meshRenderer.enabled = false;
			}
		}//	End if we don't have a 2d collider
		yield return new WaitForSeconds(respawnTime);
		falling = false;
		fallCycleStarted = false;
		if (body2D != null) { body2D.velocity = Vector2.zero; }
		else if (body3D != null) { body3D.velocity = Vector3.zero; }
		myMaterial.color = startColor;
		transform.position = startPos;
		killTimer = 0.0f;
		if (collider != null) { collider.enabled = meshRenderer.enabled = true; }
		else if (collider3D != null) { collider3D.enabled = meshRenderer.enabled = true; }
	}//	End method killAndRespawn

	public override Vector3 getVelocity() {
		if (body2D != null) { return body2D.velocity; }
		return body3D.velocity;
	}

	public override void wsOnGroundEnter() {
		if (!fallCycleStarted) { StartCoroutine(fallCycle()); }
	}//	End wsGroundableObject method wsOnGroundEnter
	public override void wsOnGroundExit() {	}

	public IEnumerator fallCycle() {
		if (fallCycleStarted) { yield return null; }
		fallCycleStarted = true;
		if (fallDelay > 0.0f) {
			if (tintObject) {
				float interval = timeStep / fallDelay;
				float timeElapsed = 0.0f;
				while (timeElapsed < fallDelay) {
					myMaterial.color = Color.Lerp(startColor, tintColor, (timeElapsed / fallDelay));
					timeElapsed += interval;
					yield return new WaitForSeconds(interval);
				}
				falling = true;
			}
			else {
				yield return new WaitForSeconds(fallDelay);
				falling = true;
			}
		}
		else {	//	If there is no fall delay
			falling = true;
		}
	}//	End coroutine method fallCycle

}//	End class wsFallingPlatform
