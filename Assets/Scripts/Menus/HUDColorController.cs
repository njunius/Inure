using UnityEngine;
using System.Collections;

public class HUDColorController : MonoBehaviour {

    private Color shieldColor;
    private Color armorColor;
    private Color powerupColor;
    private Color bombColor;
    private Color defaultColor;

	// Use this for initialization
	void Awake () {
        shieldColor = new Color(0.729f, 0.894f, 0.937f, 0.784f);
        defaultColor = new Color(0.729f, 0.894f, 0.937f, 0.784f);
        armorColor = new Color(0.729f, 0.894f, 0.937f, 0.784f);
        powerupColor = new Color(0.729f, 0.894f, 0.937f, 0.784f);
        bombColor = new Color(0.729f, 0.894f, 0.937f, 0.784f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // must use one of the following
    // shield
    // armor
    // powerup
    // bomb
    // floats must be between 0 and 1.0
    public void setColorByString(string elementName, float red, float green, float blue)
    {
        if (elementName.Equals("shield"))
        {
            shieldColor = new Color(red, green, blue, defaultColor.a);
        }
        else if (elementName.Equals("armor"))
        {
            armorColor = new Color(red, green, blue, defaultColor.a);
        }
        else if (elementName.Equals("powerup"))
        {
            powerupColor = new Color(red, green, blue, defaultColor.a);
        }
        else if (elementName.Equals("bomb"))
        {
            bombColor = new Color(red, green, blue, defaultColor.a);
        }
    }

    public void setShieldColor(Color newColor)
    {
        shieldColor = newColor;
    }

    public void setArmorColor(Color newColor)
    {
        armorColor = newColor;
    }

    public void setPowerupColor(Color newColor)
    {
        powerupColor = newColor;
    }

    public void setBombColor(Color newColor)
    {
        bombColor = newColor;
    }

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
        }
        else if(elementName.Equals("armor"))
        {
            armorColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
        }
        else if(elementName.Equals("powerup"))
        {
            powerupColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
        }
        else if (elementName.Equals("bomb"))
        {
            bombColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
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
        else if (getColor.Equals("powerup"))
        {
            return powerupColor;
        }
        else if (getColor.Equals("bomb"))
        {
            return bombColor;
        }
        else
        {
            return Color.black;
        }
    }
}
