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
            bulletList.Remove(other.gameObject);
        }
    }

    public int getNumBullets()
    {
        return bulletList.Count;
    }

    public void removeListElement(GameObject obj)
    {
        for(int i = 0; i < bulletList.Count; ++i)
        {
            if (bulletList[i].Equals(obj))
            {
                bulletList.Remove(obj);
                break;
            }
        }
    }
}
