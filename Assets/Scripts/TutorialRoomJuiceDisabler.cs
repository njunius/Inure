using UnityEngine;
using System.Collections;

public class TutorialRoomJuiceDisabler : MonoBehaviour {
    public GameObject juiceController;


    void OnTriggerEnter(Collider other)
    {

        juiceController.GetComponent<TutorialRoomJuice>().disable();
    }
}
