using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;




public class DPS_Listener : MonoBehaviour
{
    #region DllImports

    [DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
    private static extern void TransitionImmediate(int iScheme, int iVariation);

    [DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
    private static extern void TransitionManual(int iScheme, int iVariation);

    [DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
    private static extern void GetSchemeNames(out IntPtr stringBuffer, out int nSchemes);

    [DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
    private static extern void GetVariationNames(int iScheme, out IntPtr variationNames, out int nVariations);

    [DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
    private static extern void GetSchemeAndVariationNames(out IntPtr stringBuffer, out IntPtr schemeNamePositions, out int nSchemes);

    [DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
    private static extern void SetSliderValue(float s);

    #endregion


    public class Pair
    {
        public int iScheme;
        public int iVariation;

        public Pair()
        {
            iScheme = 0;
            iVariation = 0;
        }

        public Pair(int i, int j)
        {
            iScheme = i;
            iVariation = j;
        }
    }

    private enum TransitionType : int
    {
        IMMEDIATE,
        MANUAL
    }


    public enum CharacterState : int
    {
        WAITING_HIGH_HEALTH,
        WAITING_DROPPED_HEALTH,
        WAITING_LOW_HEALTH,
        MOVING_HIGH_HEALTH,
        MOVING_DROPPED_HEALTH,
        MOVING_LOW_HEALTH,
        MOVING_FAST_HIGH_HEALTH,
        MOVING_FAST_DROPPED_HEALTH,
        MOVING_FAST_LOW_HEALTH,
        DEAD
    }

    public CharacterState characterState;
    public float sliderValue = 0;
    private float lastJumpTime = -1f;
    private float jumpTimeCutoff = 1.0f;

    //public Dictionary<string, Dictionary<string, Pair>> schemeNameDict;
    private PlayerController playerController; // like player manager or source info 
                                               //private wsPlatformer2DUserControl userControl; // event systems? scripts attached to player
    private Rigidbody rigbody;


    void Awake()
    {
        this.playerController = GetComponent<PlayerController>();
        //this.userControl = GetComponent<wsPlatformer2DUserControl>();
        this.rigbody = GetComponent<Rigidbody>();
        //BuildSchemeDictionary();
    }


    void Start()
    {
    }


    void Update()
    {
        setCharacterState(this.playerController.moveSpeed);
    }


    //void BuildSchemeDictionary() {

    //       IntPtr unmanagedStringArray = IntPtr.Zero;
    //       IntPtr unmanagedIntArray = IntPtr.Zero;
    //       int nSchemes;
    //       GetSchemeAndVariationNames(out unmanagedStringArray, out unmanagedIntArray, out nSchemes);

    //       int[] schemeNamePositions = new int[nSchemes + 1];
    //       Marshal.Copy(unmanagedIntArray, schemeNamePositions, 0, nSchemes + 1);
    //       int nStrings = schemeNamePositions[nSchemes];
    //       IntPtr[] pIntPtrArray = new IntPtr[nStrings];
    //       string[] schemeAndVariationNames = new string[nStrings];

    //       Marshal.Copy(unmanagedStringArray, pIntPtrArray, 0, nStrings);

    //       for (int i = 0; i < nStrings; i++)
    //       {
    //           schemeAndVariationNames[i] = Marshal.PtrToStringAnsi(pIntPtrArray[i]);
    //           Marshal.FreeCoTaskMem(pIntPtrArray[i]);
    //       }
    //       Marshal.FreeCoTaskMem(unmanagedStringArray);

    //       schemeNameDict = new Dictionary<string, Dictionary<string, Pair>>();

    //       for (int i = 0; i < nSchemes; i++)
    //       {
    //           int position = schemeNamePositions[i];
    //           string schemeName = schemeAndVariationNames[position];
    //           Dictionary<string, Pair> varNameDict = new Dictionary<string, Pair>();
    //           int nVariations = schemeNamePositions[i + 1] - position - 1;
    //           for (int j = 0; j < nVariations; j++)
    //           {
    //               string varName = schemeAndVariationNames[position + 1 + j];
    //               Pair coords = new Pair(i, j);
    //               varNameDict.Add(varName, coords);
    //           }
    //           schemeNameDict.Add(schemeName, varNameDict);
    //       }

    //}


    //void Transition(TransitionType transType, string schemeName, string varName) { 

    //	Pair coords		= schemeNameDict[schemeName][varName];
    //	int	 iScheme	= coords.iScheme;
    //	int  iVariation = coords.iVariation;

    //	switch (transType)
    //	{
    //		case TransitionType.IMMEDIATE:
    //			TransitionImmediate(iScheme, iVariation);
    //			break;
    //		case TransitionType.MANUAL:
    //			TransitionManual(iScheme, iVariation);
    //			break;
    //	}
    //}


    public void setCharacterState(float moveSpeed)
    {
        ////////////////////////////////////////////////////
        // Important Character State Variables    

        float xSpeed = Math.Abs(this.rigbody.velocity.x);
        float yPos = this.rigbody.position.y;

        int maxHullIntegrity = playerController.getMaxHullIntegrity();
        int currHullIntegrity = playerController.getCurrHullIntegrity();

        bool highHealth = (currHullIntegrity == maxHullIntegrity);
        bool droppedHealth = (currHullIntegrity < (maxHullIntegrity));
        bool lowHealth = (currHullIntegrity < (maxHullIntegrity / 2)); // should read "if curr < 2"

        bool isWaiting = (playerController.diffX == 0 && playerController.diffY == 0); //playerController.hasStopped; //(xSpeed == 0);
        bool isMoving = xSpeed > 0 && xSpeed < 50;
        bool isMovingFast = xSpeed >= 50; // playerController.diffX > 1

        ////////////////////////////////////////////////////
        //Character State Definitions  - 10 states           

        bool isWaitingHighHealth = isWaiting && highHealth;
        bool isWaitingDroppedHealth = isWaiting && droppedHealth;
        bool isWaitingLowHealth = isWaiting && lowHealth;

        bool isMovingHighHealth = isMoving && highHealth;
        bool isMovingDroppedHealth = isMoving && droppedHealth;
        bool isMovingLowHealth = isMoving && lowHealth;

        bool isMovingFastHighHealth = isMovingFast && highHealth;
        bool isMovingFastDroppedHealth = isMovingFast && droppedHealth;
        bool isMovingFastLowHealth = isMovingFast && lowHealth;

        bool isDead = playerController.isDead();

        CharacterState characterState = this.characterState;

        ////////////////////////////////////////////////////
        //  State-Switching Mechanism 

        //print("Current state = " + characterState); // debug

        if (isDead)
        {
            characterState = CharacterState.DEAD;
        }
        else if (isWaitingHighHealth)
        {
            characterState = CharacterState.WAITING_HIGH_HEALTH;
        }
        else if (isWaitingDroppedHealth)
        {
            characterState = CharacterState.WAITING_DROPPED_HEALTH;
        }
        else if (isWaitingLowHealth)
        {
            characterState = CharacterState.WAITING_LOW_HEALTH;
        }
        else if (isMovingHighHealth)
        {
            characterState = CharacterState.MOVING_HIGH_HEALTH;
        }
        else if (isMovingDroppedHealth)
        {
            characterState = CharacterState.MOVING_DROPPED_HEALTH;
        }
        else if (isMovingLowHealth)
        {
            characterState = CharacterState.MOVING_LOW_HEALTH;
        }

        else if (isMovingFastHighHealth)
        {
            characterState = CharacterState.MOVING_FAST_HIGH_HEALTH;

            sliderValue = .5f;
            SetSliderValue(sliderValue);
        }
        else if (isMovingFastDroppedHealth)
        {
            characterState = CharacterState.MOVING_FAST_DROPPED_HEALTH;

            sliderValue = .5f;
            SetSliderValue(sliderValue);
        }
        else if (isMovingFastLowHealth)
        {
            characterState = CharacterState.MOVING_FAST_LOW_HEALTH;

            sliderValue = .5f;
            SetSliderValue(sliderValue);
        }
        else
        {
            this.characterState = CharacterState.WAITING_HIGH_HEALTH;
        }

        if (characterState != this.characterState)
        {
            this.characterState = characterState;
            setNewMusicState();
        }

    }

    IEnumerator SliderLerp(float totalLerpTime)
    {
        float currentLerpTime = 0f;
        while (currentLerpTime < totalLerpTime)
        {
            currentLerpTime += Time.deltaTime;
            float percent = currentLerpTime / totalLerpTime;
            sliderValue = Mathf.Lerp(0f, 1f, percent);
            SetSliderValue(sliderValue);
            yield return null;
        }
    }


    public void setNewMusicState()
    {
        switch (this.characterState)
        {
            case CharacterState.WAITING_HIGH_HEALTH:
                TransitionManual(1, 7);
                StartCoroutine(SliderLerp(4.0f));
                break;
            case CharacterState.WAITING_DROPPED_HEALTH:
                TransitionManual(1, 12);
                StartCoroutine(SliderLerp(4.0f));
                break;
            case CharacterState.WAITING_LOW_HEALTH:
                TransitionManual(1, 8);
                StartCoroutine(SliderLerp(4.0f));
                break;
            case CharacterState.MOVING_HIGH_HEALTH:
                TransitionManual(1, 4);
                StartCoroutine(SliderLerp(4.0f));
                break;
            case CharacterState.MOVING_DROPPED_HEALTH:
                TransitionManual(1, 13);
                StartCoroutine(SliderLerp(4.0f));
                break;
            case CharacterState.MOVING_LOW_HEALTH:
                TransitionManual(1, 6);
                StartCoroutine(SliderLerp(4.0f));
                break;
            case CharacterState.MOVING_FAST_HIGH_HEALTH:
                TransitionManual(1, 15);
                StartCoroutine(SliderLerp(3.0f));
                break;
            case CharacterState.MOVING_FAST_DROPPED_HEALTH:
                TransitionManual(1, 14);
                StartCoroutine(SliderLerp(3.0f));
                break;
            case CharacterState.MOVING_FAST_LOW_HEALTH:
                TransitionManual(1, 16);
                StartCoroutine(SliderLerp(3.0f));
                break;
            case CharacterState.DEAD:
                TransitionImmediate(1, 11);
                StartCoroutine(SliderLerp(4.0f));
                break;
            default:
                break;
        }
    }

}
