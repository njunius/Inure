using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Transform positionTarget; // this is most likely the player with few exceptions
    private Transform lookAtTarget;
    Vector3 cameraPositionOffset;

    // Use this for initialization
    void Start () {
        //Cursor.visible = false; // comment and uncomment this based on current needs

        positionTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        lookAtTarget = GameObject.FindGameObjectWithTag("Camera Target").GetComponent<Transform>();

        // y offset = -0.7 for new camera and -1.0 for newer camera from prototype
        cameraPositionOffset = new Vector3(0.0f, -1.0f, 3.0f); // x offset should not be edited as that will uncenter the camera
        transform.position = positionTarget.position - cameraPositionOffset;

        transform.LookAt(lookAtTarget.transform);
    }
	
	// Update is called once per frame
	void Update () {
        // camera looks at the player properly
        transform.rotation = positionTarget.rotation;
        // camera follows the player (this converts the target's position from world to local space, offsets the camera
        // then converts back to world space for the camera's new position
        transform.position = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);
    }
}
