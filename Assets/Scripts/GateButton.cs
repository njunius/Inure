using UnityEngine;
using System.Collections;

public class GateButton : MonoBehaviour
{
    public bool Button_Pressed = false;
    float Pressed_Position = -30.2f;
    Vector3 OG_Position;
    public float speed = 1f;
    Vector3 finalPosition = Vector3.zero;
    float step;

    // Use this for initialization
    void Start()
    {
        OG_Position = transform.position;
        step = speed * Time.deltaTime;
        finalPosition = new Vector3(Pressed_Position, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Button_Pressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, step);

            if (transform.position == finalPosition)
            {
                Button_Pressed = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, OG_Position, step);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Button_Pressed = true;
        }
    }
}