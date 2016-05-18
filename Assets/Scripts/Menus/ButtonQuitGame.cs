using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonQuitGame : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnPointerClick(PointerEventData eventData) {
        Application.Quit();	
	}
}
