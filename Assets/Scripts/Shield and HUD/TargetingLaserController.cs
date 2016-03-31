using UnityEngine;
using System.Collections;

public class TargetingLaserController : MonoBehaviour {

    private LineRenderer laserSight;
    private Ray drawPoints;
    private RaycastHit laserHit;

    private int layerMask;

	// Use this for initialization
	void Start () {

        laserSight = GetComponent<LineRenderer>();
        layerMask = 0 << 9;
	}
	
	// Update is called once per frame
	void Update () {

        drawPoints = new Ray(transform.position, transform.forward);
        laserSight.SetPosition(0, drawPoints.origin);

        if(Physics.Raycast(drawPoints, out laserHit, 1000, layerMask))
        {
            laserSight.SetPosition(1, laserHit.point);
        }
        else
        {
            laserSight.SetPosition(1, drawPoints.GetPoint(1000.0f));
        }

        laserSight.material.mainTextureOffset = new Vector2(-Time.time / 5, 0);

	}
}
