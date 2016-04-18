using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialEventIncrementer : MonoBehaviour {
    public int targetEventIndex;
    public GameObject TutorialController;
    public List<GameObject> Enables;
    public List<GameObject> Disables;
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
            Debug.Log("Target " + targetEventIndex + " " + manager.eventIndex);
            if (manager.eventIndex + 1 == targetEventIndex)
            {
                manager.eventIndex++;
                manager.refresh();
            }

            if (Enables.Count > 0)
            {
                foreach (GameObject g in Enables)
                {
                    g.SetActive(true);
                }
            }
            if (Disables.Count > 0)
            {
                foreach (GameObject g in Enables)
                {
                    g.SetActive(false);
                }
            }


        }
    }
}
