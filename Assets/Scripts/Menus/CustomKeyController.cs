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

    private bool initialized;

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
        initialized = false;
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

        // for the command, initialize to a new key value if the key in PlayerPrefs is not the default
        // the two different if statements cover the types of commands inside inputBindings
        if (!initialized)
        {
            if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
            {
                if (PlayerPrefs.GetString(command + ".posAxis", "defaultValue") != "defaultValue")
                {
                    inputBindings[command].posAxis = PlayerPrefs.GetString(command + ".posAxis", inputBindings[command].posAxis);

                }
                key = inputBindings[command].posAxis;
            }
            else if (inputBindings[command].bidirectional && !positiveDirection)
            {
                if (PlayerPrefs.GetString(command + "negAxis", "defaultValue") != "defaultValue")
                {
                    inputBindings[command].negAxis = PlayerPrefs.GetString(command + ".negAxis", inputBindings[command].posAxis);

                }
                key = inputBindings[command].negAxis;

            }
            initialized = true;
        }

        // if we have an inputBindings and we are on the settingsScreen then we can check update the key if necessary
        if (inputBindings != null && settingsScreen.enabled)
        {
            // first make sure the key is its expected value if it is not the defaultValue
            if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
            {
                if (PlayerPrefs.GetString(command + ".posAxis", "defaultValue") != "defaultValue")
                {
                    inputBindings[command].posAxis = PlayerPrefs.GetString(command + ".posAxis", inputBindings[command].posAxis);

                }
                key = inputBindings[command].posAxis;
            }
            else if (inputBindings[command].bidirectional && !positiveDirection)
            {
                if (PlayerPrefs.GetString(command + "negAxis", "defaultValue") != "defaultValue")
                {
                    inputBindings[command].negAxis = PlayerPrefs.GetString(command + ".negAxis", inputBindings[command].posAxis);

                }
                key = inputBindings[command].negAxis;

            }

            // if the player has selected this button
            if (selected)
            {
                // let other objects know so they can behave accordingly
                canQuitSettings.setSelected(true);
                EventSystem.current.SetSelectedGameObject(this.gameObject);

                // signal to the player that the button is waiting for them to press a key by flashing an underscore
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

            // for every key in the current system, go through and check if it was pressed
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
                        // if the key was already pressed and it was the same key, confirm the player's choice
                        if (vKey.ToString().ToLower().Equals(doubleKeyBindingBuffer))
                        {
                            break;
                        }
                        // other wise prompt them to confirm their selection or press a different key
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

                    // if the button is selected, the player isn't trying to use a reserved key, and they have confirmed their selection in the case of assigning an already bound key
                    if (selected && !delay && !vKey.ToString().Equals("Escape") && !alreadyBound)
                    {
                        if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
                        {
                            inputBindings[command].posAxis = vKey.ToString().ToLower();
                            currentKey.text = key.ToUpper();

                            PlayerPrefs.SetString(command + ".posAxis", inputBindings[command].posAxis);
                        }
                        else if (inputBindings[command].bidirectional && !positiveDirection)
                        {
                            inputBindings[command].negAxis = vKey.ToString().ToLower();
                            currentKey.text = key.ToUpper();

                            PlayerPrefs.SetString(command + ".negAxis", inputBindings[command].negAxis);
                        }
                        // reset all book-keeping variables including saving keys out to PlayerPrefs and then exit the loop
                        EventSystem.current.SetSelectedGameObject(null);
                        keyMessage.enabled = false;
                        keyMessageText.enabled = false;
                        selected = false;
                        delay = true;
                        alreadyBound = false;
                        doubleKeyBindingBuffer = "";
                        keyBuffer.enabled = false;

                        canQuitSettings.setSelected(false);

                        PlayerPrefs.Save();
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
        // sets the command to its stored default value
        if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
        {
            inputBindings[command].posAxis = defaultKey;
            key = inputBindings[command].posAxis;
            currentKey.text = key.ToUpper();

            PlayerPrefs.SetString(command + ".posAxis", inputBindings[command].posAxis);
        }
        else if (inputBindings[command].bidirectional && !positiveDirection)
        {
            inputBindings[command].negAxis = defaultKey;
            key = inputBindings[command].negAxis;
            currentKey.text = key.ToUpper();

            PlayerPrefs.SetString(command + ".negAxis", inputBindings[command].negAxis);
        }
        // reset all book-keeping variables and update PlayerPrefs
        EventSystem.current.SetSelectedGameObject(null);
        selected = false;
        delay = true;
        keyBuffer.enabled = false;

        PlayerPrefs.Save();
    }

    public string getKey()
    {
        return key;
    }
}