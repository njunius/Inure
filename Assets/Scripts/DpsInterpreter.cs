using UnityEngine;
using System.Collections;

public class DpsInterpreter : MonoBehaviour {

	enum MusicState : int
	{
		Waiting,
		Moving,
		Jumping,
		MovingFast,
		MovingFastJumping
	}

	private DPSMono	DPS;
	public string	startState	= "";
	public string	endState	= "";
	public float	sliderValue = 0f;

	void Start()
	{
		this.sliderValue	= 0f;
		this.startState		= "Much_Calmer";
		this.endState		= "";
		this.DPS			= GetComponent<DPSMono>();

		this.DPS.Initialize("Inure_Export");
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
			if (state == "Falling To Death") {
				this.startState = this.endState;
				this.endState = state;
				this.DPS.TransitionManual("COMBAT/BIG_FINISH");
			}
			else if (state == "Jumping") {
				this.startState = state;
				this.endState = state;
				this.sliderValue = 0;
				this.DPS.TransitionImmediate("COMBAT/ALL_MAX");
			}
			else if (state == "Moving Fast") {
				this.startState = this.endState;
				this.endState = state;
				this.sliderValue = 0;
				this.DPS.TransitionManual("COMBAT/RUNNING--HIGH_DRUMS");
				StartCoroutine(sliderLerp(3.0f));
			}
			else if (state == "Moving") {
				this.startState	= this.endState;
				this.endState	= state;
				this.sliderValue = 0;
				this.DPS.TransitionManual("COMBAT/JUST_SHAKERS_AND_STICKS");
				StartCoroutine(sliderLerp(4.0f));
			}
			else if (state == "Waiting") {
				this.startState = this.endState;
				this.endState = state;
				this.sliderValue = 0;
				this.DPS.TransitionManual("COMBAT/THINKING");
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
