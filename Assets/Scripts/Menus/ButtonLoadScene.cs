using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonLoadScene : MonoBehaviour, IPointerClickHandler {

    public string sceneToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnPointerClick(PointerEventData eventData) {
        SceneManager.LoadScene(sceneToLoad);
	}
}
