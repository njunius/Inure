using UnityEngine;
using System.Collections;

public enum CameraMode { ThirdPerson, FirstPerson }

public class CameraController : MonoBehaviour {

    CameraMode mode;
    private GameObject target;
    private Rigidbody positionTarget; // this is most likely the player with few exceptions
    private Transform lookAtTarget;
    public Vector3 cameraPositionOffset;
    public Vector3 cameraRotationOffset;
    public float defaultDistance = 4.0f;
    public float currentDistance;
    public float targetDistance;
    public float defaultHeight = -1.0f;
    public float currentHeight;
    public float targetHeight;
    public float defaultSide = 0.0f;
    public float currentSide;
    public float targetSide;
    public float clipNearDefault = 0.2f;

    public Vector3 firstPersonPosition;
    public Vector3 thirdPersonPosition;

    private Vector3 targetPrevPos;
    private bool locked;
    //public float clipNearCurrent = 

    public GameObject gameController;
    public InputManager im;
    // Use this for initialization
    void Start () {
        

        
        mode = CameraMode.ThirdPerson;

        firstPersonPosition = new Vector3(0, 0, 0);
        thirdPersonPosition = new Vector3(defaultSide, defaultHeight, defaultDistance);

        //Cursor.visible = false; // comment and uncomment this based on current needs
        targetDistance = currentDistance = defaultDistance;
        targetHeight = currentHeight = defaultHeight;
        targetSide = currentSide = defaultSide;
        target = GameObject.FindGameObjectWithTag("Player");
        positionTarget = target.GetComponent<Rigidbody>();
        lookAtTarget = GameObject.FindGameObjectWithTag("Camera Target").GetComponent<Transform>();
        

        // y offset = -0.7 for new camera and -1.0 for newer camera from prototype
        cameraPositionOffset = new Vector3(0.0f, defaultHeight, defaultDistance); // x offset should not be edited as that will uncenter the camera
        cameraRotationOffset = new Vector3(-3.0f, 0, 0);
        transform.position = positionTarget.position - cameraPositionOffset;



        //transform.LookAt(lookAtTarget.transform);
    }

    // Update is called once per frame
    void LateUpdate() {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController");
            im = gameController.GetComponent<InputManager>();
        }
        else
        {
            if (im.getInputDown("Camera Mode"))
            {
                if (mode == CameraMode.ThirdPerson)
                {
                    mode = CameraMode.FirstPerson;
                    
                    Renderer rend = target.GetComponent<Renderer>();
                    rend.enabled = false;
					GameObject.FindGameObjectWithTag ("Warning Radius").GetComponent<RadarTrigger> ().enabled = false;

                }
                else
                {
                    mode = CameraMode.ThirdPerson;
                    Renderer rend = target.GetComponent<Renderer>();
                    rend.enabled = true;
					GameObject.FindGameObjectWithTag ("Warning Radius").GetComponent<RadarTrigger> ().enabled = true;
                }
            }

            if (mode == CameraMode.ThirdPerson)
            {
                cameraPositionOffset = thirdPersonPosition;
            }
            else
            {
                cameraPositionOffset = firstPersonPosition;
            }
            /*
            currentDistance = cameraPositionOffset.z;
            currentHeight = cameraPositionOffset.y;

            //int layer = LayerMask.NameToLayer("Default");

            RaycastHit[] allHits;
            allHits = Physics.RaycastAll(positionTarget.position, -1 * positionTarget.transform.position + transform.position, defaultDistance);

            bool hitEnvironment = false;
            RaycastHit hit = new RaycastHit();
            foreach (RaycastHit ray in allHits)
            {

                if (ray.transform.tag.Equals("Environment"))
                {

                    hitEnvironment = true;
                    hit = ray;
                    break;
                }

            }


            Debug.DrawLine(transform.position, Vector3.left);

            Vector3 hitNormal = new Vector3();
            bool hitFound = false;
            Vector3 hitPos = new Vector3();
            allHits = Physics.SphereCastAll(transform.position, 0.5f, -transform.forward, 0.1f);
            foreach (RaycastHit ray in allHits)
            {
                if (ray.transform.tag.Equals("Environment"))
                {
                    hitFound = true;
                    hitNormal = ray.normal;
                    hitPos = ray.point;
                    break;

                } 
            }

            if (hitFound && !locked)
            {
                Debug.Log("offset " + cameraPositionOffset);
                Debug.Log("normal " + hitNormal);
                cameraPositionOffset += hitNormal * 0.1f;
                locked = true;
            }
            else if (!locked)
            {
                cameraPositionOffset = new Vector3(0.0f, defaultHeight, defaultDistance);
            }
            else
            {
                if (positionTarget.position != targetPrevPos)
                {
                    locked = false;
                }
            }


            /*
            if (Physics.Raycast(transform.position, Vector3.right, out hit, 0.5f))
            {
                Debug.Log(hit.transform.tag);
                if (hit.transform.tag.Equals("Environment"))
                {
                    targetSide = -hit.distance + 0.5f;
                }
            }
            else
            {
                targetSide = defaultSide;
            }
            */
            /*if (hitEnvironment)
            {

                targetDistance = hit.distance;
                float ratio = currentDistance / defaultDistance;
                targetHeight = ratio * defaultHeight;
                if (Mathf.Abs(hit.distance) < 1.1f)
                {
                    Color color = target.GetComponent<MeshRenderer>().material.color;
                    color.a = Vector3.Distance(transform.position, positionTarget.position) / 1.1f;
                    target.GetComponent<MeshRenderer>().material.color = color;
                }
            }
            else
            {
                targetDistance = defaultDistance;
                targetHeight = defaultHeight;

            }*/

            /*Camera.current.fieldOfView = Mathf.Lerp(Camera.current.fieldOfView, targetFOV, Time.deltaTime * 10);*/
            //cameraPositionOffset.z = targetDistance; // Mathf.Lerp(targetDistance, currentDistance, Time.deltaTime * 0.1f);
            //cameraPositionOffset.y = targetHeight; //Mathf.Lerp(targetHeight, currentHeight, Time.deltaTime * 0.1f);
            //cameraPositionOffset.x = targetSide;

            // camera looks at the player properly

            transform.rotation = positionTarget.rotation;
            transform.Rotate(cameraRotationOffset);

            // camera follows the player (this converts the target's position from world to local space, offsets the camera
            // then converts back to world space for the camera's new position
            Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);

            if (targetDistance < defaultDistance)
            {
                transform.position = nextPosition; // Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 15f);
            }
            else
            {
                transform.position = nextPosition;
            }


            //transform.LookAt(lookAtTarget.transform);

            targetPrevPos = positionTarget.position;
        }
        
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Hi");
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag.Equals("Environment"))
        {
            Debug.Log("Hit Evironment");
            
            /*foreach (ContactPoint contact in col.contacts)
            {
                if (contact.)
            }*/
            Vector3 normal = col.contacts[0].normal;

            transform.position += normal * 0.5f;
            /*if (Vector3.Distance(positionTarget.position, transform.position) > 1.1f)
            {
                targetDistance -= 0.5f;
                float ratio = currentDistance / defaultDistance;

                targetHeight = ratio * defaultHeight;


            }*/
        
            
        }
    }

    void OnCollisionExit(Collision col)
    {
        Debug.Log("ho");
    }

    void OnCollisionStay(Collision col)
    {
        Debug.Log("hey");
    }

    public CameraMode getMode(){
        return mode;
    }
}

