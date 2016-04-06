using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CustomKeyController : MonoBehaviour, IPointerDownHandler
{

    private InputManager inputs;
    private Dictionary<string, InputBinding> inputBindings;
    private Selectable button;
    private string key;
    private bool selected;
    private bool delay;
    private Event controlBindEvent;
    private Canvas settingsScreen;

    public EventSystem events;
    public Text currentKey;
    public string command;
    public bool positiveDirection;

    // Use this for initialization
    void Start()
    {

        inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputManager>();
        currentKey = gameObject.GetComponentInChildren<Text>();
        button = GetComponent<Selectable>();
        settingsScreen = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();

        delay = true;
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {

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

            foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    if(selected && !delay)
                    {
                        if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
                        {
                            inputBindings[command].posAxis = vKey.ToString().ToLower();
                        }
                        else if (inputBindings[command].bidirectional && !positiveDirection)
                        {
                            inputBindings[command].negAxis = vKey.ToString().ToLower();
                        }
                        EventSystem.current.SetSelectedGameObject(null);
                        selected = false;
                        delay = true;
                    }
                    break;
                }
            }

            if(selected && delay)
            {
                delay = false;
            }

            currentKey.text = key.ToUpper();

        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selected = true;
    }
}