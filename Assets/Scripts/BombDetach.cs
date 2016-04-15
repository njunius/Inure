﻿using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    private bool detached = false;
    private float count_down = 60;
    private bool escaped = false;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (detached)
        {
            count_down -= Time.deltaTime;
            if (count_down <= 0 && !escaped)
            {
                Debug.Log("Game Over!");
            }
            else if(count_down <= 0 && escaped)
            {
                Debug.Log("You Win!");
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "ReactorDetail")
        {
            gameObject.transform.parent = null;
            detached = true;
        }
    }

    // retrieve the value of the current time
    public float get_countdown()
    {
        return count_down;
    }

    public bool isDetached()
    {
        return detached;
    }

    public void set_escape(bool escape_val)
    {
        escaped = escape_val;
    }
}
