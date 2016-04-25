using UnityEngine;
using System.Collections;

public class IdleAnimation : MonoBehaviour
{
    int counter = 0;
    bool tilt = false;
    bool second = false;
    public float minAngle = 0f;
    public float maxAngle = 0f;
    public float speed = 0f;
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //yrotation = transform.rotation.y;
        if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d") && !Input.GetKey("space") && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey("q") && !Input.GetKey("e"))
        {
            //counter++;
            float angle = Mathf.LerpAngle(minAngle, maxAngle, Mathf.PingPong(Time.time / speed, 1.0f));
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
            //Debug.Log("staying still!");
            /*
            if (counter == 20 && tilt == false && second == false)
            {
                transform.Rotate(0, 0, -1);
                tilt = true;
                counter = 0;
                second = true;
            }
            if (counter == 20 && tilt == false && second == true)
            {
                transform.Rotate(0, 0, -10);
                tilt = true;
                counter = 0;
            }
            if (counter == 20 && tilt == true && second == true)
            {
                transform.Rotate(0, 0, 10);
                tilt = false;
                counter = 0;
            }
            */
        }
    }
}
