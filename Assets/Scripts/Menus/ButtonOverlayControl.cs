using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonOverlayControl : MonoBehaviour, IPointerDownHandler {

    private Canvas settingsOverlay;

    // Use this for initialization
    void Start () {
        settingsOverlay = GameObject.FindGameObjectWithTag("Settings Screen").GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	public void OnPointerDown (PointerEventData eventData) {
        settingsOverlay.enabled = true;
	}
}
