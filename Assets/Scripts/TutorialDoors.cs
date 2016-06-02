using UnityEngine;
using System.Collections;

public class TutorialDoors : MonoBehaviour {
    //public GameObject endPoint;
    private Vector3 endPosition;
    //private Vector3 startPosition;
    private bool active;
    public bool open = false;
    public float speed = 1f;

    private AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        endPosition = transform.FindChild("EndPoint").transform.position;
        //startPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

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
                source.Play();
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
