using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject positionTarget; // this is most likely the player with few exceptions
    public GameObject lookAtTarget;
    Vector3 cameraPositionOffset;

    // Use this for initialization
    void Start () {
        //Cursor.visible = false; // comment and uncomment this based on current needs
        
        // y offset = -0.7 for new camera and -1.0 for newer camera from prototype
        cameraPositionOffset = new Vector3(0.0f, -1.0f, 3.0f); // x offset should not be edited as that will uncenter the camera
        transform.position = positionTarget.transform.position - cameraPositionOffset;

        transform.LookAt(lookAtTarget.transform);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
