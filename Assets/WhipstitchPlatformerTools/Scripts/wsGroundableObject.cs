using UnityEngine;
using System.Collections;

public abstract class wsGroundableObject : MonoBehaviour {

	abstract public Vector3 getVelocity();
	abstract public void wsOnGroundEnter();
	abstract public void wsOnGroundExit();

}//	End abstract class wsGroundableObject
