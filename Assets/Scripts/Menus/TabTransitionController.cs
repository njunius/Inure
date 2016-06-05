using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabTransitionController : MonoBehaviour {

    private Image transition;
    private float alpha;
    private string buttonName;
    private bool halfway;
    private float transitionDelay;

    private GameObject[] keybindingElements;
    private GameObject[] gameSettingElements;
    private GameObject[] screenSettingElements;

    public GameObject[] hudElementSliders;
    public GameObject[] emptySliders;

    // Use this for initialization
    void Start () {
        transition = gameObject.GetComponent<Image>();
        alpha = 1.0f;
        transitionDelay = 0.0f;

        buttonName = "";

        halfway = false;

        keybindingElements = GameObject.FindGameObjectsWithTag("Keybinding Screen");
        gameSettingElements = GameObject.FindGameObjectsWithTag("Game Settings Screen");
        screenSettingElements = GameObject.FindGameObjectsWithTag("Screen Settings");

        for (int i = 0; i < gameSettingElements.Length; ++i)
        {
            gameSettingElements[i].SetActive(false);
        }
        
        for(int i = 0; i < screenSettingElements.Length; ++i)
        {
            screenSettingElements[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (transition.enabled)
        {
            
            if(transitionDelay > 1)
            {
                halfway = true;
                alpha = 1.0f;
                transitionDelay = 0.0f;

                if (buttonName.Equals("Keybindings Button"))
                {
                    for (int i = 0; i < keybindingElements.Length; ++i)
                    {
                        keybindingElements[i].SetActive(true);
                    }

                    for (int i = 0; i < gameSettingElements.Length; ++i)
                    {
                        gameSettingElements[i].SetActive(false);
                    }

                    for(int i = 0; i < screenSettingElements.Length; ++i)
                    {
                        screenSettingElements[i].SetActive(false);
                    }
                }
                else if(buttonName.Equals("Game Settings"))
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

                    for (int i = 0; i < screenSettingElements.Length; ++i)
                    {
                        screenSettingElements[i].SetActive(false);
                    }
                }
                else if (buttonName.Equals("Screen Settings"))
                {
                    for (int i = 0; i < keybindingElements.Length; ++i)
                    {
                        keybindingElements[i].SetActive(false);
                    }

                    for (int i = 0; i < gameSettingElements.Length; ++i)
                    {
                        gameSettingElements[i].SetActive(false);
                    }

                    for (int i = 0; i < screenSettingElements.Length; ++i)
                    {
                        screenSettingElements[i].SetActive(true);
                    }
                }
            }
            else if(transitionDelay < 1 && !halfway)
            {
                transitionDelay += 5 * Time.unscaledDeltaTime;
            }
            else if(alpha <= 1 && halfway)
            {
                alpha -= 6 * Time.unscaledDeltaTime;
            }
            if(alpha < 0)
            {
                alpha = 1.0f;
                transition.enabled = false;
                halfway = false;
            }
            transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, alpha);
        }

	}

    // starts the tab transition and gets the 
    public void startTabTransition(string buttonPressed)
    {
        transition.enabled = true;
        buttonName = buttonPressed;
    }
}
