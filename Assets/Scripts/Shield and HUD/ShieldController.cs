using UnityEngine;
using System.Collections;


// This script assumes the shield is already a child of the player object with a PlayerController Script attached
public class ShieldController : MonoBehaviour {

    PlayerController player;
	// Use this for initialization
	void Start () {
	    player = (PlayerController)transform.parent.gameObject.GetComponent("PlayerController"); 
    }
	
	// Update is called once per frame
	void Update () {

        // enables and disables the effect and collision volume based on the player's state
        if (player.isShielded())
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
