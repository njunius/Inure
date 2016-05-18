using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonDeselect : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
