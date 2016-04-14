using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    // GameObject bomb;
    bool detached = false;
    bool counter_on = false;
    float count_down = 60;
    //bool countOn = false;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "ReactorDetail")
        {
            detached = true;
            //countOn = true;
        }
    }
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (detached)
        {
            transform.parent = null;
            counter_on = true;
        }
        if (counter_on)
        {
            count_down -= Time.deltaTime;
            //Debug.Log(count_down);
            if (count_down <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
        /*
        if(countOn)
        {
            Debug.Log("Countdown initiated.");
            countOn = false;
        }
        */
    }
}
