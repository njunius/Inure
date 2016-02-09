using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreatTriggerController : MonoBehaviour {

    private List<GameObject> bulletList;

	// Use this for initialization
	void Start () {
        bulletList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < bulletList.Count; ++i)
        {
            if (!bulletList[i].activeSelf)
            {
                bulletList.RemoveAt(i);
                --i;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            bulletList.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            bulletList.RemoveAt(bulletList.IndexOf(other.gameObject));
        }
    }

    public int getNumBullets()
    {
        return bulletList.Count;
    }
}
