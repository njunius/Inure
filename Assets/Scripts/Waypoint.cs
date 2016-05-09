using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour {

    public GameObject NextPoint;
    public GameObject PrevPoint;
    public bool isBox = true;

    public bool active = false;

    public GameObject target;
    private Vector3 prevDir;

    public RectTransform waypointSprite;
    public Text distance;

    void Start()
    {
        if (isBox)
        {
            target = transform.FindChild("Waypoint Target").gameObject;
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

        if (active && isBox)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            //Vector3 waypointDirection = -1 * transform.eulerAngles;

            Debug.DrawRay(player.position, prevDir, Color.yellow);
            int layer = 1 << 14;
            RaycastHit hit;
            if (Physics.Raycast(player.position, prevDir, out hit, 1000.0f, layer))
            {
                if (hit.transform.gameObject.GetComponent<Waypoint>().active)
                {
                    target.transform.position = hit.point;
                }
                
            }
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            waypointSprite.position = screenPos;

            distance.text = ((int)Vector3.Distance(player.transform.position, transform.position)).ToString();
            //HUD.transform.position = screenPos;
            //HUD.transform.LookAt(Camera.main.transform, Camera.main.transform.up);

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
            
            pc.targetLocked = true;
        }
    }
}
