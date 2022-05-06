using UnityEngine;
//using UnityEngine.Audio;

public class TransitionPoint : MonoBehaviour
{
	public bool isADoor = false;
	[HideInInspector]
	public bool dontWalkOutOfDoor = false;
	[HideInInspector]
	public float entryDelay = 0.0f;
	public bool alwaysEnterRight = false;
	public bool alwaysEnterLeft = false;
	public bool hardLandOnExit = false;
	public string targetScene;
	public string entryPoint;
	[HideInInspector]
	public Vector2 entryOffset = new Vector2(0.0f, 0.0f);
	[HideInInspector]
	public PlayMakerFSM customFadeFSM = null;
	[HideInInspector]
	public bool nonHazardGate = false;
	public HazardRespawnMarker respawnMarker;
	[HideInInspector]
	public AudioMixerSnapshot atmosSnapshot = null;
	[HideInInspector]
	public AudioMixerSnapshot enviroSnapshot = null;
	[HideInInspector]
	public AudioMixerSnapshot actorSnapshot = null;
	[HideInInspector]
	public AudioMixerSnapshot musicSnapshot = null;
	public GameManager.SceneLoadVisualizations sceneLoadVisualization = GameManager.SceneLoadVisualizations.Default;
	[HideInInspector]
	public bool customFade = false;
	[HideInInspector]
	public bool forceWaitFetch = false;
}
