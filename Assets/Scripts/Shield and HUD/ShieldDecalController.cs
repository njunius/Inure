using UnityEngine;
using System.Collections;

public class ShieldDecalController : MonoBehaviour {

    private Material thisMaterial;

	// Use this for initialization
	void Awake () {
        thisMaterial = gameObject.GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        thisMaterial.SetColor("_EmisColor", 
            new Color(thisMaterial.GetColor("_EmisColor").r, thisMaterial.GetColor("_EmisColor").g, 
            thisMaterial.GetColor("_EmisColor").b, thisMaterial.GetColor("_EmisColor").a - Time.deltaTime));
        if(thisMaterial.GetColor("_EmisColor").a < 0.0f)
        {
            Destroy(gameObject);
        }
	}

    public void setColor(Color bulletColor)
    {
        thisMaterial.SetColor("_EmisColor", new Color(bulletColor.r, bulletColor.g, bulletColor.b, bulletColor.a));
    }
}
