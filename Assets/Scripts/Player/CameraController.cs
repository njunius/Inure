﻿using UnityEngine;
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
        fadeDistance = defaultDistance * 0.25f;

        mode = CameraMode.ThirdPerson;

        firstPersonPosition = new Vector3(0, -0.35f, 0);
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
        else
        {
            if (!target)
            {
                return;
            }
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
                        mode = CameraMode.ThirdPerson;
                        break;

                }
            }


            updateCamera();

        }
        
    }

    private void updateCamera()
    {
        if (mode == CameraMode.ThirdPerson)
        {
            bool hitFound = false;
            
            // Check for objects between ship and camera
            Vector3 dirToCamera = transform.position - target.transform.position;
            RaycastHit[] hits = Physics.RaycastAll(target.transform.position, dirToCamera,
                                Mathf.Sqrt(defaultDistance * defaultDistance + defaultHeight * defaultHeight));
            foreach (RaycastHit ray in hits)
            {
                if (ray.transform.CompareTag("Environment"))
                {
                    targetDistance = ray.distance - 0.8f;
                    float ratio = targetDistance / defaultDistance;
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

            /*transform.rotation = Quaternion.Lerp(transform.rotation, positionTarget.rotation, Time.deltaTime);
            transform.rotation = positionTarget.rotation;
            transform.Rotate(cameraRotationOffset);*/

            

            //Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(positionTarget.position) - cameraPositionOffset);

            

            Vector3 nextPosition = target.transform.position - (target.transform.rotation * cameraPositionOffset);
            /*currentDistance = Vector3.Distance(thisTransformCache.position, nextPosition);
            Debug.Log(currentDistance);
            if (currentDistance > 0.01f)
            {
                float shift = currentDistance;
                if (shift > 1) shift = 1;
                nextPosition = transform.TransformDirection(nextPosition.x, nextPosition.y - nextPosition.y / shift, nextPosition.z);
            }*/

            float snapSpeed = positionDamping;
            if (hitFound)
            {
                snapSpeed = positionDamping * 0.25f;
            }
            Vector3 currentPosition = Vector3.SmoothDamp(thisTransformCache.position, nextPosition, ref velocity, snapSpeed, maxSpeed);
            thisTransformCache.position = currentPosition;


            Quaternion nextRotation = Quaternion.LookRotation(lookAtTarget.position - thisTransformCache.position, lookAtTarget.transform.up);
            thisTransformCache.rotation = nextRotation;
            /*float distance = Vector3.Distance(transform.position, positionTarget.position);
            if (distance < fadeDistance)
            {
                Renderer r = target.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

            }*/

        }
        else
        {
            cameraPositionOffset = firstPersonPosition;

            transform.rotation = lookAtTarget.rotation;
            transform.Rotate(cameraRotationOffset);

            Vector3 nextPosition = transform.TransformPoint(transform.InverseTransformPoint(lookAtTarget.position) - cameraPositionOffset);

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
                Renderer r = target.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, distance / fadeDistance);

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

