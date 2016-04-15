using UnityEngine;
using System.Collections;

public class BombDetach : MonoBehaviour
{
    bool detached = false;
    float count_down = 10;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "ReactorDetail")
        {
            gameObject.transform.parent = null;
            detached = true;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (detached)
        {
            count_down -= Time.deltaTime;
            //Debug.Log(count_down);
            if (count_down <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
    }
}
