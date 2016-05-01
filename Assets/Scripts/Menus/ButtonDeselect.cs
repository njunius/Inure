using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonDeselect : MonoBehaviour, IPointerDownHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
