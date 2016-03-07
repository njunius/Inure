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

        delay = true;
        selected = false;
    }

    void OnGUI()
    {

        controlBindEvent = Event.current;

        if (selected && controlBindEvent != null)
        {

            if (controlBindEvent.isKey)
            {

                key = controlBindEvent.keyCode.ToString().ToLower();

                if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
                {
                    inputBindings[command].posAxis = key;
                }
                else if (inputBindings[command].bidirectional && !positiveDirection)
                {
                    inputBindings[command].negAxis = key;
                }

                EventSystem.current.SetSelectedGameObject(null);
                selected = false;

                delay = true;
            }
            else if (controlBindEvent.isMouse && !delay)
            {

                // These must be hard coded due to button being a number and not the string needed for the InputManager
                if(controlBindEvent.button == 0)
                    key = "mouse 0";
                else if(controlBindEvent.button == 1)
                    key = "mouse 1";
                else if(controlBindEvent.button == 2)
                    key = "mouse 3";

                if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
                {
                    inputBindings[command].posAxis = key;
                }
                else if (inputBindings[command].bidirectional && !positiveDirection)
                {
                    inputBindings[command].negAxis = key;
                }

                EventSystem.current.SetSelectedGameObject(null);
                selected = false;

                delay = true;
            }
            else if(controlBindEvent.isMouse && delay)
            {
                delay = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (inputBindings == null)
        {
            inputBindings = inputs.getInputBindings();

        }

        if (inputBindings != null)
        {
            if ((inputBindings[command].bidirectional && positiveDirection) || !inputBindings[command].bidirectional)
            {
                key = inputBindings[command].posAxis;
            }
            else if (inputBindings[command].bidirectional && !positiveDirection)
            {
                key = inputBindings[command].negAxis;
            }

        }

        currentKey.text = key.ToUpper();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selected = true;
    }
}