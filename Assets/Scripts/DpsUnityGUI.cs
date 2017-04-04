using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.InteropServices;
using MathHelpers;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Linq;


public class DpsUnityGUI : IAudioEffectPluginGUI
{

	public enum enTransitionType { NONE, IMMEDIATE, MANUAL};
    private enum enPlayStatus { STOPPED, PLAYING, PAUSED };

    public	TextAsset	dpsFile;
	private GCHandle	dpsDataHandle;
	private IntPtr		dpsData;
	public	string[]	schemeAndVariationNames;
	private int			nSchemes;
	private int[]		schemeNamePositions;
	public	int			schemeSelection;
	public	int[]		schemeSelections;
	private int[][]		schemeLookupTable;
	public	int			currentSchemeNumber;
	public	int			currentVariationNumber;
	private bool		manualTransition;
	public enTransitionType transitionType  = enTransitionType.NONE;
	public	float		sliderValue			= 0;
    private enPlayStatus playStatus         = enPlayStatus.STOPPED;
    private string playButtonText = "PLAY";

	[SerializeField]
	public	string		dpsFileName			= "";

	[SerializeField]	
	//public string audioClipName = "";
	//public AudioSource audioSource = null;


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
	private static extern void PauseDPS(bool doPause);

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

	#endregion

	void MarshalSchemeNameArray(IntPtr unmanagedStringArray, int nStrings, out string[] managedStringArray) {

		IntPtr[] pIntPtrArray	= new IntPtr[nStrings];
		managedStringArray		= new string[nStrings];

		Marshal.Copy(unmanagedStringArray, pIntPtrArray, 0, nStrings);

		for (int i = 0; i < nStrings; i++) {
			managedStringArray[i] = Marshal.PtrToStringAnsi(pIntPtrArray[i]);
			Marshal.FreeCoTaskMem(pIntPtrArray[i]);
		}
		Marshal.FreeCoTaskMem(unmanagedStringArray);
	}


	public override bool OnGUI(IAudioEffectPlugin plugin)
	{
		#region DPS FILE LOADING / UNLOADING

		Event e				= Event.current;
		Rect dpsDropArea	= GUILayoutUtility.GetRect(0.0f, 25.0f, GUILayout.ExpandWidth(true));
        //this.dpsFileName = "";// GetDpsFileName();
		GUI.Box(dpsDropArea, (this.dpsFileName != "") ? this.dpsFileName : "dps file");


		switch (e.type) {
		case EventType.DragUpdated:
		case EventType.DragPerform:
			if (dpsDropArea.Contains(e.mousePosition)) 
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				if (e.type == EventType.DragPerform) 
				{
					DragAndDrop.AcceptDrag();
					string draggedFileName = Path.GetFileNameWithoutExtension(DragAndDrop.paths[0]);
					if (draggedFileName != this.dpsFileName) 
					{
						if (DpsFileIsLoaded()) 
						{
                            UnloadDpsFile();
							this.dpsDataHandle.Free();
							Resources.UnloadAsset(this.dpsFile);
							this.dpsFile = null;
						}
						this.dpsFile = Resources.Load(draggedFileName, typeof(TextAsset)) as TextAsset;
						this.dpsDataHandle = GCHandle.Alloc(this.dpsFile.bytes, GCHandleType.Pinned);
						this.dpsData = this.dpsDataHandle.AddrOfPinnedObject();
						LoadDpsFile(draggedFileName, this.dpsData);
					}
				}
			}
			break;
		}

		#endregion

		#region AUDIO SOURCE LOAD

		//Rect audioSourceDropArea = GUILayoutUtility.GetRect(0.0f, 25.0f, GUILayout.ExpandWidth(true));

		//// refind reference to audio source if necessary (only necessary if this script is not in a dll)
		//this.audioClipName = GetAudioClipName();
		//if ((this.audioSource == null) && (this.audioClipName != ""))
		//{
		//	AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>(); //(this.audioSourceName));
		//	foreach (AudioSource source in audioSources) {
		//		if (source.clip.name == this.audioClipName) {
		//			this.audioSource = source;
		//			break;
		//		}
		//	}
		//}

		//GUI.Box(audioSourceDropArea, (this.audioClipName != "") ? this.audioClipName : "audio source");

		//switch (e.type)
		//{
		//case EventType.DragUpdated:
		//case EventType.DragPerform:
		//	if (audioSourceDropArea.Contains(e.mousePosition)) {

		//		GameObject	draggedObjectReference	= (GameObject) DragAndDrop.objectReferences[0];
		//		AudioSource audioSource				= draggedObjectReference.GetComponent<AudioSource>();

		//		if ((audioSource != null) && (audioSource.clip != null)) {

		//			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

		//			if (e.type == EventType.DragPerform) {
		//				DragAndDrop.AcceptDrag();
		//				this.audioSource = audioSource;
		//				if (audioSource.clip.name != this.audioClipName) {
		//					SetAudioClipName(audioSource.clip.name);
		//				}
		//				//this.audioSource.
		//			}
		//		}
		//	}
		//	break;
		//}

		#endregion

		GUILayout.Space(20f);

		if (DpsFileIsLoaded())
		{
			#region SCHEME MENU

			//for (int i = 0; i < nSchemes; i++)
			//{
			//	schemeSelections[i] = -1;
			//}
			// set GUI scheme menu
			IntPtr unmanagedStringArray = IntPtr.Zero;
			IntPtr unmanagedIntArray = IntPtr.Zero;

			GetSchemeAndVariationNames(out unmanagedStringArray, out unmanagedIntArray, out nSchemes);

			schemeNamePositions = new int[nSchemes + 1];
			Marshal.Copy(unmanagedIntArray, schemeNamePositions, 0, nSchemes + 1);
			int nStrings = schemeNamePositions[nSchemes];
			IntPtr[] pIntPtrArray = new IntPtr[nStrings];
			schemeAndVariationNames = new string[nStrings];

			Marshal.Copy(unmanagedStringArray, pIntPtrArray, 0, nStrings);

			for (int i = 0; i < nStrings; i++)
			{
				schemeAndVariationNames[i] = Marshal.PtrToStringAnsi(pIntPtrArray[i]);
				Marshal.FreeCoTaskMem(pIntPtrArray[i]);
			}
			Marshal.FreeCoTaskMem(unmanagedStringArray);
			schemeSelections = new int[nSchemes];

			currentSchemeNumber = GetCurrentScheme();
			currentVariationNumber = GetCurrentVariation();

			for (int i = 0; i < nSchemes; i++)
			{
				schemeSelections[i] = (i == currentSchemeNumber) ? currentVariationNumber : -1;
			}

			for (int i = 0; i < nSchemes; i++)
			{
				GUILayout.BeginHorizontal();
				int position = schemeNamePositions[i];
				GUILayout.Label(schemeAndVariationNames[position]);
				int nVariations = schemeNamePositions[i + 1] - position - 1;
				string[] variationNames = new string[nVariations];
				Array.Copy(schemeAndVariationNames, position + 1, variationNames, 0, nVariations);
				schemeSelections[i] = GUILayout.SelectionGrid(schemeSelections[i], variationNames, 1);
				GUILayout.EndHorizontal();
				GUILayout.Space(10f);
			}

			manualTransition = GUILayout.Toggle(manualTransition, "Manual");

			int iScheme = -1;
			int iVariation = -1;

			for (int i = 0; i < nSchemes; i++)
			{
				if (schemeSelections[i] != -1)
				{
					iScheme = i;
					iVariation = schemeSelections[i];

					if ((iScheme != currentSchemeNumber) || (iVariation != currentVariationNumber))
					{
						if (manualTransition)
						{
							TransitionManual(iScheme, iVariation);
							transitionType = enTransitionType.MANUAL;
						}
						else
						{
							TransitionImmediate(iScheme, iVariation);
							transitionType = enTransitionType.IMMEDIATE;
						}


						break;
					}
				}
			}

			//currentSchemeNumber = iScheme;
			//currentVariationNumber = iVariation;

			//for (int i = 0; i < nSchemes; i++)
			//{
			//	schemeSelections[i] = (i == currentSchemeNumber) ? currentVariationNumber : -1;
			//}

			#endregion

			#region TRANSITIONS

			if (manualTransition)
			{
				sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.0f, 1.0f);
				SetSliderValue(sliderValue);
			}

            #endregion

            #region PLAYBACK

            if (GUILayout.Button(this.playButtonText))
                switch (this.playStatus) {
                    case enPlayStatus.STOPPED:
                        if ((currentSchemeNumber != -1) && (currentVariationNumber != -1))
                        {
                            InitializeDPS(currentSchemeNumber, currentVariationNumber);
                            StartDPS();
                        }
                        this.playStatus = enPlayStatus.PLAYING;
                        this.playButtonText = "PAUSE"; 
                        break;
                    case enPlayStatus.PLAYING:
                        PauseDPS(true);
                        this.playStatus = enPlayStatus.PAUSED;
                        this.playButtonText = "PLAY";
                        break;
                    case enPlayStatus.PAUSED:
                        PauseDPS(false);
                        this.playStatus = enPlayStatus.PLAYING;
                        this.playButtonText = "PAUSE";
                        break;
                }

            {

			}
			if (GUILayout.Button("STOP"))
			{
				StopDPS();
                this.playStatus = enPlayStatus.STOPPED;
                this.playButtonText = "PLAY";
            }

			#endregion
		}
	
		return true;
	}


	#region Public Overrides

	public override string Name
	{
		get { return "DynamicPercussionSystem"; }
	}

	public override string Description
	{
		get { return "dps"; }
	}

	public override string Vendor
	{
		get { return "IMS"; }
	}

	#endregion

}


