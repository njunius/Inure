using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SettingsScreenClose : MonoBehaviour, IPointerDownHandler{

    private Canvas settingsOverlay;

	// Use this for initialization
	void Start () {
        settingsOverlay = gameObject.transform.parent.gameObject.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	public void OnPointerDown (PointerEventData eventData) {
        settingsOverlay.enabled = false;
	}
}
