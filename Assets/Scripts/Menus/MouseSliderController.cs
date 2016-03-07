using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseSliderController : MonoBehaviour
{

    private string[] axisNames = new string[2];

    private Slider mouseSensitivitySlider;
    private InputManager inputs;

    private bool initialized;

    // Use this for initialization
    void Start()
    {
        initialized = false;

        axisNames[0] = "Pitch";
        axisNames[1] = "Yaw";

        inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
        mouseSensitivitySlider = GetComponent<Slider>();

        mouseSensitivitySlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void Update()
    {
        if (!initialized)
        {
            mouseSensitivitySlider.value = inputs.getInputSensitivity(axisNames[0]);
            initialized = true;
        }
    }

    public void ValueChangeCheck()
    {
        for(int i = 0; i < axisNames.Length; ++i)
        {
            inputs.setInputSensitivity(axisNames[i], mouseSensitivitySlider.value);
        }
    }
}
