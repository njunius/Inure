using UnityEngine;
using System.Collections;

public class TutorialEventIncrementer : MonoBehaviour {
    public int targetEventIndex;
    public GameObject TutorialController;
    private TutorialEventManager manager;


	// Use this for initialization
	void Start () {
        manager = TutorialController.GetComponent<TutorialEventManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Turret Trigger"))
        {
            if (manager.eventIndex + 1 == targetEventIndex)
            {
                manager.eventIndex++;
                manager.refresh();
            }
        }
    }
}
