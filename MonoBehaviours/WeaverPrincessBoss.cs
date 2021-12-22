using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine.Audio;
using SFCore.MonoBehaviours;
using System.Linq;

namespace TestOfTeamwork.MonoBehaviours
{
    public class WeaverPrincessBoss : MonoBehaviour
    {
        private static AudioMixer _musicAm = null;

        public string EncounterPdBoolName = "SFGrenadeTestOfTeamworkEncounteredWeaverPrincess";
        public string DefeatBoolName = "SFGrenadeTestOfTeamworkDefeatedWeaverPrincess";
        public GameObject AdditionalEffects;
        public string FightSnapShot = "Normal";
        public AudioClip[] FightAudioClips = new AudioClip[6];
        public GameObject BossGo;

        private MusicCue fightingMusicCue;

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
            
            PlayMakerFSM controlFsm = hornetBoss.LocateMyFSM("Control");
            FsmVariables controlFsmVars = controlFsm.FsmVariables;
            controlFsmVars.FindFsmFloat("Gravity").Value = 2f;
            controlFsmVars.FindFsmFloat("A Dash Speed").Value = 40f;
            controlFsmVars.FindFsmFloat("Evade Speed").Value = 25f;
            controlFsmVars.FindFsmFloat("G Dash Speed").Value = -35f;
            controlFsmVars.FindFsmFloat("Throw Speed").Value = -45f;
            controlFsmVars.FindFsmFloat("Run Speed").Value = -10f;
            controlFsmVars.FindFsmFloat("Run Wait Min").Value = 0.2f;
            controlFsmVars.FindFsmFloat("Run Wait Max").Value = 0.5f;
            controlFsmVars.FindFsmFloat("Stun Air Speed").Value = 15f;

            controlFsmVars.FindFsmFloat("Wall X Left").Value = 8f;
            controlFsmVars.FindFsmFloat("Wall X Right").Value = 40f;

            controlFsmVars.FindFsmFloat("Jump X").Value = 39f;
            controlFsmVars.FindFsmFloat("Jump Y").Value = 9f;

            controlFsmVars.FindFsmFloat("Left X").Value = 9f;
            controlFsmVars.FindFsmFloat("Right X").Value = 39f;

            controlFsmVars.FindFsmFloat("Floor Y").Value = 24f;
            controlFsmVars.FindFsmFloat("Roof Y").Value = 52f;
            controlFsmVars.FindFsmFloat("Sphere Y").Value = 30.25f;

            controlFsmVars.FindFsmFloat("Throw X L").Value = 15.45f;
            controlFsmVars.FindFsmFloat("Throw X R").Value = 44.55f;
            controlFsm.GetAction<PlayerDataBoolTest>("Inert", 1).boolName = EncounterPdBoolName;
            controlFsm.AddMethod("Wake", () => {
                AdditionalEffects.SetActive(true);
            });
            controlFsm.GetAction<ApplyMusicCue>("Music", 0).musicCue = fightingMusicCue;
            controlFsm.GetAction<TransitionToAudioSnapshot>("Music", 1).snapshot = _musicAm.FindSnapshot(FightSnapShot);
            controlFsm.GetAction<ApplyMusicCue>("Music (not GG)", 1).musicCue = fightingMusicCue;
            controlFsm.GetAction<TransitionToAudioSnapshot>("Music (not GG)", 2).snapshot = _musicAm.FindSnapshot(FightSnapShot);
            controlFsm.AddMethod("Refight Wake", () => {
                AdditionalEffects.SetActive(true);
            });
            controlFsm.GetAction<SetFsmString>("Refight Wake", 14).setValue = "SFGrenadeTestOfTeamwork_WeaverPrincessName";
            controlFsm.GetAction<SetFsmString>("Flourish", 6).setValue = "SFGrenadeTestOfTeamwork_WeaverPrincessName";
            controlFsm.GetAction<IntCompare>("Escalation", 2).integer2 = 750;

            HealthManager hornetHm = hornetBoss.GetComponent<HealthManager>();
            hornetHm.hp = 1500;

            EnemyDreamnailReaction hornetEdr = hornetBoss.GetComponent<EnemyDreamnailReaction>();
            hornetEdr.SetConvoTitle("SFGrenadeTestOfTeamwork_WeaverPrincessDN");

            Recoil hornetR = hornetBoss.GetComponent<Recoil>();
            hornetR.SetAttr("recoilSpeedBase", 10f);
            hornetR.SetAttr("recoilDuration", 0.15f);

            AdditionalEffects.transform.SetParent(hornetBoss.transform, false);
            hornetBoss.transform.position = transform.position;
            hornetBoss.SetActive(true);
        }

        private void Log(string message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }

        private void Log(object message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }
    }
}