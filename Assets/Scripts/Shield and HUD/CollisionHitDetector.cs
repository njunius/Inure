using UnityEngine;
using System.Collections;

public class CollisionHitDetector : MonoBehaviour {

    private FirstPersonHitIndicatorController[] editableIndicators = new FirstPersonHitIndicatorController[2];
    public string[] indicatorNames = new string[2]; 

	// Use this for initialization
	void Start () {

        for (int i = 0; i < editableIndicators.Length; ++i)
        {
            editableIndicators[i] = GameObject.Find(indicatorNames[i]).GetComponent<FirstPersonHitIndicatorController>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateIndicators()
    {
        for(int i = 0; i < editableIndicators.Length; ++i)
        {
            if(editableIndicators[i] != null)
            {
                editableIndicators[i].detectHit();
            }
        }
    }
}
