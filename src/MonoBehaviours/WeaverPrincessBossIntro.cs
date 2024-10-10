using JetBrains.Annotations;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours;

[UsedImplicitly]
public class WeaverPrincessBossIntro : MonoBehaviour
{
    [UsedImplicitly]
    public string EncounterPdBoolName = "SFGrenadeTestOfTeamworkEncounterBeforeBoss";
    [UsedImplicitly]
    public string Dialogue1ConvoSheet = "Hornet";
    [UsedImplicitly]
    public string Dialogue1ConvoKey = "";
    [UsedImplicitly]
    public string Dialogue2ConvoSheet = "Hornet";
    [UsedImplicitly]
    public string Dialogue2ConvoKey = "";
    [UsedImplicitly]
    public string FightSnapShot = "Normal";
    [UsedImplicitly]
    public AudioClip[] FightAudioClips = new AudioClip[6];
    [UsedImplicitly]
    public GameObject BossGo;

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