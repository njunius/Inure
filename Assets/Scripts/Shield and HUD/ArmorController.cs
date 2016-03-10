using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmorController : MonoBehaviour {

    private PlayerController player;

    public GameObject armorChunk;
    private Image[] armorChunkTracker;

    private int maxChunks;
    private int currChunks;

    private Color chunkOn;
    private Color chunkOff;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        currChunks = maxChunks = player.getMaxHullIntegrity();

        armorChunkTracker = new Image[maxChunks];

        for (int i = 0; i < maxChunks; ++i)
        {
            GameObject temp = (GameObject)Instantiate(armorChunk, gameObject.transform.position, gameObject.transform.rotation);
            temp.transform.SetParent(gameObject.transform);
            armorChunkTracker[i] = temp.GetComponent<Image>();
            chunkOn = armorChunkTracker[i].color;
            chunkOff = new Color(chunkOn.r, chunkOn.g, chunkOn.b, 0.0f);
            if(i % 2 != 0)
            {
                armorChunkTracker[i].gameObject.transform.Rotate(new Vector3(180, 0, 0));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        currChunks = player.getCurrHullIntegrity();

        for(int i = 0; i < currChunks; ++i)
        {
            armorChunkTracker[i].color = chunkOn;
        }
        for(int i = currChunks; i < maxChunks; ++i)
        {
            armorChunkTracker[i].color = chunkOff;
        }
	}
}
