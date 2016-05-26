using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VsyncToggleController : MonoBehaviour, IPointerClickHandler {

    private Text vSyncToggleText;

	// Use this for initialization
	void Awake () {

        vSyncToggleText = GetComponentInChildren<Text>();
        
        if(QualitySettings.vSyncCount == 1)
        {
            vSyncToggleText.text = "Vertical Sync On";

        }
        else if(QualitySettings.vSyncCount == 0)
        {
            vSyncToggleText.text = "Vertical Sync Off";

        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if(QualitySettings.vSyncCount == 0)
        {
            QualitySettings.vSyncCount = 1;
            vSyncToggleText.text = "Vertical Sync On";

        }
        else if(QualitySettings.vSyncCount == 1)
        {
            QualitySettings.vSyncCount = 0;
            vSyncToggleText.text = "Vertical Sync Off";

        }
    }
}
