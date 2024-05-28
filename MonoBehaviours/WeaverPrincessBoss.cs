using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine.Audio;
using SFCore.MonoBehaviours;
using System.Linq;
using TestOfTeamwork.Consts;

namespace TestOfTeamwork.MonoBehaviours;

public class WeaverPrincessBoss : MonoBehaviour
{
    private static AudioMixer _musicAm = null;

    public string EncounterPdBoolName = "SFGrenadeTestOfTeamworkEncounteredWeaverPrincess";
    public string DefeatBoolName = "SFGrenadeTestOfTeamworkDefeatedWeaverPrincess";
    public GameObject AdditionalEffects;
    public string FightSnapShot = "Normal";
    public AudioClip[] FightAudioClips = new AudioClip[6];
    public GameObject BlockerGo;

    private MusicCue fightingMusicCue;
    private static Color32 startColor = new Color32(208, 221, 221, 224); // D0DDDD
    private static Color32 endColor = new Color32(208, 160, 160, 224); // D0A0A0

    public void Start()
    {
        if (_musicAm == null)
        {
            var ams = Resources.FindObjectsOfTypeAll<AudioMixer>();
            _musicAm = ams.First(x => x.name == "Music");
        }
        fightingMusicCue = SFCore.MonoBehaviours.CueHolder.GetMusicCue("SFGrenadeTestOfTeamworkHornetBoss", _musicAm.FindSnapshot(FightSnapShot), FightAudioClips, new SceneManagerPatcher.MusicChannelSync[]
        {
            SceneManagerPatcher.MusicChannelSync.Implicit,
            SceneManagerPatcher.MusicChannelSync.Implicit,
            SceneManagerPatcher.MusicChannelSync.Implicit,
            SceneManagerPatcher.MusicChannelSync.Implicit,
            SceneManagerPatcher.MusicChannelSync.Implicit,
            SceneManagerPatcher.MusicChannelSync.Implicit
        });

        GameObject hornetBoss = GameObject.Instantiate(PrefabHolder.Hornet2BossPrefab);
        hornetBoss.SetActive(false);
        PlayMakerFSM destroyFsm = hornetBoss.LocateMyFSM("destroy_if_playerdatabool");
        FsmVariables destroyFsmVars = destroyFsm.FsmVariables;
        destroyFsmVars.FindFsmString("playerData bool").Value = DefeatBoolName;

        int maxHp = 3000; // 3000
        HealthManager hornetHm = hornetBoss.GetComponent<HealthManager>();
        hornetHm.hp = maxHp;

        PlayMakerFSM stunControlFsm = hornetBoss.LocateMyFSM("Stun Control");
        FsmVariables stunControlFsmVars = stunControlFsm.FsmVariables;
        stunControlFsmVars.FindFsmInt("Stun Combo").Value = maxHp;
        stunControlFsmVars.FindFsmInt("Stun Hit Max").Value = maxHp;

        PlayMakerFSM controlFsm = hornetBoss.LocateMyFSM("Control");
        FsmVariables controlFsmVars = controlFsm.FsmVariables;
        controlFsmVars.FindFsmFloat("Gravity").Value = 2.2f; // orig: 2.5
        controlFsmVars.FindFsmFloat("A Dash Speed").Value = 40f; // orig: 35
        controlFsmVars.FindFsmFloat("Evade Speed").Value = 25f; // orig: 22
        controlFsmVars.FindFsmFloat("G Dash Speed").Value = -35f; // orig: -30
        controlFsmVars.FindFsmFloat("Throw Speed").Value = 45f; // orig: 38
        controlFsmVars.FindFsmFloat("Run Speed").Value = -10f; // orig: -8
        controlFsmVars.FindFsmFloat("Run Wait Min").Value = 0.2f; // orig: 0.25
        controlFsmVars.FindFsmFloat("Run Wait Max").Value = 0.5f; // orig: 0.6
        controlFsmVars.FindFsmFloat("Stun Air Speed").Value = 15f; // orig: 10

        Rect h2Arena = new Rect(15, 27, 23, 13);
        Rect wpArena = new Rect(8, 24, 32, 22);

        controlFsmVars.FindFsmFloat("Wall X Left").Value = wpArena.xMin + 0.15f;
        controlFsmVars.FindFsmFloat("Wall X Right").Value = wpArena.xMax - 0.15f;

        //controlFsmVars.FindFsmFloat("Jump X").Value = 39f; // dynamically set in fsm
        //controlFsmVars.FindFsmFloat("Jump Y").Value = 9f; // dynamically set in fsm

        controlFsmVars.FindFsmFloat("Left X").Value = wpArena.xMin + 1.25f;
        controlFsmVars.FindFsmFloat("Right X").Value = wpArena.xMax - 1.25f;

        controlFsmVars.FindFsmFloat("Floor Y").Value = wpArena.yMin + 0.55f;
        controlFsmVars.FindFsmFloat("Roof Y").Value = wpArena.yMax - 1.0f;
        controlFsmVars.FindFsmFloat("Sphere Y").Value = wpArena.yMin + 7.0f;

        controlFsmVars.FindFsmFloat("Throw X L").Value = wpArena.xMin + ((wpArena.width / 3) - 1f);
        controlFsmVars.FindFsmFloat("Throw X R").Value = wpArena.xMax - ((wpArena.width / 3) - 1f);
        ////controlFsm.RemoveAction("Inert", 1);
        controlFsm.GetAction<PlayerDataBoolTest>("Inert", 1).boolName = EncounterPdBoolName;
        controlFsm.RemoveTransition("Inert", "REFIGHT");
        controlFsm.RemoveTransition("Inert", "GG BOSS");
        controlFsm.AddMethod("Wake", () => {
            BlockerGo.SetActive(true);
            AdditionalEffects.SetActive(true);
        });
        controlFsm.GetAction<ApplyMusicCue>("Music", 0).musicCue = fightingMusicCue;
        controlFsm.GetAction<TransitionToAudioSnapshot>("Music", 1).snapshot = _musicAm.FindSnapshot(FightSnapShot);
        controlFsm.GetAction<ApplyMusicCue>("Music (not GG)", 1).musicCue = fightingMusicCue;
        controlFsm.GetAction<TransitionToAudioSnapshot>("Music (not GG)", 2).snapshot = _musicAm.FindSnapshot(FightSnapShot);
        controlFsm.AddMethod("Refight Wake", () => {
            BlockerGo.SetActive(true);
            AdditionalEffects.SetActive(true);
        });
        controlFsm.GetAction<SetFsmString>("Refight Wake", 14).setValue = "SFGrenadeTestOfTeamwork_WeaverPrincessName";
        controlFsm.GetAction<SetFsmString>("Flourish", 6).setValue = "SFGrenadeTestOfTeamwork_WeaverPrincessName";
        controlFsm.GetAction<IntCompare>("Escalation", 2).integer2 = maxHp / 2;
        controlFsm.RemoveAction("Escalation", 4);
        controlFsm.InsertMethod("Escalation", () =>
        {
            foreach (var ps in AdditionalEffects.GetComponentsInChildren<ParticleSystem>())
            {
                ps.startColor = Color.Lerp(endColor, startColor, ((float) hornetHm.hp) / ((float) maxHp));
            }
        }, 0);
        controlFsm.AddMethod("Escalation", () =>
        {
            controlFsmVars.FindFsmFloat("Gravity").Value = 2f; // orig: 2.2f
            controlFsmVars.FindFsmFloat("A Dash Speed").Value = 45f; // orig: 40f
            controlFsmVars.FindFsmFloat("Evade Speed").Value = 30f; // orig: 25f
            controlFsmVars.FindFsmFloat("G Dash Speed").Value = -40f; // orig: -35f
            controlFsmVars.FindFsmFloat("Throw Speed").Value = 55f; // orig: 45f
            controlFsmVars.FindFsmFloat("Run Speed").Value = -15f; // orig: -10f
            controlFsmVars.FindFsmFloat("Run Wait Min").Value = 0.0f; // orig: 0.2f
            controlFsmVars.FindFsmFloat("Run Wait Max").Value = 0.3f; // orig: 0.5f
            controlFsmVars.FindFsmFloat("Stun Air Speed").Value = 20f; // orig: 15f
        });
        controlFsm.SetState(controlFsm.Fsm.StartState);
        //controlFsm.MakeLog();

        EnemyDreamnailReaction hornetEdr = hornetBoss.GetComponent<EnemyDreamnailReaction>();
        hornetEdr.SetConvoTitle("SFGrenadeTestOfTeamwork_WeaverPrincessDN");
        hornetEdr.SetAttr("convoAmount", 1);

        Recoil hornetR = hornetBoss.GetComponent<Recoil>();
        hornetR.SetAttr("recoilSpeedBase", 10f);
        hornetR.SetAttr("recoilDuration", 0.15f);

        EnemyDeathEffectsUninfected hornetEDEU = hornetBoss.GetComponent<EnemyDeathEffectsUninfected>();
        {
            //Log("############################################################\n############################################################\n############################################################");
            GameObject origCorpse = hornetEDEU.GetAttr<EnemyDeathEffects, GameObject>("corpsePrefab");
            //GameObject newCorpse = hornetEDEU.GetAttr<EnemyDeathEffects, GameObject>("corpsePrefab");
            GameObject newCorpse = GameObject.Instantiate(origCorpse);
            newCorpse.SetActive(false);
            PlayMakerFSM corpseControlFsm = newCorpse.LocateMyFSM("Control");
            FsmVariables corpseControlFsmVars = corpseControlFsm.FsmVariables;
            corpseControlFsm.GetAction<CallMethodProper>("Set PD", 2).parameters[0].stringValue = AchievementStrings.DefeatedWeaverPrincessKey;
            corpseControlFsm.GetAction<SetPlayerDataBool>("Set PD", 0).boolName = DefeatBoolName;
            corpseControlFsm.ChangeTransition("Land", "FINISHED", "Pause frame");
            corpseControlFsm.InsertMethod("Land", () =>
            {
                BlockerGo.SetActive(false);
            }, 0);
            corpseControlFsm.AddMethod("Yank", () =>
            {
                newCorpse.SetActive(false);
                corpseControlFsmVars.FindFsmGameObject("Thread").Value.SetActive(false);
            });
            //corpseControlFsm.MakeLog();
            hornetEDEU.SetAttr<EnemyDeathEffects, GameObject>("corpsePrefab", newCorpse);
        }
        {
            GameObject origCorpse = hornetEDEU.GetAttr<EnemyDeathEffects, GameObject>("corpsePrefab");
            //GameObject newCorpse = hornetEDEU.GetAttr<EnemyDeathEffects, GameObject>("corpsePrefab");
            GameObject newCorpse = GameObject.Instantiate(origCorpse, hornetEDEU.transform.position + hornetEDEU.corpseSpawnPoint, Quaternion.identity, hornetEDEU.transform);
            newCorpse.SetActive(false);
            PlayMakerFSM corpseControlFsm = newCorpse.LocateMyFSM("Control");
            FsmVariables corpseControlFsmVars = corpseControlFsm.FsmVariables;
            corpseControlFsm.GetAction<CallMethodProper>("Set PD", 2).parameters[0].stringValue = AchievementStrings.DefeatedWeaverPrincessKey;
            corpseControlFsm.GetAction<SetPlayerDataBool>("Set PD", 0).boolName = DefeatBoolName;
            corpseControlFsm.ChangeTransition("Land", "FINISHED", "Pause frame");
            corpseControlFsm.InsertMethod("Land", () =>
            {
                BlockerGo.SetActive(false);
            }, 0);
            corpseControlFsm.AddMethod("End", () =>
            {
                newCorpse.SetActive(false);
                corpseControlFsmVars.FindFsmGameObject("Thread").Value.SetActive(false);
            });
            //corpseControlFsm.MakeLog();
            hornetEDEU.SetAttr<EnemyDeathEffects, GameObject>("corpse", newCorpse);
        }

        AdditionalEffects.transform.SetParent(hornetBoss.transform, false);
        hornetBoss.transform.position = transform.position;
        hornetBoss.SetActive(true);
    }

    private void Log(string message)
    {
        Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        Debug.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
    }

    private void Log(object message)
    {
        Log($"{message}");
    }
}