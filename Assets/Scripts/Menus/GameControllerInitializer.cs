using UnityEngine;
using System.Collections;

public class GameControllerInitializer : MonoBehaviour {

    public GameObject gameController;

    // Use this for initialization
    void Awake () {

        if (GameObject.FindGameObjectWithTag("GameController") == null)
        {
            Instantiate(gameController);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
