using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Rigidbody positionTarget; // this is most likely the player with few exceptions
    private Transform lookAtTarget;
    public Vector3 cameraPositionOffset;
    public Vector3 cameraRotationOffset;
    public float defaultDistance = 4.0f;
    private float currentDistance;
    private float targetDistance;
    public float defaultHeight = -1.0f;
    private float currentHeight;
    private float targetHeight;

    // Use this for initialization
    void Start () {
        //Cursor.visible = false; // comment and uncomment this based on current needs
        targetDistance = currentDistance = defaultDistance;
        targetHeight = currentHeight = defaultHeight;
        positionTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        lookAtTarget = GameObject.FindGameObjectWithTag("Camera Target").GetComponent<Transform>();

        // y offset = -0.7 for new camera and -1.0 for newer camera from prototype
        cameraPositionOffset = new Vector3(0.0f, defaultHeight, defaultDistance); // x offset should not be edited as that will uncenter the camera
        cameraRotationOffset = new Vector3(-3.0f, 0, 0);
        transform.position = positionTarget.position - cameraPositionOffset;

        //transform.LookAt(lookAtTarget.transform);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        currentDistance = cameraPositionOffset.z;
        currentHeight = cameraPositionOffset.y;
        RaycastHit hit;
        if (Physics.Raycast(positionTarget.position, -1 * positionTarget.transform.position + transform.position, out hit, defaultDistance))
        {
            if (hit.distance > 1.1f)
            {
                targetDistance = hit.distance;
                float ratio = currentDistance / defaultDistance;
                Debug.Log(ratio);
                targetHeight = ratio * defaultHeight;
            }
        }
        else
        {
            targetDistance = defaultDistance;
            targetHeight = defaultHeight;
        }
        cameraPositionOffset.z = Mathf.Lerp(targetDistance, currentDistance, Time.deltaTime * 1);
        cameraPositionOffset.y = Mathf.Lerp(targetHeight, currentHeight, Time.deltaTime * 1);


        // camera looks at the player properly

        transform.rotation = positionTarget.rotation;
        transform.Rotate(cameraRotationOffset);

        // camera follows the player (this converts the target's position from world to local space, offsets the camera
        // then converts back to world space for the camera's new position
        Vector3 wantedPosition = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);
        transform.position = wantedPosition;

        //transform.LookAt(lookAtTarget.transform);
    }
}
