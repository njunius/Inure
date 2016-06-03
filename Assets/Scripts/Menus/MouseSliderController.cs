using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseSliderController : MonoBehaviour
{

    private string[] axisNames = new string[2];

    private Slider mouseSensitivitySlider;
    private InputManager inputs;

    private float defaultSensitivity;
    private bool initialized;

    // Use this for initialization
    void Start()
    {
        initialized = false;

        axisNames[0] = "Pitch";
        axisNames[1] = "Yaw";

        defaultSensitivity = 0.4444444f; // taken from the original value returned by inputs.getInputSensitivity before any edits

        inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
        mouseSensitivitySlider = GetComponent<Slider>();

        mouseSensitivitySlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void Update()
    {
        if (!initialized)
        {
            if(PlayerPrefs.GetFloat("mouseSensitivity", -1) == -1) // no saved value
            {
                mouseSensitivitySlider.value = inputs.getInputSensitivity(axisNames[0]);
            }
            else
            {
                mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivity");
            }
            initialized = true;
        }
    }

    public void ValueChangeCheck()
    {
        for(int i = 0; i < axisNames.Length; ++i)
        {
            inputs.setInputSensitivity(axisNames[i], mouseSensitivitySlider.value);
        }

        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivitySlider.value);
        PlayerPrefs.Save();
    }

    public void resetSensitivity()
    {

        for (int i = 0; i < axisNames.Length; ++i)
        {
            inputs.setInputSensitivity(axisNames[i], defaultSensitivity);
        }

        mouseSensitivitySlider.value = inputs.getInputSensitivity(axisNames[0]);

        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivitySlider.value);
        PlayerPrefs.Save();
    }
}
