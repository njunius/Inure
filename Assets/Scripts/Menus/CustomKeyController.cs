using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CustomKeyController : MonoBehaviour, IPointerClickHandler
{

    private CustomKeyController[] keyBindings;
    private InputManager inputs;
    private Dictionary<string, InputBinding> inputBindings;
    private string key;
    private string doubleKeyBindingBuffer;
    private bool selected;
    private bool delay;
    private bool alreadyBound;
    private Canvas settingsScreen;
    private Image keyBuffer;
    private float timerMax;
    private float timer;

    private SettingsExitBuffer canQuitSettings;

    public Text currentKey;
    public string command;
    public bool positiveDirection;
    public Image keyMessage;
    public Text keyMessageText;

    // Use this for initialization
    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Keybinding Button");
        keyBindings = new CustomKeyController[temp.Length];

        for (int i = 0; i < keyBindings.Length; ++i)
        {
            keyBindings[i] = temp[i].GetComponent<CustomKeyController>();
        }

        canQuitSettings = GameObject.FindGameObjectWithTag("GameController").GetComponent<SettingsExitBuffer>();

        inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
        currentKey = gameObject.GetComponentInChildren<Text>();
        settingsScreen = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();
        keyBuffer = GameObject.FindGameObjectWithTag("Key Buffer").GetComponent<Image>();
        timerMax = 0.5f;
        timer = 0.0f;

        delay = true;
        selected = false;

        alreadyBound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs == null)
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
                canQuitSettings.setSelected(true);
                EventSystem.current.SetSelectedGameObject(this.gameObject);

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

                    if (alreadyBound)
                    {
                        alreadyBound = false;
                    }

                    for (int i = 0; i < keyBindings.Length; ++i)
                    {
                        if (vKey.ToString().ToLower().Equals(doubleKeyBindingBuffer))
                        {
                            break;
                        }
                        else if (!delay && vKey.ToString().ToLower().Equals(keyBindings[i].getKey()) && !keyBindings[i].Equals(this))
                        {
                            alreadyBound = true;
                            doubleKeyBindingBuffer = vKey.ToString().ToLower();

                            keyMessage.enabled = true;
                            keyMessageText.enabled = true;
                            keyMessageText.text = "Press again to confirm";
                            break;
                        }
                    }

                    if (selected && !delay && !vKey.ToString().Equals("Escape") && !alreadyBound)
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
                        keyMessage.enabled = false;
                        keyMessageText.enabled = false;
                        selected = false;
                        delay = true;
                        alreadyBound = false;
                        doubleKeyBindingBuffer = "";
                        keyBuffer.enabled = false;

                        canQuitSettings.setSelected(false);
                        break;
                    }
                    else if (selected && !delay && vKey.ToString().Equals("Escape"))
                    {
                        keyMessage.enabled = true;
                        keyMessageText.enabled = true;
                        keyMessageText.text = "Key is reserved";

                        alreadyBound = false;
                    }
                }
            }

            if (selected && delay)
            {
                delay = false;
                keyBuffer.enabled = true;
            }

        }

    }

    public void OnPointerClick(PointerEventData eventData)
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