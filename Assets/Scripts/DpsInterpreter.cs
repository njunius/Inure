using UnityEngine;
using System.Collections;

public class DpsInterpreter : MonoBehaviour {

	enum MusicState : int
	{
		NotMovingHectic,
		NotMovingCalm,
		MovingHectic,
		MovingCalm,
		Cooridors,
        Charging,
        FinalCooridor
	}

	private DPSMono	DPS;
	public string	startState	= "";
	public string	endState	= "";
	public float	sliderValue = 0f;

	void Start()
	{
		this.sliderValue	= 0f;
		this.startState		= "NotMovingCalm";
		this.endState		= "";
		this.DPS			= GetComponent<DPSMono>();

		this.DPS.Initialize("Inure_Final/Not_moving_(calm)");
		this.DPS.Play();
	}

	public void setSliderValue(float sVal)
	{
		sliderValue = sVal;
		this.DPS.SetSliderValue(sliderValue);
	}

	public void setState(string state) 
	{
		if (state != this.endState) {
			if (state == "NotMovingHectic") {
				this.startState = this.endState;
				this.endState = state;
				this.DPS.TransitionManual("Inure_Final/Not_Moving_(hectic)");
			}
			else if (state == "NotMovingCalm") {
				this.startState = state;
				this.endState = state;
				this.sliderValue = 0;
				this.DPS.TransitionImmediate("Inure_Final/Not_moving_(calm)");
			}
			else if (state == "MovingHectic") {
				this.startState = this.endState;
				this.endState = state;
				this.sliderValue = 0;
				this.DPS.TransitionManual("Inure_Final/Moving_(hectic)");
				StartCoroutine(sliderLerp(3.0f));
			}
			else if (state == "Cooridors") {
				this.startState	= this.endState;
				this.endState	= state;
				//this.sliderValue = 0;
				this.DPS.TransitionManual("Inure_Final/Moving_(not_hectic)");
				StartCoroutine(sliderLerp(4.0f));
			}
			else if (state == "Charging") {
				this.startState = this.endState;
				this.endState = state;
				this.sliderValue = 0;
				this.DPS.TransitionManual("Inure_Final/Not_moving_(calm)");
				StartCoroutine(sliderLerp(4.0f));
			}
		}
	}

	IEnumerator sliderLerp(float totalLerpTime)
	{
		float currentLerpTime = 0f;
		//increment timer once per frame
		while (currentLerpTime < totalLerpTime)
		{
			currentLerpTime += Time.deltaTime;
			float percent = currentLerpTime / totalLerpTime;
			sliderValue = Mathf.Lerp(0f, 1f, percent);
			this.DPS.SetSliderValue(sliderValue);
			yield return null;
		}
	}

}
