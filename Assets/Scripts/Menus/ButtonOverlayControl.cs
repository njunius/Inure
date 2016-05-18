﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonOverlayControl : MonoBehaviour, IPointerClickHandler {

    public Canvas overlay;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	public void OnPointerClick(PointerEventData eventData) {
        overlay.enabled = !overlay.enabled;
	}
}
