using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmorController : MonoBehaviour, HUDElement {

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
        hudElementName = "armor";
        hudColorController = GameObject.FindGameObjectWithTag("GameController").GetComponent<HUDColorController>();

        maxChunks = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getMaxHullIntegrity();
        currChunks = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getCurrHullIntegrity();

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

        updateChunks(currChunks);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void updateChunks(int newHullIntegrity)
    {
        currChunks = newHullIntegrity;

        for (int i = 0; i < arrayMidpoint - currChunks / 2; ++i)
        {
            armorChunkTracker[i].color = chunkOff;
        }

        for (int i = arrayMidpoint - currChunks / 2; i < arrayMidpoint + currChunks / 2 + 1; ++i)
        {
            armorChunkTracker[i].color = chunkOn;
        }

        for (int i = arrayMidpoint + currChunks / 2; i < maxChunks; ++i)
        {
            if (currChunks % 2 == 0)
            {
                armorChunkTracker[i].color = chunkOff;
            }
        }

        if (currChunks == maxChunks)
        {
            armorChunkTracker[armorChunkTracker.Length - 1].color = chunkOn;
        }

        if (currChunks == 1)
        {
            armorChunkTracker[arrayMidpoint].color = chunkOn;
        }
    }

    public void UpdateColor()
    {
        chunkOn = hudColorController.getColorByString(hudElementName);
        chunkOff = new Color(chunkOn.r, chunkOn.g, chunkOn.b, 0.0f);

        updateChunks(currChunks);
    }
}
