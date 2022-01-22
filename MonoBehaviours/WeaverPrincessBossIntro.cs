using System.Linq;
using HutongGames.PlayMaker.Actions;
using SFCore.MonoBehaviours;
using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;
using UnityEngine.Audio;

namespace TestOfTeamwork.MonoBehaviours
{
    public class WeaverPrincessBossIntro : MonoBehaviour
    {
        private static AudioMixer _musicAm = null;

        public string EncounterPdBoolName = "SFGrenadeTestOfTeamworkEncounteredWeaverPrincess";
        public string Dialogue1ConvoSheet = "Hornet";
        public string Dialogue1ConvoKey = Consts.LanguageStrings.HornetMonologue1Key;
        public string Dialogue2ConvoSheet = "Hornet";
        public string Dialogue2ConvoKey = Consts.LanguageStrings.HornetMonologue2Key;
        public string FightSnapShot = "Normal";
        public AudioClip[] FightAudioClips = new AudioClip[6];
        public GameObject BossGo;

        private MusicCue fightingMusicCue;
        private GameObject particles;
        private GameObject blocker;

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

            particles = gameObject.Find("Particles Falling");
            blocker = gameObject.Find("Blocker");

            var beforeFightPrefab = Instantiate(PrefabHolder.Hornet2BossEncounterPrefab);
            beforeFightPrefab.SetActive(false);
            beforeFightPrefab.transform.position = transform.position;
            //beforeFightPrefab.SetActive(true); // DEBUG
            beforeFightPrefab.transform.localScale.Scale(transform.localScale);

            var bfpDIPT = beforeFightPrefab.AddComponent<DeactivateIfPlayerdataTrue>();
            bfpDIPT.boolName = nameof(TestOfTeamwork.Instance.SaveSettings.SFGrenadeTestOfTeamworkDefeatedWeaverPrincess);

            var encounterFsm = beforeFightPrefab.LocateMyFSM("Encounter");
            //encounterFsm.RemoveAction("Init", 1);
            encounterFsm.GetAction<PlayerDataBoolTest>("Init", 1).boolName = EncounterPdBoolName;
            encounterFsm.RemoveTransition("Init", "DESTROY");

            encounterFsm.RemoveAction("Point", 2);
            encounterFsm.InsertAction("Point", new TransitionToAudioSnapshot() {
                transitionTime = 2f,
                snapshot = _musicAm.FindSnapshot("Silent")
            }, 2);

            var dialogueVars1 = new[] { new HutongGames.PlayMaker.FsmVar(typeof(string)), new HutongGames.PlayMaker.FsmVar(typeof(string)) };
            dialogueVars1[0].SetValue(Dialogue1ConvoKey);
            dialogueVars1[1].SetValue(Dialogue1ConvoSheet);
            encounterFsm.GetAction<CallMethodProper>("Dialogue", 1).parameters = dialogueVars1;

            encounterFsm.AddMethod("Blizzard Start", () => {
                particles.SetActive(true);
                blocker.SetActive(false);
            });
            encounterFsm.AddAction("Blizzard Start", new Wait() {
                time = 1,
                finishEvent = HutongGames.PlayMaker.FsmEvent.FindEvent("FINISHED"),
                realTime = false
            });

            var dialogueVars2 = new[] { new HutongGames.PlayMaker.FsmVar(typeof(string)), new HutongGames.PlayMaker.FsmVar(typeof(string)) };
            dialogueVars2[0].SetValue(Dialogue2ConvoKey);
            dialogueVars2[1].SetValue(Dialogue2ConvoSheet);
            encounterFsm.GetAction<CallMethodProper>("Dialogue 2", 0).parameters = dialogueVars2;

            encounterFsm.GetAction<SetPlayerDataBool>("Start Fight", 0).boolName = EncounterPdBoolName;
            encounterFsm.GetAction<TransitionToAudioSnapshot>("Start Fight", 3).transitionTime = 0.5f;
            encounterFsm.InsertAction("Start Fight", new ApplyMusicCue() {
                delayTime = 0f,
                transitionTime = 0.1f,
                musicCue = fightingMusicCue
            }, 4);

            encounterFsm.InsertMethod("Start Fight", () => {
                BossGo.SetActive(true);
            }, 5); // 0
            encounterFsm.SetState(encounterFsm.Fsm.StartState);
            //encounterFsm.MakeLog();
            beforeFightPrefab.SetActive(true);
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