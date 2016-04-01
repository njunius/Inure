using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public float duration = 1.0F;
    public Light lt;
    void Start()
    {
        lt = GetComponent<Light>();
        GameObject[] allLights = GameObject.FindGameObjectsWithTag("GLight");
    }
    void Update()
    {
        float phi = Time.time / duration * 0.6f * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.9F + 0.9F;
        lt.intensity = amplitude;
    }
}

/*foreach (GameObject i in "GLight")
        {
            i.SetActive(false);
        }
*/