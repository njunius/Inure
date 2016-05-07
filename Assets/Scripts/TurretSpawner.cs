using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretSpawner : MonoBehaviour {

    public GameObject[] TurretConfigurationPrefabs;
    private GameObject childConfiguration;
    public GameObject juggernaut;

    private Turret[] turrets;
    private bool orientationConfirmed = false;

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	    if (!orientationConfirmed)
        {
            Vector3 dirToJug = juggernaut.transform.position - transform.GetChild(0).position;
            RaycastHit[] hits = Physics.RaycastAll(transform.GetChild(0).position, dirToJug, 1000.0f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Environment"))
                {
                    Vector3 other = new Vector3(hit.normal.x, hit.normal.y, hit.normal.z + 15);
                    Vector3 perp = Vector3.Cross(hit.normal, other);
                    transform.rotation = Quaternion.LookRotation(perp, hit.normal);
                    orientationConfirmed = true;
                }
            }
            if (!orientationConfirmed)
            {
                dirToJug = juggernaut.transform.position - transform.GetChild(1).position;
                hits = Physics.RaycastAll(transform.GetChild(1).position, dirToJug, 1000.0f);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.CompareTag("Environment"))
                    {
                        Vector3 other = new Vector3(hit.normal.x, hit.normal.y, hit.normal.z + 15);
                        Vector3 perp = Vector3.Cross(hit.normal, other);
                        transform.rotation = Quaternion.LookRotation(perp, hit.normal);
                        orientationConfirmed = true;
                    }
                }
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Turret Trigger"))
        {
            Vector3 direction = other.gameObject.transform.position - transform.position;
            direction = transform.InverseTransformDirection(direction);
            direction.y = 0;
            Quaternion orientation = Quaternion.LookRotation(transform.TransformDirection(direction), transform.up);

            if (turrets == null)
            {
                

                childConfiguration = (GameObject)Instantiate(TurretConfigurationPrefabs[0], transform.position, orientation);

                turrets = childConfiguration.GetComponentsInChildren<Turret>();
                for (int numTurret = 0; numTurret < turrets.Length; ++numTurret)
                {
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
            else
            {
                childConfiguration.transform.rotation = orientation;
            }
            
        }
    }

    /*void OnTriggerExit(Collider other)
    {
        if (turrets != null)
        {
            for (int numTurret = 0; numTurret < turrets.Length; ++numTurret)
            {
                turrets[numTurret].GetComponent<Turret>().TurnOff();
            }
            turrets = null;
        }
    }*/
}
