using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeGame : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Collider"))
        {
            SceneManager.LoadScene("AliveEnding");
        }
    }
}
