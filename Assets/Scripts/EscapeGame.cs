﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscapeGame : MonoBehaviour
{

    private CanvasGroup sceneTransition;

    private bool startTransition;
    private int transitionRate;
    private float transitionCounter;

    void Start()
    {
        sceneTransition = GameObject.FindGameObjectWithTag("Screen Transition").GetComponent<CanvasGroup>();

        startTransition = false;
        transitionRate = 3;
        transitionCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTransition)
        {
            transitionCounter += transitionRate * Time.unscaledDeltaTime;

            sceneTransition.alpha = transitionCounter;
        }

        if (sceneTransition.alpha >= 1 && startTransition)
        {
            SceneManager.LoadScene("AliveEnding");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Collider"))
        {
            startTransition = true;
        }
    }
}
