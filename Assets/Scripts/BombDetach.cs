using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    // GameObject bomb;
    bool detached = false;
    Vector3 offset = new Vector3 (0, -1.139f, -4.079f);
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "ReactorDetail")
        {
            detached = true;
            Debug.Log("Bomb is detached!");
            col.transform.parent = null;
        }
    }
    // Use this for initialization
    void Start ()
    {
        // bomb = GameObject.FindGameObjectWithTag("Bomb");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!detached)
        {
            transform.position = transform.parent.position + offset;
        }
    }
}
