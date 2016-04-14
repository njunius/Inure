using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    GameObject core;
	// Use this for initialization
	void Start ()
    {
        core = GameObject.FindGameObjectWithTag("Core");
	}
	
	// Update is called once per frame
	void Update ()
    {
        //int distance = Vector3.Distance(core, transform.position);
        if (distance < 10)
        {
            transform.parent = null;
        }
    }
}
