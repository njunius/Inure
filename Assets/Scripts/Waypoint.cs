using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

    public GameObject NextPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Turret Trigger"))
        {
            PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            pc.lockOnTarget = NextPoint;
            pc.targetLocked = true;
        }
    }
}
