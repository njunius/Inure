﻿using UnityEngine;
using System.Collections;

public class BulletSound : MonoBehaviour
{
    //public GameObject projectile;
    public AudioClip shoot_sound;
    public AudioClip machinegun_sound;

    //private float bullet_speed = 2000f;
    private AudioSource source;
    private float vol_low_range = 0.5f;
    private float vol_high_range = 1.0f;
    public bool crashed = false;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("PLAYING FIRING SOUND!");
            float vol = Random.Range(vol_low_range, vol_high_range);
            source.PlayOneShot(shoot_sound, vol);
        }
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("PLAYING machinegun SOUND!");
            float vol = Random.Range(vol_low_range, vol_high_range);
            source.PlayOneShot(machinegun_sound, vol);
        }
    }
}
