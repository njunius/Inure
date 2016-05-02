using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonReloadScene : MonoBehaviour, IPointerDownHandler {

    private string sceneToLoad;

    // Use this for initialization
    void Start () {
        sceneToLoad = SceneManager.GetActiveScene().name;
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
