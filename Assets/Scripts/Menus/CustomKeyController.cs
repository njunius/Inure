using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CustomKeyController : MonoBehaviour, IPointerDownHandler
{

    private InputManager inputs;
    private Dictionary<string, InputBinding> inputBindings;
    private string key;
    private bool selected;
    private bool delay;
    private Canvas settingsScreen;
    private Image keyBuffer;
    private float timerMax;
    private float timer;

    public Text currentKey;
    public string command;
    public bool positiveDirection;

    // Use this for initialization
    void Start()
    {

        inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
        currentKey = gameObject.GetComponentInChildren<Text>();
        settingsScreen = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();
        keyBuffer = GameObject.FindGameObjectWithTag("Key Buffer").GetComponent<Image>();
        timerMax = 0.5f;
        timer = 0.0f;

        delay = true;
        selected = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(inputs == null)
        {
            inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
        }

        if (inputBindings == null)
        {
            inputBindings = inputs.getInputBindings();

        }

        if (inputBindings != null && settingsScreen.enabled)
        {
            if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
            {
                key = inputBindings[command].posAxis;
            }
            else if (inputBindings[command].bidirectional && !positiveDirection)
            {
                key = inputBindings[command].negAxis;
            }

            if (selected)
            {
                if (timer < timerMax)
                {
                    timer += Time.unscaledDeltaTime;
                }
                else if (timer >= timerMax)
                {
                    timer = 0;
                    if (currentKey.text.Equals("_"))
                    {
                        currentKey.text = "";
                    }
                    else
                    {
                        currentKey.text = "_";
                    }
                }
            }
            else
            {
                currentKey.text = key.ToUpper();

            }

            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    if(selected && !delay)
                    {
                        if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
                        {
                            inputBindings[command].posAxis = vKey.ToString().ToLower();
                            currentKey.text = key.ToUpper();

                        }
                        else if (inputBindings[command].bidirectional && !positiveDirection)
                        {
                            inputBindings[command].negAxis = vKey.ToString().ToLower();
                            currentKey.text = key.ToUpper();

                        }
                        EventSystem.current.SetSelectedGameObject(null);
                        selected = false;
                        delay = true;
                        keyBuffer.enabled = false;
                    }
                    break;
                }
            }

            if(selected && delay)
            {
                delay = false;
                keyBuffer.enabled = true;
            }

        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selected = true;
    }

    // defaultKey must be lowercase to work properly
    public void resetKey(string defaultKey)
    {

        /*if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
        {
            key = inputBindings[command].posAxis;
        }
        else if (inputBindings[command].bidirectional && !positiveDirection)
        {
            key = inputBindings[command].negAxis;
        }*/

        if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
        {
            inputBindings[command].posAxis = defaultKey;
            key = inputBindings[command].posAxis;
            currentKey.text = key.ToUpper();

        }
        else if (inputBindings[command].bidirectional && !positiveDirection)
        {
            inputBindings[command].negAxis = defaultKey;
            key = inputBindings[command].negAxis;
            currentKey.text = key.ToUpper();

        }
        EventSystem.current.SetSelectedGameObject(null);
        selected = false;
        delay = true;
        keyBuffer.enabled = false;
    }

    public string getKey()
    {
        return key;
    }
}