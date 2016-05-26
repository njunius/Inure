using UnityEngine;
using System.Collections;

public class GiantDeathLaserOfDoom : MonoBehaviour {
    public GameObject player;
    public float chargeTime = 1.5f;
    public float fireTime = 2.0f;
    public float maxHalo = 1000f;
    public LayerMask layermask;

    private bool isFiring = false;
    private float energy = 0f;

    private Vector3 startTarget;
    private Vector3 target;

    private bool isCharging = false;
    private float charge = 0f;

    private LineRenderer lr;
    private Light chargeHalo;

    private float haloDelta;
    private Vector3 dir;
    //private bool lockDist;

    private float accuracy;


    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        chargeHalo = GetComponent<Light>();
        haloDelta = maxHalo / chargeTime;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!isCharging && !isFiring)
        {
            /*
            dir = player.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 10.0f, dir, out hit,  dir.magnitude * 1.1f, layermask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    isCharging = true;
                    target = transform.position + dir.normalized;
                    Debug.Log("Targeted");
                }

            }*/
        }

        if (isCharging)
        {
            charge += Time.deltaTime;
            chargeHalo.range += haloDelta * Time.deltaTime;

            if (charge >= chargeTime)
            {
                isCharging = false;
                isFiring = true;
                charge = 0f;

                dir = (player.transform.position + player.GetComponent<Rigidbody>().velocity * 2f) - transform.position;
                //dir = (player.transform.position) - transform.position;
                lr.enabled = true;
                //target = transform.position + dir * 2;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, target);

            }
        }

        if (isFiring)
        {
            energy += Time.deltaTime;
            startTarget = transform.position + dir * 2f;
            Vector3 endTarget = transform.position + (player.transform.position - transform.position) * 2;
            target = Vector3.Lerp(startTarget, endTarget, energy / (fireTime * 1.2f));
            Vector3 newDir = target - transform.position;
            lr.SetPosition(1, target);
            
            if (chargeHalo.range > 0)
            {
                chargeHalo.range -= Time.deltaTime * 2 * haloDelta;
            }

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5f, newDir, Vector3.Distance(transform.position, target), layermask, QueryTriggerInteraction.Ignore);
            Vector3 closestHit = target;
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Environment"))
                {
                    
                    if (Vector3.Distance(transform.position, hit.point) < Vector3.Distance(transform.position, closestHit))
                    {
                        closestHit = hit.point;
                        //Debug.Log(hit.transform.name);
                    }
                    

                }
                else if (hit.transform.CompareTag("Player"))
                {
                    //Debug.Log("Hit");
                    if (Vector3.Distance(transform.position, hit.point) < Vector3.Distance(transform.position, closestHit))
                    {

                        player.GetComponent<PlayerController>().setHullIntegrity(0);
                        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        player.transform.position = player.transform.position;
                    }
                    
                }

            }
            if (energy >= fireTime)
            {
                isFiring = false;
                energy = 0f;
                lr.enabled = false;
                //lockDist = false;
            }

            lr.SetPosition(1, closestHit);

        }
        


    }

    public void fireAt(Vector3 target)
    {
        if(!isCharging && !isFiring)
        {
            this.target = target;
            startTarget = target;
            isCharging = true;
        }
    }

}
