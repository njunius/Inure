using UnityEngine;
using System.Collections;

public class IdleAnimation : MonoBehaviour
{
    int counter = 0;
    bool tilt = false;
    bool second = false;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d") && !Input.GetKey("space") && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey("q") && !Input.GetKey("e"))
        {
            counter++;
            //Debug.Log("staying still!");
            if (counter == 20 && tilt == false && second == false)
            {
                transform.Rotate(0, 0, -1);
                tilt = true;
                counter = 0;
                second = true;
            }
            if (counter == 20 && tilt == false && second == true)
            {
                transform.Rotate(0, 0, -4);
                tilt = true;
                counter = 0;
            }
            if (counter == 20 && tilt == true && second == true)
            {
                transform.Rotate(0, 0, 4);
                tilt = false;
                counter = 0;
            }
        }
    }
}
