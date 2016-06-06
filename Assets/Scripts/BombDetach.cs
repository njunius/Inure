using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    public bool detached = false;
    private float count_down = 65;
    private bool escaped = false;
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
