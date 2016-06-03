using UnityEngine;
using System.Collections;

public class HUDColorController : MonoBehaviour {

    private Color shieldColor;
    private Color armorColor;
    private Color bombColor;
    private Color defaultColor;

    private float shieldColorR;
    private float shieldColorG;
    private float shieldColorB;

    private float armorColorR;
    private float armorColorG;
    private float armorColorB;

    private float bombColorR;
    private float bombColorG;
    private float bombColorB;

	// Use this for initialization
	void Awake () {

        defaultColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);

        // if the colors have not been saved, use the default color
        // references the red channel for convenience
        if (PlayerPrefs.GetFloat("shieldColorR", -1) == -1)
        {
            shieldColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);
        }
        else
        {
            shieldColorR = PlayerPrefs.GetFloat("shieldColorR");
            shieldColorG = PlayerPrefs.GetFloat("shieldColorG");
            shieldColorB = PlayerPrefs.GetFloat("shieldColorB");
            shieldColor = new Color(shieldColorR, shieldColorG, shieldColorB, defaultColor.a);
        }

        if(PlayerPrefs.GetFloat("armorColorR", -1) == -1)
        {
            armorColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);
        }
        else
        {
            armorColorR = PlayerPrefs.GetFloat("armorColorR");
            armorColorG = PlayerPrefs.GetFloat("armorColorG");
            armorColorB = PlayerPrefs.GetFloat("armorColorB");
            armorColor = new Color(armorColorR, armorColorG, armorColorB, defaultColor.a);
        }

        if (PlayerPrefs.GetFloat("bombColorR", -1) == -1)
        {
            bombColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);

        }
        else
        {
            bombColorR = PlayerPrefs.GetFloat("bombColorR");
            bombColorG = PlayerPrefs.GetFloat("bombColorG");
            bombColorB = PlayerPrefs.GetFloat("bombColorB");
            bombColor = new Color(bombColorR, bombColorG, bombColorB, defaultColor.a);
        }

        /*shieldColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);
        armorColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);
        bombColor = new Color(0.729f, 0.894f, 0.937f, 0.9f);*/
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // must use one of the following
    // shield
    // armor
    // bomb
    // floats must be between 0 and 1.0
    public void setColorByString(string elementName, float red, float green, float blue)
    {
        if (elementName.Equals("shield"))
        {
            shieldColor = new Color(red, green, blue, defaultColor.a);
            PlayerPrefs.SetFloat("shieldColorR", red);
            PlayerPrefs.SetFloat("shieldColorG", green);
            PlayerPrefs.SetFloat("shieldColorB", blue);
            PlayerPrefs.Save();
        }
        else if (elementName.Equals("armor"))
        {
            armorColor = new Color(red, green, blue, defaultColor.a);
            PlayerPrefs.SetFloat("armorColorR", red);
            PlayerPrefs.SetFloat("armorColorG", green);
            PlayerPrefs.SetFloat("armorColorB", blue);
            PlayerPrefs.Save();
        }
        else if (elementName.Equals("bomb"))
        {
            bombColor = new Color(red, green, blue, defaultColor.a);
            PlayerPrefs.SetFloat("bombColorR", red);
            PlayerPrefs.SetFloat("bombColorG", green);
            PlayerPrefs.SetFloat("bombColorB", blue);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Invalid Element");
        }
    }

    /*public void setShieldColor(Color newColor)
    {
        shieldColor = newColor;
    }

    public void setArmorColor(Color newColor)
    {
        armorColor = newColor;
    }

    public void setBombColor(Color newColor)
    {
        bombColor = newColor;
    }*/

    // must use one of the following
    // shield
    // armor
    // powerup
    // bomb
    public void setColorToDefault(string elementName)
    {
        if(elementName.Equals("shield"))
        {
            shieldColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
            PlayerPrefs.SetFloat("shieldColorR", defaultColor.r);
            PlayerPrefs.SetFloat("shieldColorG", defaultColor.g);
            PlayerPrefs.SetFloat("shieldColorB", defaultColor.b);
            PlayerPrefs.Save();
        }
        else if(elementName.Equals("armor"))
        {
            armorColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
            PlayerPrefs.SetFloat("armorColorR", defaultColor.r);
            PlayerPrefs.SetFloat("armorColorG", defaultColor.g);
            PlayerPrefs.SetFloat("armorColorB", defaultColor.b);
            PlayerPrefs.Save();
        }
        else if (elementName.Equals("bomb"))
        {
            bombColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
            PlayerPrefs.SetFloat("bombColorR", defaultColor.r);
            PlayerPrefs.SetFloat("bombColorG", defaultColor.g);
            PlayerPrefs.SetFloat("bombColorB", defaultColor.b);
            PlayerPrefs.Save();
        }
    }

    public Color getColorByString(string getColor)
    {
        if (getColor.Equals("shield"))
        {
            return shieldColor;
        }
        else if (getColor.Equals("armor"))
        {
            return armorColor;
        }
        else if (getColor.Equals("bomb"))
        {
            return bombColor;
        }
        else
        {
            Debug.Log("Invalid Element");
            return Color.black;
        }
    }
}
