using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonReloadScene : MonoBehaviour, IPointerClickHandler {

    private string sceneToLoad;

    // Use this for initialization
    void Start () {
        sceneToLoad = SceneManager.GetActiveScene().name;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
