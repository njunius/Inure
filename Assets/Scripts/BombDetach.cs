using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    private bool detached = false;
    private float count_down = 60;
    private bool escaped = false;
    private bool stopChecking = false;
    //private int counter = 0;
    PlayerController pc;

    // Use this for initialization
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //counter++;
        if (detached)
        {
            count_down -= Time.deltaTime;
            if(count_down <= 0)
            {
                if(escaped)
                {
                    //Debug.Log("if " + counter + ": " + stopChecking);
                    if (!stopChecking)
                    {
                        //Debug.Log("if " + counter + ": " + stopChecking);
                        stopChecking = true;
                        //Debug.Log("if " + counter + ": " + stopChecking);
                        //Debug.Log(escaped);
                        Debug.Log("You Win!");
                        count_down = 0.00f;
                        detached = false;
                    }
                }
                else
                {
                    //Debug.Log("else " + counter + ": " + stopChecking);
                    if (!stopChecking)
                    {
                        //Debug.Log("else " + counter + ": " + stopChecking);
                        stopChecking = true;
                        //Debug.Log("else " + counter + ": " + stopChecking);
                        //Debug.Log("Game Over!");
                        pc.setHullIntegrity(0);
                    }
                }
            }
            /*if (count_down <= 0 && escaped)
            {
                Debug.Log(escaped);
                Debug.Log("You Win!");
            }
            else if (count_down <= 0 && !escaped)
            {
                //Debug.Log("Game Over!");
                pc.setHullIntegrity(0);
            }*/
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
