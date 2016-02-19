using UnityEngine;
using System.Collections;

public enum CameraMode { ThirdPerson, FirstPerson, ThirdPersonLoose }

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
    public float defaultHeight = -0.0f;
    public float currentHeight;
    public float targetHeight;
    public float defaultSide = 0.0f;
    public float currentSide;
    public float targetSide;
    public float clipNearDefault = 0.2f;

    public Vector3 firstPersonPosition;
    public Vector3 thirdPersonPosition;
    private float fadeDistance;

    private Vector3 targetPrevPos;
    private Transform targetPrevTransform;
    private bool locked;
    //public float clipNearCurrent = 

    private Vector3 velocity = Vector3.zero;
    public GameObject gameController;
    public InputManager im;
    // Use this for initialization
    void Start () {

        fadeDistance = defaultDistance * 0.25f;

        mode = CameraMode.ThirdPerson;

        firstPersonPosition = new Vector3(0, -0.35f, 0);
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
    void FixedUpdate() {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController");
            im = gameController.GetComponent<InputManager>();
        }
        else
        {
            if (im.getInputDown("Camera Mode"))
            {
                switch (mode)
                {
                    case CameraMode.ThirdPerson:
                        Debug.Log("Camera Mode: First Person");
                        mode = CameraMode.FirstPerson;

                        Renderer rend = target.GetComponent<Renderer>();
                        rend.enabled = false;
                        GameObject.FindGameObjectWithTag("Warning Radius").GetComponent<RadarTrigger>().enabled = false;
                        cameraPositionOffset = firstPersonPosition;
                        gameObject.GetComponent<Camera>().fieldOfView = 90;
                        //updateCamera();
                        break;
                    case CameraMode.FirstPerson:
                        Debug.Log("Camera Mode: Third Person Loose");
                        
                        rend = target.GetComponent<Renderer>();
                        rend.enabled = true;
                        GameObject.FindGameObjectWithTag("Warning Radius").GetComponent<RadarTrigger>().enabled = true;
                        cameraPositionOffset = thirdPersonPosition;
                        gameObject.GetComponent<Camera>().fieldOfView = 60;
                        mode = CameraMode.ThirdPerson;
                        updateCamera();
                        mode = CameraMode.ThirdPersonLoose;
                        break;

                    case CameraMode.ThirdPersonLoose:
                        Debug.Log("Camera Mode: Third Person Rigid");
                        mode = CameraMode.ThirdPerson;
                        break;

                }
            }


            /*if (!positionTarget.velocity.Equals(Vector3.zero) || !positionTarget.angularVelocity.Equals(Vector3.zero))
            {
                updateCamera();
            }*/
            updateCamera();

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
            */



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



            //transform.LookAt(lookAtTarget.transform);

        }
        
    }

    private void updateCamera()
    {
        if (mode == CameraMode.ThirdPerson)
        {
            Vector3 hitNormal;
            bool hitFound = false;
            Vector3 hitPos = new Vector3();
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.forward * 0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.forward * -0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.right * 0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.right * -0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.up * 0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.up * -0.5f));

            //check if camera is near wall

            //Debug.DrawRay(transform.position, Quaternion.AngleAxis(Camera.current.fieldOfView / 2, transform.up) * transform.forward, Color.blue);
            //Debug.DrawRay(transform.position, Quaternion.AngleAxis(-Camera.current.fieldOfView / 2, transform.up) * transform.forward, Color.blue);


            /*RaycastHit[] rays = Physics.RaycastAll(transform.position, Quaternion.AngleAxis(Camera.current.fieldOfView / 2, transform.up) * transform.forward, Camera.current.nearClipPlane);
            foreach(RaycastHit ray in rays)
            {
                Debug.Log(ray.transform.tag);
                if (ray.transform.tag.Equals("Environment"))
                {
                    Debug.Log("EnvSphr");
                    //Debug.Log("hit");
                    hitFound = true;
                    hitNormal = ray.normal;
                    hitPos = ray.point;
                    break;

                }
            }*/

            if (hitFound)
            {
                RaycastHit hit;
                Debug.Log("found");
                Debug.DrawRay(transform.position,hitPos, Color.red);
                if (Physics.Raycast(transform.position, hitPos, out hit, 0.02f, 1 << 11))
                {
                    Debug.Log("line");
                    Debug.Log(hit.distance);
                    targetDistance -= hit.distance;
                    float ratio = targetDistance / defaultDistance;
                    targetHeight = ratio * defaultHeight;
                    ratio -= ratio * 0.1f;
                }
                cameraPositionOffset.y = targetHeight;
                cameraPositionOffset.z = targetDistance;
            }
            else
            {
                Vector3 dirToCamera = transform.position - positionTarget.transform.position;
                RaycastHit[] hits = Physics.RaycastAll(positionTarget.position, dirToCamera,
                                    Mathf.Sqrt(defaultDistance * defaultDistance + defaultHeight * defaultHeight));
                foreach (RaycastHit ray in hits)
                {
                    if (ray.transform.CompareTag("Environment"))
                    {
                        targetDistance = ray.distance - 0.25f;
                        float ratio = targetDistance / defaultDistance;
                        targetHeight = ratio * defaultHeight;
                        cameraPositionOffset.y = targetHeight;
                        cameraPositionOffset.z = targetDistance;
                        hitFound = true;
                        break;
                    }

                }

                Vector3 topOfScreen = transform.TransformDirection(new Vector3(0, defaultHeight, 0));
                Vector3 localTargetPos = transform.InverseTransformPoint(positionTarget.position);
                Vector3 rayBase = transform.TransformPoint(new Vector3(localTargetPos.x, localTargetPos.y, localTargetPos.z));
                Debug.DrawRay(rayBase, transform.position - rayBase, Color.blue);
                hits = Physics.RaycastAll(rayBase, transform.position - rayBase, defaultHeight + 0.2f);
                foreach (RaycastHit ray in hits)
                {
                    if (ray.transform.CompareTag("Environment"))
                    {
                        targetHeight = ray.distance - 0.25f;
                        cameraPositionOffset.y = targetHeight;
                        hitFound = true;
                        break;
                    }

                }

                if (!hitFound)
                {

                    //Debug.Log("No hit");
                    cameraPositionOffset = thirdPersonPosition;
                }
            }
            transform.rotation = positionTarget.rotation;
            transform.Rotate(cameraRotationOffset);

            Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);

            if (targetDistance < defaultDistance)
            {
                transform.position = nextPosition; //Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 15f);
            }
            else
            {
                transform.position = nextPosition;
            }


            float distance = Vector3.Distance(transform.position, positionTarget.position);
            if (distance < fadeDistance)
            {
                Renderer r = target.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

            }
        }
        else if (mode == CameraMode.ThirdPersonLoose)
        {
            Vector3 hitNormal;
            bool hitFound = false;
            Vector3 hitPos = new Vector3();
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.forward * 0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.forward * -0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.right * 0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.right * -0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.up * 0.5f));
            Debug.DrawLine(transform.position, transform.TransformPoint(transform.up * -0.5f));

            //check if camera is near wall

            //Debug.DrawRay(transform.position, Quaternion.AngleAxis(Camera.current.fieldOfView / 2, transform.up) * transform.forward, Color.blue);
            //Debug.DrawRay(transform.position, Quaternion.AngleAxis(-Camera.current.fieldOfView / 2, transform.up) * transform.forward, Color.blue);


            /*RaycastHit[] rays = Physics.RaycastAll(transform.position, Quaternion.AngleAxis(Camera.current.fieldOfView / 2, transform.up) * transform.forward, Camera.current.nearClipPlane);
            foreach(RaycastHit ray in rays)
            {
                Debug.Log(ray.transform.tag);
                if (ray.transform.tag.Equals("Environment"))
                {
                    Debug.Log("EnvSphr");
                    //Debug.Log("hit");
                    hitFound = true;
                    hitNormal = ray.normal;
                    hitPos = ray.point;
                    break;

                }
            }*/

            if (hitFound)
            {
                RaycastHit hit;
                Debug.Log("found");
                Debug.DrawRay(transform.position, hitPos, Color.red);
                if (Physics.Raycast(transform.position, hitPos, out hit, 0.02f, 1 << 11))
                {
                    Debug.Log("line");
                    Debug.Log(hit.distance);
                    targetDistance -= hit.distance;
                    float ratio = targetDistance / defaultDistance;
                    targetHeight = ratio * defaultHeight;
                    ratio -= ratio * 0.1f;
                }
                cameraPositionOffset.y = targetHeight;
                cameraPositionOffset.z = targetDistance;
            }
            else
            {
                Vector3 dirToCamera = transform.position - positionTarget.transform.position;
                RaycastHit[] hits = Physics.RaycastAll(positionTarget.position, dirToCamera,
                                    Mathf.Sqrt(defaultDistance * defaultDistance + defaultHeight * defaultHeight));
                foreach (RaycastHit ray in hits)
                {
                    if (ray.transform.CompareTag("Environment"))
                    {
                        targetDistance = ray.distance - 0.25f;
                        float ratio = targetDistance / defaultDistance;
                        targetHeight = ratio * defaultHeight;
                        cameraPositionOffset.y = targetHeight;
                        cameraPositionOffset.z = targetDistance;
                        hitFound = true;
                        break;
                    }

                }

                Vector3 topOfScreen = transform.TransformDirection(new Vector3(0, defaultHeight, 0));
                Vector3 localTargetPos = transform.InverseTransformPoint(positionTarget.position);
                Vector3 rayBase = transform.TransformPoint(new Vector3(localTargetPos.x, localTargetPos.y, localTargetPos.z));
                Debug.DrawRay(rayBase, transform.position - rayBase, Color.blue);
                hits = Physics.RaycastAll(rayBase, transform.position - rayBase, defaultHeight + 0.2f);
                foreach (RaycastHit ray in hits)
                {
                    if (ray.transform.CompareTag("Environment"))
                    {
                        targetHeight = ray.distance - 0.25f;
                        cameraPositionOffset.y = targetHeight;
                        hitFound = true;
                        break;
                    }

                }

                if (!hitFound)
                {

                    //Debug.Log("No hit");
                    cameraPositionOffset = thirdPersonPosition;
                }
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, positionTarget.rotation, Time.deltaTime);
            transform.Rotate(cameraRotationOffset);

            Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);
            
            if (targetDistance < defaultDistance)
            {
                transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, 0.05f);
            }
            else
            {
                transform.position = nextPosition;
            }


            float distance = Vector3.Distance(transform.position, positionTarget.position);
            if (distance < fadeDistance)
            {
                Renderer r = target.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

            }
        }
        else
        {
            cameraPositionOffset = firstPersonPosition;

            transform.rotation = positionTarget.rotation;
            transform.Rotate(cameraRotationOffset);

            Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);

            if (targetDistance < defaultDistance)
            {
                transform.position = nextPosition; //Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 15f);
            }
            else
            {
                transform.position = nextPosition;
            }


            float distance = Vector3.Distance(transform.position, positionTarget.position);
            if (distance < fadeDistance)
            {
                Renderer r = target.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

            }
        }



        // camera looks at the player properly

        /*Quaternion nextRot = Quaternion.LookRotation(positionTarget.position - transform.position, -positionTarget.transform.forward);

        if (Quaternion.Angle(transform.rotation, nextRot) < 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, Time.deltaTime * 10.0f);
        }
        else
        {
            transform.rotation = nextRot;
        }*/
        
        

        //transform.LookAt(lookAtTarget);

        // camera follows the player (this converts the target's position from world to local space, offsets the camera
        // then converts back to world space for the camera's new position


        /*Vector3 destination = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);
        
        if (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset), 20 * Time.deltaTime);
            transform.position = nextPosition;
        }
        else
        {
            transform.position = destination;
        }*/


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

