using UnityEngine;
using System.Collections;

public class EscapeGame : MonoBehaviour
{
    BombDetach bd = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BombDetach>();
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "New Player")
        {
            bd.set_escape(true);
        }
    }
}
