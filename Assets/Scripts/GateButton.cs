using UnityEngine;
using System.Collections;

public class GateButton : MonoBehaviour
{
    bool Raise_Gate = false;
    bool Button_Pressed = false;
    float Pressed_Position = -30.2f;
    float FinalHeight = 8f;
    Vector3 gate_OG_Position;
    Vector3 button_OG_Position;
    public float speed = 1f;
    public float button_speed = 2f;
    Vector3 buttonFinalPosition = Vector3.zero;
    Vector3 finalPosition = Vector3.zero;
    float button_step;
    float gate_step;
    public GameObject moving_gate;

    // Use this for initialization
    void Start()
    {
        button_OG_Position = transform.position;
        gate_OG_Position = moving_gate.transform.position;
        button_step = button_speed * Time.deltaTime;
        gate_step = speed * Time.deltaTime;
        buttonFinalPosition = new Vector3(Pressed_Position, transform.position.y, transform.position.z);
        finalPosition = new Vector3(moving_gate.transform.position.x, FinalHeight, moving_gate.transform.position.z);
    }

    // called once per frame
    void Update()
    {
        if (Button_Pressed)
        {
            Debug.Log("BUTTON IS PRESSED!");
            transform.position = Vector3.MoveTowards(transform.position, buttonFinalPosition, button_step);

            moving_gate.transform.position = Vector3.MoveTowards(moving_gate.transform.position, finalPosition, 10 * gate_step);

            // Move Button back to Original Position once pressed all the way
            if (transform.position == buttonFinalPosition)
            {
                Button_Pressed = false;
            }

            // Move Gate back to original Position once raised all the way
            
            if (moving_gate.transform.position == finalPosition)
            {
                Raise_Gate = false;
            }
            
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, button_OG_Position, button_step);
            moving_gate.transform.position = Vector3.MoveTowards(moving_gate.transform.position, gate_OG_Position, gate_step);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Button_Pressed = true;
            Raise_Gate = true;
        }
    }
}