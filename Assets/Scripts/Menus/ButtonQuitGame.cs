using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonQuitGame : MonoBehaviour, IPointerDownHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnPointerDown (PointerEventData eventData) {
        Application.Quit();	
	}
}
