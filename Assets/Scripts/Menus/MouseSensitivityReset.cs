using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MouseSensitivityReset : MonoBehaviour, IPointerClickHandler {

    public MouseSliderController mouseControl;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mouseControl.resetSensitivity();
    }
}
