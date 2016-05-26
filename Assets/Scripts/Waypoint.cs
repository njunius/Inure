using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {

    public GameObject NextPoint;
    public GameObject PrevPoint;
    public bool isBox = true;

    public bool active = false;

    public GameObject target;
    private Vector3 prevDir;

    public RectTransform waypointSprite;
    public Text distance;

    public bool lockOnNext = true;

    public bool pathDetector = false;
    public bool showyBeams = false;
    public float pathWidth = 250;
    private Vector3 toPrevWaypoint;

    public List<GameObject> DeathLasers;
    [Range(0, 1)]
    public float showBeams = 0;
    private int beamIndex = 0;


    void Start()
    {
        if (isBox)
        {
            target = transform.FindChild("Waypoint Target").gameObject;
        }
        if (pathDetector)
        {
            toPrevWaypoint = PrevPoint.transform.position - transform.position;
            
        }
        
    }

    void Update()
    {
        if (PrevPoint != null)
        {
            transform.LookAt(PrevPoint.transform);
            target.transform.rotation = transform.rotation;
            prevDir = transform.position - PrevPoint.transform.position;
        }

        if (active)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            if (isBox)
            {
                
                //Vector3 waypointDirection = -1 * transform.eulerAngles;

                //Debug.DrawRay(player.position, prevDir, Color.yellow);
                int layer = 1 << 14;
                RaycastHit hit;
                if (Physics.Raycast(player.position, prevDir, out hit, 20000.0f, layer))
                {
                    if (hit.transform.gameObject.GetComponent<Waypoint>().active)
                    {
                        target.transform.position = hit.point;
                    }

                }

                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                float sign = Mathf.Sign(screenPos.z);

                if (sign > 0)
                {
                    if (waypointSprite.GetComponent<Image>().enabled == false)
                    {
                        waypointSprite.GetComponent<Image>().enabled = true;
                        distance.enabled = true;
                    }

                    waypointSprite.position = new Vector3(screenPos.x, screenPos.y, sign);
                    distance.text = ((int)Vector3.Distance(player.transform.position, transform.position)).ToString();
                }
                else
                {
                    if (waypointSprite.GetComponent<Image>().enabled == true)
                    {
                        waypointSprite.GetComponent<Image>().enabled = false;
                        distance.enabled = false;
                    }
                }
            }
            
            
            if (pathDetector)
            {
                Vector3 toPlayer = player.position - transform.position;
                Vector3 nearPoint = (Vector3.Dot(toPlayer, toPrevWaypoint) / Mathf.Pow(toPrevWaypoint.magnitude, 2)) * toPrevWaypoint;
                Vector3 toNearPoint = (nearPoint + transform.position) - player.position;

                if (toNearPoint.magnitude > pathWidth)
                {
                    Debug.Log("OutOfBounds");
                    foreach (GameObject deathLaser in DeathLasers)
                    {
                        deathLaser.GetComponent<GiantDeathLaserOfDoom>().fireAt(player.gameObject.GetComponent<Rigidbody>().velocity + player.position);
                    }
                }
                
                
                if (showyBeams && nearPoint.magnitude / toPrevWaypoint.magnitude < (1 - showBeams))
                {
                    Debug.Log("showy");
                    foreach (GameObject deathLaser in DeathLasers)
                    {
                        Vector3 target = 500 * player.forward + player.position;
                        deathLaser.GetComponent<GiantDeathLaserOfDoom>().fireAt(target);
                    }
                }

            }

        }
        
    }

    public GameObject getTarget()
    {
        return target;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Turret Trigger"))
        {
            PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Waypoint next = NextPoint.GetComponent<Waypoint>();
            if (next != null && next.isBox)
            {
                
                pc.lockOnTarget = next.getTarget();
                next.active = true;
                

            }
            else
            {
                pc.lockOnTarget = NextPoint;
            }
            active = false;
            
            if (lockOnNext)
            {
                pc.targetLocked = true;
            }
            else
            {
                foreach (GameObject deathLaser in DeathLasers)
                {
                    deathLaser.GetComponent<GiantDeathLaserOfDoom>().killPlayer(true);
                }
            }
            
        }
    }
}
