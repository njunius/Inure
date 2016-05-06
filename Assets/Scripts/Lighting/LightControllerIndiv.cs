using UnityEngine;
using System.Collections;

public class LightControllerIndiv : MonoBehaviour
{
    public float duration = 0f; // change this in editor to increase the duration light lasts per cycle
    public float lifetime = 0f; // do not touch
    public bool activated = false;
    public float timeLimit = 0f; // do not touch
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        float phi;
        float amplitude;
        if (activated)
        {
            lifetime += Time.deltaTime;
            phi = lifetime / duration * 0.6f * Mathf.PI;
            amplitude = Mathf.Cos(phi) * 1.6f + 0.5f;
            GetComponent<Light>().intensity = amplitude;
        }
	}
}
