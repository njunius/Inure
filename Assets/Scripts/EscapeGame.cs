using UnityEngine;
using System.Collections;

public class EscapeGame : MonoBehaviour
{
    BombDetach bd;
	// Use this for initialization
	void Start ()
    {
        bd = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombDetach>();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            bd.set_escape(true);
        }
    }
}
