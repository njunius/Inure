﻿using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    public GameObject NextPoint;
    public GameObject PrevPoint;
    public bool isBox = true;

    public bool active = false;

    public GameObject target;
    private Vector3 prevDir;

    void Start()
    {
        if (isBox)
        {
            target = transform.FindChild("Waypoint Target").gameObject;
        }
    }

    void Update()
    {
        if (PrevPoint != null)
        {
            transform.LookAt(PrevPoint.transform);
            target.transform.rotation = transform.rotation;
            prevDir = transform.position - PrevPoint.transform.position;
        }

        if (active && isBox)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            //Vector3 waypointDirection = -1 * transform.eulerAngles;

            Debug.DrawRay(player.position, prevDir, Color.yellow);
            int layer = 1 << 14;
            RaycastHit hit;
            if (Physics.Raycast(player.position, prevDir, out hit, 1000.0f, layer))
            {
                if (hit.transform.gameObject.GetComponent<Waypoint>().active)
                {
                    target.transform.position = hit.point;
                }
                
            }

        }
        
    }

    public GameObject getTarget()
    {
        return target;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Turret Trigger"))
        {
            PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Waypoint next = NextPoint.GetComponent<Waypoint>();
            if (next != null && next.isBox)
            {
                
                pc.lockOnTarget = next.getTarget();
                next.active = true;
                next.GetComponent<SpriteRenderer>().enabled = true;
                if (isBox)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                

            }
            else
            {
                pc.lockOnTarget = NextPoint;
            }
            active = false;
            
            pc.targetLocked = true;
        }
    }
}
