using UnityEngine;
using System.Collections;

public class CollisionHitDetector : MonoBehaviour {

    public FirstPersonHitIndicatorController[] editableIndicators = new FirstPersonHitIndicatorController[2]; 

	// Use this for initialization
	void Start () {
	
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
