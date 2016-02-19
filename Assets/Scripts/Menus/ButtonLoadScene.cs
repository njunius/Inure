using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonLoadScene : MonoBehaviour, IPointerDownHandler{

    public string sceneToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnPointerDown (PointerEventData eventData) {
        SceneManager.LoadScene(sceneToLoad);
	}
}
