using UnityEngine;
using System.Collections;

public class TutorialDoors : MonoBehaviour {
    public GameObject endPoint;
    public Vector3 endPosition;
    public Vector3 startPosition;
    private bool active;
    public bool open = false;
    public float speed = 1f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (startPosition == Vector3.zero)
        {
            startPosition = transform.position;
            endPosition = endPoint.transform.position;

            GameObject.Destroy(endPoint);
        }
        else
        {
            if (active)
            {
                //Debug.Log(transform.position);
                //Debug.Log(endPoint);
                transform.position = Vector3.MoveTowards(transform.position, endPosition, speed);
                //Debug.Log("Opening");
                if (transform.position == endPosition)
                {
                    active = false;
                    open = !open;
                }
            }
        }
	    
	}

    public void activate()
    {
        //Debug.Log("Activate");
        if (transform.position != endPosition)
        {
            active = true;
        }
    }
}
