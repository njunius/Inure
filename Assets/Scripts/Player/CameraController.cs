using UnityEngine;
using System.Collections;

public enum CameraMode { ThirdPerson, FirstPerson }

public class CameraController : MonoBehaviour {

    CameraMode mode;
    private GameObject target;
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
    public float minDistPercent = 0.8f;
    public float minHeightPercent = 0.25f;
    public float maxDistPercent = 0.2f;
    public float maxHeightPercent = 0.25f;


    private float currentSide;
    private float targetSide;
    

    public float positionDamping = 2.0f;
    public float rotationDamping = 2.0f;
    public float maxSpeed = 1.0f;
    private Transform thisTransformCache;


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
        thisTransformCache = transform;
        fadeDistance = defaultDistance * 0.5f;

        mode = CameraMode.ThirdPerson;

        firstPersonPosition = new Vector3(0, 0, 0);
        thirdPersonPosition = new Vector3(defaultSide, defaultHeight, defaultDistance);

        //Cursor.visible = false; // comment and uncomment this based on current needs
        targetDistance = currentDistance = defaultDistance;
        targetHeight = currentHeight = defaultHeight;
        targetSide = currentSide = defaultSide;
        target = GameObject.FindGameObjectWithTag("Player");
        //positionTarget = target.GetComponent<Rigidbody>();
        lookAtTarget = GameObject.FindGameObjectWithTag("Camera Target").GetComponent<Transform>();
        

        // y offset = -0.7 for new camera and -1.0 for newer camera from prototype
        cameraPositionOffset = new Vector3(0.0f, defaultHeight, defaultDistance); // x offset should not be edited as that will uncenter the camera
        cameraRotationOffset = new Vector3(-3.0f, 0, 0);
        //transform.position = positionTarget.position - cameraPositionOffset;



        //transform.LookAt(lookAtTarget.transform);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController");
            im = gameController.GetComponent<InputManager>();
            return;
        }

        if (!target)
        {
            return;
        }
        


        updateCamera();

        
    }

    void Update()
    {
        if (im.getInputDown("Camera Mode"))
        {
            switch (mode)
            {
                case CameraMode.ThirdPerson:
                    Debug.Log("Camera Mode: First Person");
                    mode = CameraMode.FirstPerson;

                    //Renderer rend = target.transform.Find("Sol Starfighter Advanced Model").GetComponent<Renderer>();
                    //rend.enabled = false;
                    GameObject.FindGameObjectWithTag("Radar Trigger").GetComponent<RadarTrigger>().enabled = false;
                    cameraPositionOffset = firstPersonPosition;
                    gameObject.GetComponent<Camera>().fieldOfView = 90;
                    //updateCamera();
                    break;
                case CameraMode.FirstPerson:
                    mode = CameraMode.ThirdPerson;
                    Debug.Log("Camera Mode: Third Person");
                    //rend = target.transform.Find("Sol Starfighter Advanced Model").GetComponent<Renderer>();
                    //rend.enabled = true;
                    GameObject.FindGameObjectWithTag("Radar Trigger").GetComponent<RadarTrigger>().enabled = true;
                    cameraPositionOffset = thirdPersonPosition;
                    gameObject.GetComponent<Camera>().fieldOfView = 60;
                    
                    break;

            }
        }
    }

    void LateUpdate()
    {
        if (mode == CameraMode.FirstPerson)
        {
            transform.position = target.transform.position;

            transform.rotation = lookAtTarget.rotation;
            transform.Rotate(cameraRotationOffset);

            /*Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(lookAtTarget.position) - cameraPositionOffset);

            if (targetDistance < defaultDistance)
            {
                transform.position = nextPosition; //Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 15f);
            }
            else
            {
                transform.position = nextPosition;
            }


            float distance = Vector3.Distance(transform.position, lookAtTarget.position);
            if (distance < fadeDistance)
            {
                Renderer r = target.transform.Find("Sol Starfighter Advanced Model").GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

            }*/

        }
    }

    private void updateCamera()
    {
        if (mode == CameraMode.ThirdPerson)
        {
            bool hitFound = false;
            
            // Check for objects between ship and camera
            Vector3 dirToCamera = transform.position - target.transform.position;
            int layer = 1 << 11;

            RaycastHit[] hits = Physics.SphereCastAll(target.transform.position, 2.8f, dirToCamera,
                                Mathf.Sqrt(defaultDistance * defaultDistance + defaultHeight * defaultHeight), layer);

            Vector3 cameraPlaneCenter = transform.InverseTransformDirection(transform.position);
            float clipPlane = Camera.main.nearClipPlane;
            cameraPlaneCenter.z = Camera.main.nearClipPlane;

            foreach (RaycastHit ray in hits)
            {
                if (ray.transform.CompareTag("Environment"))
                {
                    targetDistance = ray.distance - minDistPercent;
                    if (targetDistance < maxDistPercent)
                    {
                        targetDistance = maxDistPercent;
                    }
                    float ratio = (2*targetDistance) / (3*defaultDistance);
                    targetHeight = ratio * defaultHeight;
                    cameraPositionOffset.y = targetHeight;
                    cameraPositionOffset.z = targetDistance;
                    hitFound = true;
                    break;
                }

            }

            Vector3 topOfScreen = transform.TransformDirection(new Vector3(0, defaultHeight, 0));
            Vector3 localTargetPos = transform.InverseTransformPoint(target.transform.position);
            Vector3 rayBase = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            Debug.DrawRay(rayBase, transform.position - rayBase, Color.blue);
            hits = Physics.RaycastAll(rayBase, transform.position - rayBase, defaultHeight + 0.2f);
            foreach (RaycastHit ray in hits)
            {
                if (ray.transform.CompareTag("Environment"))
                {
                    Debug.Log("Camera hit 2");
                    targetHeight = ray.distance - minHeightPercent;
                    cameraPositionOffset.y = targetHeight;
                    hitFound = true;
                    break;
                }

            }

            if (!hitFound)
            {

                //Debug.Log("No hit");
                cameraPositionOffset = thirdPersonPosition;
                /* Code for lowering camera when player has forward velocity.
                float forwardVelocity = target.transform.InverseTransformVector(target.GetComponent<Rigidbody>().velocity).z;
                if (forwardVelocity > 0.01f)
                {

                    
                    if (forwardVelocity < 70)
                    {
                        float diff = 70 - forwardVelocity;
                        if (diff < 0) diff = 0;
                        cameraPositionOffset.y = (Mathf.Pow(diff / 70, 0.5f)) * cameraPositionOffset.y;
                        if (cameraPositionOffset.y > 0)
                        {
                            cameraPositionOffset.y = 0;
                        }

                    }
                    else
                    {
                        cameraPositionOffset.y = 0;
                    }


                    //cameraPositionOffset.z += 2 * (forwardVelocity / 90);
                    //Debug.Log("Dist: " + curDist);
                }*/
            }

            

            

            Vector3 nextPosition = target.transform.position - (target.transform.rotation * cameraPositionOffset);


            float snapSpeed = positionDamping;
            if (hitFound)
            {
                snapSpeed = positionDamping * 0.25f;
            }
            Vector3 currentPosition = Vector3.SmoothDamp(thisTransformCache.position, nextPosition, ref velocity, snapSpeed, maxSpeed);
            thisTransformCache.position = currentPosition;
            currentHeight = Mathf.Abs(currentPosition.y - transform.TransformVector(target.transform.position).y);


            Quaternion nextRotation = Quaternion.LookRotation(lookAtTarget.position - thisTransformCache.position, lookAtTarget.transform.up);
            thisTransformCache.rotation = nextRotation;
            float distance = Vector3.Distance(transform.position, rayBase);
            if (distance < fadeDistance)
            {
                Renderer r = target.transform.Find("Sol Starfighter Advanced Model").GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

            }
            else
            {
                Renderer r = target.transform.Find("Sol Starfighter Advanced Model").GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 1);
            }

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

