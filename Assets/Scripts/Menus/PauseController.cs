using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseController : MonoBehaviour {

    private Canvas pauseOverlay;
    private Canvas settingsOverlay;
    private PlayerController player;

    public InputManager im;

    // Use this for initialization
    void Start () {

        pauseOverlay = GameObject.FindGameObjectWithTag("Pause Overlay").GetComponent<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        settingsOverlay = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update () {

        if(!pauseOverlay.enabled && player.paused)
        {
            player.paused = !player.paused;
            Time.timeScale = 1;
            pauseOverlay.enabled = false;
            Cursor.visible = false;
        }

        if (im.getInputDown("Pause") && !player.paused)
        {
            player.paused = !player.paused;
            Time.timeScale = 0;
            pauseOverlay.enabled = true;
            Cursor.visible = true;
        }
        else if (im.getInputDown("Pause") && player.paused)
        {
            if (!settingsOverlay.enabled)
            {
                player.paused = !player.paused;
                Time.timeScale = 1;
                pauseOverlay.enabled = false;
                Cursor.visible = false;
            }
            else if (settingsOverlay.enabled)
            {
                settingsOverlay.enabled = false;
            }
        }
        if (Cursor.visible && !player.paused && !player.isDead())
            Cursor.visible = false;
    }

}