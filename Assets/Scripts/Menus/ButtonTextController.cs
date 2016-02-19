using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    private Text buttonText;

	// Use this for initialization
	void Start () {
        buttonText = gameObject.GetComponentInChildren<Text>();
        buttonText.enabled = false;
	}
	
	public void OnPointerEnter (PointerEventData eventData) {
        buttonText.enabled = true;
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.enabled = false;
    }

}
