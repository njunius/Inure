using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAfterClip : MonoBehaviour {

    private AudioSource voiceOver;
    public MovieTexture endingAnimation;

    public string sceneToLoad;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        voiceOver = GetComponent<AudioSource>();
        GetComponent<RawImage>().texture = endingAnimation;

        voiceOver.Play();
        endingAnimation.Play();

        // add start and end delays to the audio clip to make sure the next scene doesn't load before the clip is finished playing
        Invoke("loadScene", voiceOver.clip.length);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void loadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
