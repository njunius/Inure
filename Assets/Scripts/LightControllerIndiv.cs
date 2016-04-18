using UnityEngine;
using System.Collections;

public class LightControllerIndiv : MonoBehaviour
{
    public float duration = 0f;
    public float lifetime = 0f;
    public bool activated = false;
    public float timeLimit = 0f;
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
            amplitude = Mathf.Cos(phi) * 1.6f + 0.1f;
            GetComponent<Light>().intensity = amplitude;
        }
	}
}
