using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Runtime.InteropServices;

[System.Serializable]
public class DPS_Mono : MonoBehaviour {

	public TextAsset dpsFile;
	[SerializeField]
	private GCHandle dpsDataHandle;
	[SerializeField]
	private IntPtr dpsData;

	public int startScheme = 0;
	public int startVariation = 0;
	public int currentScheme = 0;
	public int currentVariation = 0;

    public enum enTransitionType { NONE, IMMEDIATE, MANUAL };
    public enTransitionType transitionType;
	public float sliderValue;

	private AudioSource audioSource;
	//private IEnumerator syncCoroutine;
	//private float syncTime;
	//private bool readyToSync;

	# region DllImports

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void LoadDpsFile(string dpsFileName, IntPtr dpsDataBuffer);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void UnloadDpsFile();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void StartDPS();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void StopDPS();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void PauseDPS();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void InitializeDPS(int iScheme, int iVariation);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void TransitionImmediate(int iScheme, int iVariation);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void TransitionManual(int iScheme, int iVariation);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll", CharSet = CharSet.Ansi)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string GetDpsFileName();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void SetAudioClipName(string audioClipName);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll", CharSet = CharSet.Ansi)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	private static extern string GetAudioClipName();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void GetSchemeAndVariationNames(out IntPtr stringBuffer, out IntPtr schemeNamePositions, out int nSchemes);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern int GetCurrentScheme();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern int GetCurrentVariation();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern bool DpsFileIsLoaded();

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void SetSliderValue(float s);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void SetSyncOffsetTime(float currentTime, float clipLength);

	[DllImport("Assets/Plugins/AudioPluginDynamicPercussion.dll")]
	private static extern void SetSyncOffsetSamples(int currentSample, int clipLength);
    #endregion


    //void LoadAndInitializeDPS() {
    //    this.dpsDataHandle = GCHandle.Alloc(this.dpsFile.bytes, GCHandleType.Pinned);
    //    this.dpsData = this.dpsDataHandle.AddrOfPinnedObject();
    //    LoadPackage(this.dpsFile.name, this.dpsData);
    //    InitializeDPS(startScheme, startVariation);
    //    this.dpsFile.name = GetDpsFileName();
    //}


	//void OnValidate() {

	//	// This function is called when the script is loaded or a 
	//	// value is changed in the inspector (Called in the editor only!) (And yet it's getting called when the scene is building. Why?)

	//	bool userRemovedDpsFile		= (!this.dpsFile && DpsFileIsLoaded());
	//	bool userAddedDpsFile		= (this.dpsFile && !DpsFileIsLoaded());
	//	bool userSwitchedDpsFile	= (this.dpsFile && DpsFileIsLoaded() && (this.dpsFile.name != GetDpsFileName()));

	//	if (userRemovedDpsFile || userSwitchedDpsFile) {
 //           UnloadDpsFile();
	//		this.dpsDataHandle.Free();
	//		this.dpsData = IntPtr.Zero;

	//		if (userSwitchedDpsFile) {
	//			Resources.UnloadAsset(this.dpsFile);
	//			this.dpsFile = null;
	//		}
	//	}
		
	//	if (userAddedDpsFile || userSwitchedDpsFile) {
 //           this.dpsDataHandle = GCHandle.Alloc(this.dpsFile.bytes, GCHandleType.Pinned);
 //           this.dpsData = this.dpsDataHandle.AddrOfPinnedObject();
 //           LoadDpsFile(this.dpsFile.name, this.dpsData);
 //           //LoadAndInitializeDPS();
 //       }
	//}


	void Awake() {
		this.transitionType = enTransitionType.NONE;
		this.sliderValue = 0;
		//this.syncTime = 2.0f;
		//this.syncCoroutine = SyncWithAudioClip(this.syncTime);
		//this.readyToSync = false;

		this.audioSource = gameObject.GetComponent<AudioSource>();
        if (DpsFileIsLoaded()) {
            UnloadDpsFile();
        }
        // --------------- NEXT LINE IS THE CRASH: ---------------------------------//
        this.dpsDataHandle = GCHandle.Alloc(this.dpsFile.bytes, GCHandleType.Pinned);
        this.dpsData = this.dpsDataHandle.AddrOfPinnedObject();
        LoadDpsFile(this.dpsFile.name, this.dpsData);
        if (this.audioSource.playOnAwake) {
            InitializeDPS(startScheme, startVariation);
            StartDPS();
        }

	}

	void Start () {
	}
	
	void Update () {
	}

	void OnApplicationQuit() {
		StopDPS();
	}
}
