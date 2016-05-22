using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnDeathTurretsOn : MonoBehaviour {

	public List<GameObject> turretList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit (Collider other)
    {
		if (other.gameObject.CompareTag ("Player Turret Trigger"))
        {
            Debug.Log("YOUR GON DIE!");
            Debug.Log(transform.name);
			toggleTurrets();
		}
	}

	void toggleTurrets ()
    {
		for (int numTurret = 0; numTurret < turretList.Count; ++numTurret)
        {
            Turret turret = turretList[numTurret].GetComponent<Turret>();
            if (turret.isTurretOn())
            {
                turret.TurnOn();
            }
            else
            {
                turret.TurnOff();
            }
			
		}
	}
}
