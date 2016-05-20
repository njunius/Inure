using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using FMOD.Studio;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Linq;

public class DPSMono : MonoBehaviour {

	public enum DPS_STOPTYPE
	{
		END_OF_THIS_BEAT,			// stop at end of beat (i.e. immediately)
		BEGINNING_OF_NEXT_BEAT,		// stop on downbeat of next beat
		END_OF_THIS_MEASURE,			// stop at end of measure
		BEGINNING_OF_NEXT_MEASURE,	// stop on downbeat of next measure
	};



	public string		startSchemeName;
	public string		endSchemeName;
	public int			startScheme;
	public int			startVariation;
	public int			sampleRate;
	public string		dpsFileName;
	public float		sliderValue;
	public List<string>	schemes;
	//public enum		enSchemeNames { };  // this would probably be better than using a List, in order to prevent passing misspelled scheme names into the DPS Mono.

	private IntPtr						system;
	private TextAsset					dpsFile;
	private byte[]						dpsData;
	private GCHandle					dpsDataHandle;
	private IntPtr						pDpsData;

	private int							nChannels;
	private int							nSchemes;
	private Dictionary<string, int[]>	schemeNameDict;

	#region PInvokes

	[DllImport("Assets/Plugins/DPS/Assets/Plugins/DPS/DPS.dll", EntryPoint = "DPS_TestFcn")]
	private static extern int DPS_TestFcn(int x, int y);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_CreateSystem(out IntPtr system, int nChannels, float samplesPerSecond);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_DestroySystem(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_LoadPackage(IntPtr system, IntPtr pkgMem);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_UnloadPackage(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern int DPS_GetChannels(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern bool DPS_OkToUpdate(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern int DPS_GetSampleCount(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetChannels(IntPtr system, int nChannels); // 1 = mono, 2 = stereo

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetDefaultStopType(IntPtr system, DPS_STOPTYPE stoptype);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetSampleRate(IntPtr system, float samplerate);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetTempo(IntPtr system, float tempo);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetSyncOffsetTime(IntPtr system, float fPlayTime, float fPlayLength);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetSliderValue(IntPtr system, float value);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_SetDebugLevel(IntPtr system, int value);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_Initialize(IntPtr system, int iPlayer, int iScheme);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_Start(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_Stop(IntPtr system);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_StopWithType(IntPtr system, DPS_STOPTYPE n);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_Pause(IntPtr system, Boolean state);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_TransitionImmediate(IntPtr system, int iPlayer, int iScheme);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_TransitionManual(IntPtr system, int iPlayer, int iScheme);

	[DllImport("Assets/Plugins/DPS/DPS.dll")]
	private static extern void DPS_Update(IntPtr system, [Out] float[] buffer, int length);

	#endregion PInvokes

	#region Wrapper Methods

	public int TestFcn(int x, int y)
	{
		return DPS_TestFcn(x, y);
	}

	public void CreateSystem(int nChannels, float sampleRate)
	{
		DPS_CreateSystem(out this.system, nChannels, sampleRate);
	}

	public void LoadPackage(IntPtr pkgMem)
	{
		DPS_LoadPackage(this.system, pkgMem);
	}

	public void UnloadPackage()
	{
		DPS_UnloadPackage(this.system);
	}

	public void GetChannels()
	{
		DPS_GetChannels(this.system);
	}

	public void OkToUpdate()
	{
		DPS_OkToUpdate(this.system);
	}

	public void GetSampleCount()
	{
		DPS_GetSampleCount(this.system);
	}

	public void SetChannels(int nChannels)
	{
		DPS_SetChannels(this.system, nChannels);
	}

	public void SetDefaultStopType(DPS_STOPTYPE stopType)
	{
		DPS_SetDefaultStopType(this.system, stopType);
	}

	public void SetSampleRate(float sampleRate)
	{
		DPS_SetSampleRate(this.system, sampleRate);
	}

	public void SetTempo(float tempo)
	{
		DPS_SetTempo(this.system, tempo);
	}

	public void SetSyncOffsetTime(float fPlayTime, float fPlayLength)
	{
		DPS_SetSyncOffsetTime(this.system, fPlayTime, fPlayLength);
	}

	public void SetSliderValue(float value)
	{
		this.sliderValue = value;
		DPS_SetSliderValue(this.system, value);
	}

	public void SetDebugLevel(int value)
	{
		DPS_SetDebugLevel(this.system, value);
	}

	public void Initialize(int iScheme, int iVariation)
	{
		DPS_Initialize(this.system, iScheme, iVariation);
	}

	public void Play()
	{
		DPS_Start(this.system);
	}

	public void Stop()
	{
		DPS_Stop(this.system);
	}

	public void StopWithType(DPS_STOPTYPE stopType)
	{
		DPS_StopWithType(this.system, stopType);
	}

	public void Pause(Boolean state)
	{
		DPS_Pause(this.system, state);
	}

	public void TransitionImmediate(int iPlayer, int iScheme)
	{
		DPS_TransitionImmediate(this.system, iPlayer, iScheme);
	}

	public void TransitionManual(int iPlayer, int iScheme)
	{
		DPS_TransitionManual(this.system, iPlayer, iScheme);
	}

	//public void Update(float[] buffer, int length)
	//{
	//	DPS_Update(this.system, buffer, length);
	//}

	#endregion Wrapper methods


	#region public methods

	public void Initialize(string firstSchemeName) {
		sliderValue = 0;
		int iScheme;
		int iVariation;
		SchemeNameToNumbers(firstSchemeName, out iScheme, out iVariation);
		Initialize(iScheme, iVariation);
		startSchemeName = firstSchemeName;
	}

	public void TransitionImmediate(string schemeName)
	{
		sliderValue = 0;
		int iScheme;
		int iVariation;
		SchemeNameToNumbers(schemeName, out iScheme, out iVariation);
		TransitionImmediate(iScheme, iVariation);
		startSchemeName = schemeName;
		endSchemeName = schemeName;
	}

	public void TransitionManual(string schemeName)
	{
		sliderValue = 0;
		int iScheme;
		int iVariation;
		SchemeNameToNumbers(schemeName, out iScheme, out iVariation);
		TransitionManual(iScheme, iVariation);
		startSchemeName = endSchemeName;
		endSchemeName = schemeName;
	}

	#endregion

	void Awake()
	{
		this.nSchemes					= 0;
		this.sampleRate					= 44100;
		AudioConfiguration audioConfig	= AudioSettings.GetConfiguration();
		audioConfig.sampleRate			= this.sampleRate;
		audioConfig.speakerMode			= AudioSpeakerMode.Mono;
		this.nChannels					= (int) audioConfig.speakerMode;

		AudioSettings.Reset(audioConfig);
		AudioConfiguration audioConfig2 = AudioSettings.GetConfiguration();
		CreateSystem(this.nChannels, (float) this.sampleRate);
		LoadDpsFile();
		LoadDpsInfoFile();
	}

	void SchemeNameToNumbers(string schemeName, out int iScheme, out int iVariation) {
		int[] numbers	= schemeNameDict[schemeName];
		iScheme			= numbers[0];
		iVariation		= numbers[1];	
	}

	void OnDestroy() {
		if (this.system != IntPtr.Zero) {
			UnloadDpsFile();
			DPS_DestroySystem(this.system);
			this.system = IntPtr.Zero;
		}
	}

	//protected virtual void Dispose(bool bDisposing)
	//{
	//	if (this.this.system != IntPtr.Zero)
	//	{
	//		// DPS_ the DLL Export to dispose this class
	//		DPS_DestroySystem(this.this.system);
	//		this.this.system = IntPtr.Zero;
	//	}

	//	if (bDisposing)
	//	{
	//		// No need to call the finalizer since we've now cleaned
	//		// up the unmanaged memory
	//		GC.SuppressFinalize(this);
	//	}
	//}

	// Use this for initialization
	//void Start () {
	//}
	

	// Update is called once per frame



	void OnAudioFilterRead(float[] data, int channels)
	{
		DPS_Update(this.system, data, data.Length); // * channels);
	}

	void LoadDpsFile() {
        Debug.Log(dpsFileName);
		this.dpsFile		= Resources.Load<TextAsset>(dpsFileName);
        Debug.Log(this.dpsFile);
		this.dpsData		= this.dpsFile.bytes;
		this.dpsDataHandle	= GCHandle.Alloc(this.dpsData, GCHandleType.Pinned);
		this.pDpsData		= this.dpsDataHandle.AddrOfPinnedObject();
		LoadPackage(this.pDpsData);
	}

	void UnloadDpsFile() {
		UnloadPackage();
		this.dpsDataHandle.Free();
		this.dpsData = null;
		Resources.UnloadAsset(this.dpsFile);	
	}

	void LoadDpsInfoFile()
	{
		schemes								= new List<string>();
		schemeNameDict						= new Dictionary<string, int[]>();	
		string	 dpsInfoFileName			= "Assets/Resources/" + dpsFileName + "_info.xml";
		XElement dpsDocRoot					= XElement.Load(dpsInfoFileName);
		XElement schemeDatabaseRoot			= (XElement)dpsDocRoot.FirstNode;
		IEnumerable<XElement> schemeNodes	= from schemeNode in schemeDatabaseRoot.Elements() 
											  select schemeNode;
		foreach (XElement schemeNode in schemeNodes) {
			int nVariations = 0;
			string schemeName = schemeNode.Attribute("Name").Value.ToString();
			IEnumerable<XElement> variationNodes = from variationNode in schemeNode.Elements() 
												   select variationNode;
			foreach (XElement variationNode in variationNodes) {
				string variationName	= variationNode.Attribute("Name").Value.ToString();
				string fullName			= schemeName + "/" + variationName;
				schemes.Add(fullName);				
				schemeNameDict.Add(fullName, new int[] { nSchemes, nVariations });
				nVariations++;
			}
			nSchemes++;
		}
		//enSchemes = schemes.Select(a => (enSchemeNames)Enum.Parse(typeof(enSchemeNames), a));
	}
}
