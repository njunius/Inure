using UnityEngine;
using System.Collections;

public class GateController: MonoBehaviour
{
    public bool Raise_Gate = false;
    float FinalHeight = 8f;
    Vector3 OG_Position;
    public float speed = 1f;
    Vector3 finalPosition = Vector3.zero;
    float step;

    // Use this for initialization
    void Start()
    {
        OG_Position = transform.position;
        step = speed * Time.deltaTime;
        finalPosition = new Vector3(transform.position.x, FinalHeight, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Raise_Gate)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, 10 * step);

            if (transform.position == finalPosition)
            {
                Raise_Gate = false;
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
            Raise_Gate = true;
        }
    }
}