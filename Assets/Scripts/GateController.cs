using UnityEngine;
using System.Collections;

public class GateController: MonoBehaviour
{
    public bool Raise_Gate = false;
    float FinalHeight;
    public float speed;
    Vector3 OG_Position;
    Vector3 finalPosition = Vector3.zero;
    float step;

    // Use this for initialization
    void Start()
    {
        OG_Position = transform.position;
        step = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (Raise_Gate && transform.position.y == OG_Position.y)
        {
            Debug.Log("YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            finalPosition = new Vector3(transform.position.x, FinalHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, step);
        }

        if(Raise_Gate && transform.position.y == finalPosition.y)
        {
            Debug.Log("AYYYYYYYYYE LLLLMMMMMMAAAAAAOOOOO");
            Raise_Gate = false;
            transform.position = Vector3.MoveTowards(transform.position, OG_Position, step);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Raise_Gate = true;
            FinalHeight = 8f;
        }
    }
}