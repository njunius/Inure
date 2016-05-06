using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretSpawner : MonoBehaviour {

    public GameObject[] TurretConfigurationPrefabs;
    private GameObject childConfiguration;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Turret Trigger"))
        {
            Vector3 direction = other.gameObject.transform.position - transform.position;
            direction = transform.InverseTransformDirection(direction);
            direction.y = 0;
            Debug.DrawRay(transform.position, direction, Color.cyan);
            Quaternion orientation = Quaternion.LookRotation(transform.TransformDirection(direction), transform.up);

            childConfiguration = (GameObject)Instantiate(TurretConfigurationPrefabs[0], transform.position, orientation);
            
            Turret[] turrets = childConfiguration.GetComponentsInChildren<Turret>();
            for (int numTurret = 0; numTurret < turrets.Length; ++numTurret)
            {
                Debug.Log(turrets[numTurret].GetType());
                System.Type turretType = turrets[numTurret].GetType();
                if (turretType == typeof(PointTurret))
                {
                    turrets[numTurret].GetComponent<PointTurret>().target = other.gameObject;
                }
                else if (turretType == typeof(T_Turret))
                {
                    turrets[numTurret].GetComponent<T_Turret>().target = other.gameObject;
                }
                else if (turretType == typeof(TriangleTurret_Flat))
                {
                    turrets[numTurret].GetComponent<TriangleTurret_Flat>().target = other.gameObject;
                }
                else if (turretType == typeof(TriangleTurret_Wall))
                {
                    turrets[numTurret].GetComponent<TriangleTurret_Wall>().target = other.gameObject;
                }
                turrets[numTurret].GetComponent<Turret>().TurnOn();
            }
        }
    }
}
