using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmorController : MonoBehaviour {

    public GameObject armorChunk;

	// Use this for initialization
	void Start () {
	    for(int i = 0; i < 5; ++i)
        {
            GameObject temp = (GameObject)Instantiate(armorChunk, gameObject.transform.position, gameObject.transform.rotation);
            temp.transform.SetParent(gameObject.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
