using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletCountDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<UnityEngine.UI.Text> ().text = GameObject.FindGameObjectWithTag ("Object Pooler").GetComponent<ObjectPooler> ().numActiveObj.ToString ();
	}
}
