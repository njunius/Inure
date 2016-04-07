using UnityEngine;
using System.Collections;

public class TutorialDoors : MonoBehaviour {
    public Vector3 endPoint;
    private bool active;
    public bool open = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (active)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, 1f);
            Debug.Log("Opening");
            if (transform.position == endPoint)
            {
                active = false;
                open = !open;
            }
        }
	}

    public void activate()
    {
        Debug.Log("Activate");
        if (transform.position != endPoint)
        {
            active = true;
        }
    }
}
