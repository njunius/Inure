using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public void LoadScene(string level)
    {
        Debug.Log("Unpausing");
        PlayerController playerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        playerCon.paused = false;
        Time.timeScale = 1;
        Debug.Log("Loading!");
        SceneManager.LoadScene(level);
    }
}
