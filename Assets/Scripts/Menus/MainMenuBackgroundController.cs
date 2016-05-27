using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuBackgroundController : MonoBehaviour {

    public MovieTexture menuBackground;

	// Use this for initialization
	void Start () {
        GetComponent<RawImage>().texture = menuBackground as MovieTexture;
        menuBackground.loop = true;
        menuBackground.Play();
    }

    // Update is called once per frame
    void Update () {

	}
}
