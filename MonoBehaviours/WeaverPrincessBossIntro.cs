using HutongGames.PlayMaker.Actions;
using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;

namespace TestOfTeamwork.MonoBehaviours
{
    public class WeaverPrincessBossIntro : MonoBehaviour
    {
        public string EncounterPdBoolName = "SFGrenadeTestOfTeamworkEncounterBeforeBoss";
        public string Dialogue1ConvoSheet = "Hornet";
        public string Dialogue1ConvoKey = Consts.LanguageStrings.HornetMonologue1Key;
        public string Dialogue2ConvoSheet = "Hornet";
        public string Dialogue2ConvoKey = Consts.LanguageStrings.HornetMonologue2Key;

        public void Start()
        {
            var beforeFightPrefab = Instantiate(PrefabHolder.Hornet2BossPrefab);
            beforeFightPrefab.transform.position = transform.position;
            beforeFightPrefab.SetActive(true);

            var encounterFsm = beforeFightPrefab.LocateMyFSM("Encounter");
            encounterFsm.GetAction<PlayerDataBoolTest>("Init", 1).boolName = EncounterPdBoolName;

            encounterFsm.RemoveAction("Point", 2);

            var dialogueVars1 = new[] { new HutongGames.PlayMaker.FsmVar(typeof(string)), new HutongGames.PlayMaker.FsmVar(typeof(string)) };
            dialogueVars1[0].SetValue(Dialogue1ConvoKey);
            dialogueVars1[1].SetValue(Dialogue1ConvoSheet);
            encounterFsm.GetAction<CallMethodProper>("Dialogue", 1).parameters = dialogueVars1;

            var dialogueVars2 = new[] { new HutongGames.PlayMaker.FsmVar(typeof(string)), new HutongGames.PlayMaker.FsmVar(typeof(string)) };
            dialogueVars2[0].SetValue(Dialogue2ConvoKey);
            dialogueVars2[1].SetValue(Dialogue2ConvoSheet);
            encounterFsm.GetAction<CallMethodProper>("Dialogue 2", 0).parameters = dialogueVars2;

            encounterFsm.RemoveAction("Start Fight", 0);

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