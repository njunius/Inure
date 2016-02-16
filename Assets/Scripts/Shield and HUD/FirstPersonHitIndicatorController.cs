using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstPersonHitIndicatorController : MonoBehaviour {

    private Image hitViewer;
    private bool isHit;
    private float fillTimer;

	// Use this for initialization
	void Start () {
        isHit = false;
        hitViewer = GetComponent<Image>();
        hitViewer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isHit)
        {
            if(hitViewer.color.a < 1.0)
            {
                hitViewer.color = new Color(hitViewer.color.r, hitViewer.color.g, hitViewer.color.b, hitViewer.color.a + Time.deltaTime);
            }
            if(hitViewer.color.a >= 1.0)
            {
                hitViewer.color = new Color(hitViewer.color.r, hitViewer.color.g, hitViewer.color.b, 1.0f);
                isHit = false;
            }
        }
        else if(!isHit && hitViewer.color.a > 0.0f)
        {
            if (hitViewer.color.a > 0.0)
            {
                hitViewer.color = new Color(hitViewer.color.r, hitViewer.color.g, hitViewer.color.b, hitViewer.color.a - Time.deltaTime);
            }
            if (hitViewer.color.a <= 0.0)
            {
                hitViewer.color = new Color(hitViewer.color.r, hitViewer.color.g, hitViewer.color.b, 0.0f);
            }
        }
	}

    public void detectHit()
    {
        if (!isHit)
        {
            isHit = true;
        }
    }
}
