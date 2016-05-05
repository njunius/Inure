using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmorController : MonoBehaviour, HUDElement {

    private PlayerController player;
    private HUDColorController hudColorController;
    private string hudElementName;
    public GameObject armorChunk;
    private Image[] armorChunkTracker;

    private int maxChunks;
    private int currChunks;
    private int arrayMidpoint;

    private Color chunkOn;
    private Color chunkOff;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        hudElementName = "armor";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        currChunks = maxChunks = player.getMaxHullIntegrity();

        arrayMidpoint = maxChunks / 2;

        armorChunkTracker = new Image[maxChunks];

        for (int i = 0; i < maxChunks; ++i)
        {
            GameObject temp = (GameObject)Instantiate(armorChunk, gameObject.transform.position, gameObject.transform.rotation);
            temp.transform.SetParent(gameObject.transform);
            armorChunkTracker[i] = temp.GetComponent<Image>();
            armorChunkTracker[i].color = hudColorController.getColorByString(hudElementName);
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

        for (int i = 0; i < arrayMidpoint - currChunks / 2; ++i)
        {
            armorChunkTracker[i].color = chunkOff;
        }

        for (int i = arrayMidpoint - currChunks / 2; i < arrayMidpoint + currChunks / 2 + 1; ++i)
        {
            armorChunkTracker[i].color = chunkOn;
        }

        for(int i = arrayMidpoint + currChunks / 2; i < maxChunks; ++i)
        {
            if (currChunks % 2 == 0)
            {
                armorChunkTracker[i].color = chunkOff;
            }
        }

        if(currChunks == maxChunks)
        {
            armorChunkTracker[armorChunkTracker.Length - 1].color = chunkOn;
        }

        if(currChunks == 1)
        {
            armorChunkTracker[arrayMidpoint].color = chunkOn;
        }
    }

    public void UpdateColor()
    {
        chunkOn = hudColorController.getColorByString(hudElementName);
        chunkOff = new Color(chunkOn.r, chunkOn.g, chunkOn.b, 0.0f);
    }
}
