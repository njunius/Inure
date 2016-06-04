using UnityEngine;
using System.Collections;

public class changeMusic : MonoBehaviour {

    public GameObject Camera;
    public AudioClip newSong;
    private AudioSource source;
	// Use this for initialization
	void Start () {
        source = Camera.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.CompareTag("Player Collider"))
        {
            source.clip = newSong;
            source.Stop();
            source.Play();
        }
    }
}
