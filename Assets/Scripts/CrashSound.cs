using UnityEngine;
using System.Collections;

public class CrashSound : MonoBehaviour
{
    //public GameObject projectile;
    public AudioClip crash_sound;

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
        if (crashed)
        {
            float vol = Random.Range(vol_low_range, vol_high_range);
            source.PlayOneShot(crash_sound, vol);
            crashed = false;
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            crashed = true;
        }
    }
}
