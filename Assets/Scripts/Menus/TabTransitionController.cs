using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabTransitionController : MonoBehaviour {

    private Image transition;
    private float alpha;
    private bool keyBindingPressed;
    private bool halfway;

    private GameObject[] keybindingElements;
    private GameObject[] gameSettingElements;

    public GameObject[] hudElementSliders;
    public GameObject[] emptySliders;

    // Use this for initialization
    void Start () {
        transition = gameObject.GetComponent<Image>();
        alpha = 0.0f;
        keyBindingPressed = false;
        halfway = false;

        keybindingElements = GameObject.FindGameObjectsWithTag("Keybinding Screen");
        gameSettingElements = GameObject.FindGameObjectsWithTag("Game Settings Screen");

        for (int i = 0; i < gameSettingElements.Length; ++i)
        {
            gameSettingElements[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (transition.enabled)
        {
            
            if(alpha > 1)
            {
                halfway = true;
                alpha = 1.0f;

                if (keyBindingPressed)
                {
                    for (int i = 0; i < keybindingElements.Length; ++i)
                    {
                        keybindingElements[i].SetActive(true);
                    }

                    for (int i = 0; i < gameSettingElements.Length; ++i)
                    {
                        gameSettingElements[i].SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < keybindingElements.Length; ++i)
                    {
                        keybindingElements[i].SetActive(false);
                    }

                    for (int i = 0; i < gameSettingElements.Length; ++i)
                    {
                        gameSettingElements[i].SetActive(true);
                    }

                    for(int i = 0; i < hudElementSliders.Length; ++i)
                    {
                        hudElementSliders[i].SetActive(false);
                    }

                    for(int i = 0; i < emptySliders.Length; ++i)
                    {
                        emptySliders[i].SetActive(true);
                    }
                }
            }
            else if(alpha < 1 && !halfway)
            {
                alpha += 6 * Time.unscaledDeltaTime;
            }
            else if(alpha <= 1 && halfway)
            {
                alpha -= 6 * Time.unscaledDeltaTime;
            }
            if(alpha < 0)
            {
                alpha = 0.0f;
                transition.enabled = false;
                halfway = false;
            }
            transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, alpha);
        }

	}

    // flag true for keybinding was pressed, false for game settings was pressed
    public void startTabTransition(bool keyBinding)
    {
        transition.enabled = true;
        keyBindingPressed = keyBinding;
    }
}
