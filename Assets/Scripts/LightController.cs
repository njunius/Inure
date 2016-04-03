using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public int i = 0;
    public GameObject[] all_lights;
    public float lifetime = 0f;
    public float timeLimit = 0.4f;
    void Start()
    {
        for(int j = 0; j < all_lights.Length; j++)
        {
            all_lights[j].GetComponent<Light>().intensity = 0;
        }
    }
    void Update()
    {
        lifetime += Time.deltaTime;
       
        if (lifetime > timeLimit && i < all_lights.Length)
        {
            lifetime = 0;
            all_lights[i].GetComponent<LightControllerIndiv>().activated = true;
            i++;
        }
        if (i >= all_lights.Length)
        {
            i = 0;
        }
    }
}