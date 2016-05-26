using UnityEngine;
using System.Collections;

public class TutorialRoomJuice : MonoBehaviour {

    public Light[] lights;
    private float[] lightTimers;

    AudioSource[] sources;

    private float timer = 0;

    private bool active = true;

	// Use this for initialization
	void Start () {
        lights = GetComponentsInChildren<Light>();
        lightTimers = new float[lights.Length];
        for (int i = 0; i < lights.Length; ++i)
        {
            lightTimers[i] = 0;
        }
        sources = GetComponents<AudioSource>();
        timer = .9f;
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                fireEvent();
                timer = Random.Range(0.5f, 1.7f);
            }

            for (int i = 0; i < lights.Length; ++i)
            {
                if (!lights[i].enabled)
                {
                    lightTimers[i] -= Time.deltaTime;
                    if (lightTimers[i] <= 0)
                    {
                        lights[i].enabled = true;
                    }
                }
            }
        }
        
        
	}

    void fireEvent()
    {
        foreach(AudioSource source in sources)
        {
            if (!source.isPlaying)
            {
                source.Play();
                break;
            }
        }
        
        for (int i = 0; i < lights.Length; ++i)
        {
            if (lights[i].enabled)
            {
                lights[i].enabled = false;
                lightTimers[i] = Random.Range(0.2f, 1);
            }
        }
    }

    public void disable()
    {
        foreach (Light l in lights)
        {
            l.enabled = true;
        }
        active = false;
    }
}
