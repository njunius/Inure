using UnityEngine;
using System.Collections;

public class MoveEscapeBlocker : MonoBehaviour
{
    BombDetach bd;
    public float speed;
    public Transform desiredSpot;
	// Use this for initialization
	void Start ()
    {
        bd = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombDetach>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(bd.detached)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, desiredSpot.transform.position, step);
        }
	}
}
