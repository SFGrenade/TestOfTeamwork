using JetBrains.Annotations;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours;

[UsedImplicitly]
public class WeaverPrincessBoss : MonoBehaviour
{
    [UsedImplicitly]
    public string EncounterPdBoolName;
    [UsedImplicitly]
    public string DefeatBoolName;
    [UsedImplicitly]
    public GameObject AdditionalEffects;
    [UsedImplicitly]
    public string FightSnapShot = "Normal";
    [UsedImplicitly]
    public AudioClip[] FightAudioClips = new AudioClip[6];
    [UsedImplicitly]
    public GameObject BlockerGo;

    public void Start()
    {
    }

    private void Log(string message)
    {
    }

    private void Log(object message)
    {
    }
}