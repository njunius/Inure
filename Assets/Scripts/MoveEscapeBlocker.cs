using UnityEngine;
using System.Collections;

public class MoveEscapeBlocker : MonoBehaviour
{
    BossManager bm;
    public float speed;
    public Transform desiredSpot;
	// Use this for initialization
	void Start ()
    {
        bm = GameObject.FindGameObjectWithTag("Bawse").GetComponent<BossManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(bm.activatedDoor)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, desiredSpot.transform.position, step);
        }
	}
}
