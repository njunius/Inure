using UnityEngine;
using System.Collections;

public class AlertLight : MonoBehaviour {
    public float minIntensityPercent;
    public float upSpeed = 1;
    public float downSpeed = 1;
    private float minIntensity;
    private float maxIntensity;
    private bool flashUp = false;
    private float timeStart;
    private float time = 0;
    private Light flashingLight;

	// Use this for initialization
	void Start () {
        flashingLight = GetComponent<Light>();
        maxIntensity = flashingLight.intensity;
        minIntensity = maxIntensity * (minIntensityPercent/100);
        timeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        time = Time.time - timeStart;
        if (!flashUp)
        {
            flashingLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, time * downSpeed);
            if (flashingLight.intensity <= minIntensity)
            {
                flashUp = true;
                timeStart = Time.time;
                time = 0;
            }
        }
        else
        {
            flashingLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, time * upSpeed);
            if (flashingLight.intensity >= maxIntensity)
            {
                flashUp = false;
                timeStart = Time.time;
                time = 0;
            }
        }
	
	}
}
