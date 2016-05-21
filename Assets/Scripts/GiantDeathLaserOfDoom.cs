using UnityEngine;
using System.Collections;

public class GiantDeathLaserOfDoom : MonoBehaviour {
    public GameObject player;
    public float chargeTime = 1.5f;
    public float fireTime = 2.0f;
    public float maxHalo = 1000f;


    private bool isFiring = false;
    private float energy = 0f;

    private Vector3 target;

    private bool isCharging = false;
    private float charge = 0f;

    private LineRenderer lr;
    private Light chargeHalo;

    private float haloDelta;
    private Vector3 dir;
    private bool lockDist;


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
            dir = player.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 20.0f, dir, out hit))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    isCharging = true;
                    target = transform.position + dir.normalized;
                }
            }
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

                dir = player.transform.position - transform.position;
                
                lr.enabled = true;
                
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, target);

            }
        }

        if (isFiring)
        {
            energy += Time.deltaTime;
            target = transform.position + dir * energy * 4f;
            if (!lockDist)
            {
                lr.SetPosition(1, target);
            }
            
            if (chargeHalo.range > 0)
            {
                chargeHalo.range -= Time.deltaTime * 2 * haloDelta;
            }
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, dir, Vector3.Distance(transform.position, target));
            foreach (RaycastHit hit in hits)
            {
               
                if (hit.transform.CompareTag("Player Collider"))
                {
                    player.GetComponent<PlayerController>().setHullIntegrity(0);
                    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    player.transform.position = player.transform.position;
                }
                else if (hit.transform.CompareTag("Environment"))
                {
                    lockDist = true;
                }
            }
            if (energy >= fireTime)
            {
                isFiring = false;
                energy = 0f;
                lr.enabled = false;
                lockDist = false;
            }
           


        }
        

    }
}
