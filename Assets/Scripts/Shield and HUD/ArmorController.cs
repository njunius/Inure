using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmorController : MonoBehaviour {

    public Bullet bullet;
    private PlayerController player;

    public GameObject armorChunk;
    private Image[] armorChunkTracker;

    private int playerMaxHull;
    private int playerCurrHull;
    private int bulletDamage;

    private int maxChunks;
    private int currChunks;
    private Color chunkOn;
    private Color chunkOff;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        currChunks = maxChunks = 100 / 50;

        armorChunkTracker = new Image[maxChunks];

        for (int i = 0; i < maxChunks; ++i)
        {
            GameObject temp = (GameObject)Instantiate(armorChunk, gameObject.transform.position, gameObject.transform.rotation);
            temp.transform.SetParent(gameObject.transform);
            armorChunkTracker[i] = temp.GetComponent<Image>();
            chunkOn = armorChunkTracker[i].color;
            chunkOff = new Color(armorChunkTracker[i].color.r, armorChunkTracker[i].color.g, armorChunkTracker[i].color.b, 0.0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        currChunks = player.getCurrHullIntegrity() / 50;

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
