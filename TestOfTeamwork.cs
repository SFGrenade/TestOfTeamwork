using Modding;
using SFCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using InControl;
using TestOfTeamwork.Consts;
using TestOfTeamwork.MonoBehaviours;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace TestOfTeamwork
{
    public class TestOfTeamwork : Mod<TotSaveSettings, TotGlobalSettings>
    {
        internal static TestOfTeamwork Instance;

        public LanguageStrings LangStrings { get; private set; }
        public TextureStrings SpriteDict { get; private set; }
        public AudioStrings AudioDict { get; private set; }
        public SceneChanger SceneChanger { get; private set; }

        public static AudioClip GetAudio(string name) => Instance.AudioDict.Get(name);

        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        // Thx to 56
        public override string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            string ver = asm.GetName().Version.ToString();

            SHA1 sha1 = SHA1.Create();
            FileStream stream = File.OpenRead(asm.Location);

            byte[] hashBytes = sha1.ComputeHash(stream);

            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            stream.Close();
            sha1.Clear();

            string ret = $"{ver}-{hash.Substring(0, 6)}";

            return ret;
        }

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                new ValueTuple<string, string>("White_Palace_18", "White Palace Fly"),
                new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw"),
                new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw (2)"),
                new ValueTuple<string, string>("White_Palace_18", "Soul Totem white_Infinte"),
                new ValueTuple<string, string>("White_Palace_18", "Area Title Controller"),
                new ValueTuple<string, string>("White_Palace_18", "glow response lore 1/Glow Response Object (11)"),
                new ValueTuple<string, string>("White_Palace_18", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_18", "Inspect Region"),
                new ValueTuple<string, string>("White_Palace_18", "_Managers/PlayMaker Unity 2D"),
                new ValueTuple<string, string>("White_Palace_18", "Music Region (1)"),
                //new ValueTuple<string, string>("White_Palace_18", "BlurPlane (1)"),
                new ValueTuple<string, string>("White_Palace_17", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
                new ValueTuple<string, string>("White_Palace_17", "White_ Spikes"),
                new ValueTuple<string, string>("White_Palace_17", "Cave Spikes Invis"),
                new ValueTuple<string, string>("White_Palace_19", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_20", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_20", "Battle Scene"),
                new ValueTuple<string, string>("White_Palace_09", "Quake Floor"),
                new ValueTuple<string, string>("Grimm_Divine", "Charm Holder"),
                //new ValueTuple<string, string>("Abyss_05", "Dusk Knight/Idle Pt"),
                //new ValueTuple<string, string>("Abyss_05", "Dusk Knight/Dream Enter 2"),
                //new ValueTuple<string, string>("Abyss_05", "door_dreamReturn"),
                //new ValueTuple<string, string>("Abyss_05", "door_dreamReturn_reality"),
                //new ValueTuple<string, string>("White_Palace_03_hub", "door1"),
                //new ValueTuple<string, string>("White_Palace_03_hub", "Dream Entry"),
                //new ValueTuple<string, string>("White_Palace_03_hub", "doorWarp"),
                //new ValueTuple<string, string>("White_Palace_03_hub", "dream_beam_animation"),
                new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
                new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette"),
                new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Outskirts Battle Encounter")
            };
        }

        public TestOfTeamwork() : base("Test of Teamwork")
        {
            Instance = this;

            LangStrings = new LanguageStrings();
            SpriteDict = new TextureStrings();

            AchievementHelper.Initialize();
            AchievementHelper.AddAchievement(AchievementStrings.DefeatedWeaverPrincess_Key, TestOfTeamwork.GetSprite(TextureStrings.AchievementWeaverPrincessKey), LanguageStrings.AchievementDefeatedWeaverPrincessTitleKey, LanguageStrings.AchievementDefeatedWeaverPrincessTextKey, true);
            InitCallbacks();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");
            Instance = this;

            InitGlobalSettings();
            SceneChanger = new SceneChanger(preloadedObjects);
            AudioDict = new AudioStrings(SceneChanger);
            InitInventory();
            //UIManager.instance.RefreshAchievementsList();

            //GameManager.instance.StartCoroutine(DEBUG_Shade_Style());
            GameManager.instance.StartCoroutine(Register2BossModCore());

            Log("Initialized");
        }
        
        private void InitGlobalSettings()
        {
            // Found in a project, might help saving, don't know, but who cares
            // Global Settings
            GlobalSettings.SpriteData = GlobalSettings.SpriteData;
        }

        private void InitSaveSettings(SaveGameData data)
        {
            // Found in a project, might help saving, don't know, but who cares
            // Save Settings
            // Start Mod Quest
            Settings.SFGrenadeTestOfTeamworkStartQuest = Settings.SFGrenadeTestOfTeamworkStartQuest;
            if (!Settings.SFGrenadeTestOfTeamworkStartQuest)
                Settings.SFGrenadeTestOfTeamworkStartQuest = (PlayerData.instance.royalCharmState == 4);
            // Mechanics
            Settings.SFGrenadeTestOfTeamworkHornetCompanion = Settings.SFGrenadeTestOfTeamworkHornetCompanion;
            // Bosses
            Settings.SFGrenadeTestOfTeamworkDefeatedWeaverPrincess = Settings.SFGrenadeTestOfTeamworkDefeatedWeaverPrincess;
            // Areas
            Settings.SFGrenadeTestOfTeamworkTotOpened = Settings.SFGrenadeTestOfTeamworkTotOpened;
            Settings.SFGrenadeTestOfTeamworkVisitedTestOfTeamwork = Settings.SFGrenadeTestOfTeamworkVisitedTestOfTeamwork;
            Settings.SFGrenadeTestOfTeamworkTotOpenedShortcut = Settings.SFGrenadeTestOfTeamworkTotOpenedShortcut;
            Settings.SFGrenadeTestOfTeamworkTotOpenedTotem = Settings.SFGrenadeTestOfTeamworkTotOpenedTotem;

            //// Charms
            //Settings.gotCharms = Settings.gotCharms;
            //Settings.newCharms = Settings.newCharms;
            //Settings.equippedCharms = Settings.equippedCharms;
            //Settings.charmCosts = Settings.charmCosts;
        }

        private void InitCallbacks()
        {
            // Hooks
            ModHooks.Instance.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.Instance.SetPlayerBoolHook += OnSetPlayerBoolHook;
            ModHooks.Instance.GetPlayerIntHook += OnGetPlayerIntHook;
            ModHooks.Instance.SetPlayerIntHook += OnSetPlayerIntHook;
            ModHooks.Instance.AfterSavegameLoadHook += InitSaveSettings;
            ModHooks.Instance.ApplicationQuitHook += SaveTotGlobalSettings;
            ModHooks.Instance.LanguageGetHook += OnLanguageGetHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void InitInventory()
        {
            ItemHelper.AddNormalItem(StateNames.InvStateHornet, TestOfTeamwork.GetSprite(TextureStrings.InvHornetKey), nameof(Settings.SFGrenadeTestOfTeamworkHornetCompanion), LanguageStrings.HornetInvNameKey, LanguageStrings.HornetInvDescKey);
        }

        private void OnSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            // Don't change scene content if quest isn't started
            if (!Settings.SFGrenadeTestOfTeamworkStartQuest)
                return;

            string scene = to.name;
            Log("Scene Changed to: " + scene);

            try
            {
                if (HeroController.instance.gameObject.GetComponent<SceneExpander>() != null)
                {
                    UObject.Destroy(HeroController.instance.gameObject.GetComponent<SceneExpander>());
                }
            }
            catch
            { }

            if (scene == TransitionGateNames.Rt)
            {
                // Black Egg Entrance, needs change to make Hornet give the Item
                SceneChanger.CR_Change_Room_temple(to);
            }
            else if (scene == TransitionGateNames.Wp06)
            {
                // Path of Pain Entrance, needs change to make "Test of Teamwork" accessible
                SceneChanger.CR_Change_White_Palace_06(to);
                //var tmpET = GameObject.FindObjectOfType<SceneManager>().gameObject.LocateMyFSM("Music Control").GetAction<SendEventByName>("Start", 1).eventTarget;
                //Log($"tmpET.target: {tmpET.target.ToString()}");
                //Log($"tmpET.excludeSelf: {tmpET.excludeSelf.Value}");
                //Log($"tmpET.fsmComponent: {tmpET.fsmComponent}");
                //Log($"tmpET.fsmName: {tmpET.fsmName.Value}");
                //Log($"tmpET.gameObject.OwnerOption: {tmpET.gameObject.OwnerOption.ToString()}");
                //Log($"tmpET.gameObject.GameObject: {tmpET.gameObject.GameObject.Value}");
                //Log($"tmpET.sendToChildren: {tmpET.sendToChildren.Value}");
            }
            else if (scene == TransitionGateNames.Tot01)
            {
                SceneChanger.CR_Change_ToT01(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.Tot02)
            {
                SceneChanger.CR_Change_ToT02(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.Tot03)
            {
                SceneChanger.CR_Change_ToT03(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.TotEndless)
            {
                SceneChanger.CR_Change_ToTEndless(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            //else if (scene == "GG_Hive_Knight")
            //{
            //    var hkGO = to.Find("Hive Knight");
            //    var hkFsm = hkGO.LocateMyFSM("Control");
            //    var hkFsmVars = hkFsm.FsmVariables;

            //    var srev3_1 = hkFsm.GetAction<SendRandomEventV3>("Phase 1", 1);
            //    srev3_1.events = new FsmEvent[] { FsmEvent.FindEvent("JUMP") };
            //    srev3_1.weights = new FsmFloat[] { 1.0f };
            //    srev3_1.trackingInts = new FsmInt[] { hkFsmVars.FindFsmInt("Ct Jump") };
            //    srev3_1.eventMax = new FsmInt[] { int.MaxValue };
            //    srev3_1.trackingIntsMissed = new FsmInt[] { hkFsmVars.FindFsmInt("Ms Jump") };
            //    srev3_1.missedMax = new FsmInt[] { 4 };

            //    var srev3_2 = hkFsm.GetAction<SendRandomEventV3>("Phase 2", 2);
            //    srev3_2.events = new FsmEvent[] { FsmEvent.FindEvent("JUMP") };
            //    srev3_2.weights = new FsmFloat[] { 1.0f };
            //    srev3_2.trackingInts = new FsmInt[] { hkFsmVars.FindFsmInt("Ct Jump") };
            //    srev3_2.eventMax = new FsmInt[] { int.MaxValue };
            //    srev3_2.trackingIntsMissed = new FsmInt[] { hkFsmVars.FindFsmInt("Ms Jump") };
            //    srev3_2.missedMax = new FsmInt[] { 4 };

            //    var srev3_3 = hkFsm.GetAction<SendRandomEventV3>("Phase 3", 1);
            //    srev3_3.events = new FsmEvent[] { FsmEvent.FindEvent("JUMP") };
            //    srev3_3.weights = new FsmFloat[] { 1.0f };
            //    srev3_3.trackingInts = new FsmInt[] { hkFsmVars.FindFsmInt("Ct Jump") };
            //    srev3_3.eventMax = new FsmInt[] { int.MaxValue };
            //    srev3_3.trackingIntsMissed = new FsmInt[] { hkFsmVars.FindFsmInt("Ms Jump") };
            //    srev3_3.missedMax = new FsmInt[] { 4 };
            //}
        }

        private void SaveTotGlobalSettings()
        {
            SaveGlobalSettings();
        }

        #region Get/Set Hooks

        private string OnLanguageGetHook(string key, string sheet)
        {
            //Log($"Sheet: {sheet}; Key: {key}");
            ////if ("CUSTOM_HORNET_PRE_FINAL_BATTLE" == key)
            ////{
            ////    Settings.SFGrenadeTestOfTeamworkHornetCompanion = true;
            ////}
            // There probably is a better way to do this, but for now take this
            //#region Custom Charms
            //if (key.StartsWith("CHARM_NAME_"))
            //{
            //    int charmNum = int.Parse(key.Split('_')[2]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        return "CHARM NAME";
            //    }
            //}
            //if (key.StartsWith("CHARM_DESC_"))
            //{
            //    int charmNum = int.Parse(key.Split('_')[2]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        return "CHARM DESC";
            //    }
            //}
            //#endregion
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return Language.Language.GetInternal(key, sheet);
        }

        private bool OnGetPlayerBoolHook(string target)
        {
            if (Settings.BoolValues.ContainsKey(target))
            {
                return Settings.BoolValues[target];
            }
            //#region Custom Charms
            //if (target.StartsWith("gotCharm_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        return Settings.gotCharms[charmHelper.charmIDs.IndexOf(charmNum)];
            //    }
            //}
            //if (target.StartsWith("newCharm_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        return Settings.newCharms[charmHelper.charmIDs.IndexOf(charmNum)];
            //    }
            //}
            //if (target.StartsWith("equippedCharm_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        return Settings.equippedCharms[charmHelper.charmIDs.IndexOf(charmNum)];
            //    }
            //}
            //#endregion
            return PlayerData.instance.GetBoolInternal(target);
        }

        private void OnSetPlayerBoolHook(string target, bool val)
        {
            if (Settings.BoolValues.ContainsKey(target))
            {
                Settings.BoolValues[target] = val;
                return;
            }
            //#region Custom Charms
            //if (target.StartsWith("gotCharm_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        Settings.gotCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
            //        return;
            //    }
            //}
            //if (target.StartsWith("newCharm_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        Settings.newCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
            //        return;
            //    }
            //}
            //if (target.StartsWith("equippedCharm_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        Settings.equippedCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
            //        return;
            //    }
            //}
            //#endregion
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private int OnGetPlayerIntHook(string target)
        {
            if (Settings.IntValues.ContainsKey(target))
            {
                return Settings.IntValues[target];
            }
            //#region Custom Charms
            //if (target.StartsWith("charmCost_"))
            //{
            //    int charmNum = int.Parse(target.Split('_')[1]);
            //    if (charmHelper.charmIDs.Contains(charmNum))
            //    {
            //        return Settings.charmCosts[charmHelper.charmIDs.IndexOf(charmNum)];
            //    }
            //}
            //#endregion
            return PlayerData.instance.GetIntInternal(target);
        }

        private void OnSetPlayerIntHook(string target, int val)
        {
            if (Settings.IntValues.ContainsKey(target))
            {
                Settings.IntValues[target] = val;
            }
            else
            {
                PlayerData.instance.SetIntInternal(target, val);
            }
            if ((target == "royalCharmState") && (!Settings.SFGrenadeTestOfTeamworkStartQuest))
                Settings.SFGrenadeTestOfTeamworkStartQuest = (PlayerData.instance.royalCharmState == 4);
            //Log("Int  set: " + target + "=" + val.ToString());
        }

        #endregion Get/Set Hooks

        private void PrintDebugFsm(PlayMakerFSM fsm)
        {
            foreach (var state in fsm.FsmStates)
            {
                Log("State: " + state.Name);
                foreach (var trans in state.Transitions)
                {
                    Log("\t" + trans.EventName + " -> " + trans.ToState);
                }
            }
        }

        private IEnumerator DebugPrintWait(string name)
        {
            yield return new WaitWhile(() => !(GameObject.Find(name)));

            PrintDebug(GameObject.Find(name));
        }

        private void PrintDebug(GameObject go, string tabindex = "", int parentCount = 0)
        {
            Transform parent = go.transform.parent;
            for (int i = 0; i < parentCount; i++)
            {
                if (parent == null) continue;

                Log(tabindex + "DEBUG parent: " + parent.gameObject.name);
                parent = parent.parent;
            }
            Log(tabindex + "DEBUG Name: " + go.name);
            foreach (var comp in go.GetComponents<Component>())
            {
                Log(tabindex + "DEBUG Component: " + comp.GetType());
            }
            for (int i = 0; i < go.transform.childCount; i++)
            {
                PrintDebug(go.transform.GetChild(i).gameObject, tabindex + "\t");
            }
        }

        private static void SetInactive(GameObject go)
        {
            if (go == null) return;

            UnityEngine.Object.DontDestroyOnLoad(go);
            go.SetActive(false);
        }

        private static void SetInactive(UnityEngine.Object go)
        {
            if (go != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
        }

        private bool r2BmcTimeout;
        private bool r2BmcSuccess;
        private const string R2BmcSep = " - ";
        private const string R2BmcBmc = "BossModCore";
        private const string R2BmcCom = R2BmcBmc + R2BmcSep;
        private const string R2BmcSetNum = R2BmcSep + "numBosses";
        private const string R2BmcSetStatName = R2BmcSep + "statueName" + R2BmcSep;
        private const string R2BmcSetStatDesc = R2BmcSep + "statueDescription" + R2BmcSep;
        private const string R2BmcSetCustomScene = R2BmcSep + "customScene" + R2BmcSep;
        private const string R2BmcSetCustomSceneName = R2BmcSep + "scenePrefabName" + R2BmcSep;
        private const string R2BmcSetStatGo = R2BmcSep + "statueGO" + R2BmcSep;

        private IEnumerator Register2BossModCore()
        {
            PlayerData pd = PlayerData.instance;
            r2BmcTimeout = false;

            GameManager.instance.StartCoroutine(RegisterTimeout());

            while (!r2BmcTimeout)
            {
                r2BmcSuccess = pd.GetBool(R2BmcBmc);
                if (r2BmcSuccess)
                {
                    r2BmcTimeout = true;
                }
                yield return null;
            }
            if (!r2BmcSuccess)
            {
                Log(R2BmcBmc + " not found!");
                yield break;
            }
            Log(R2BmcBmc + " is able to be registered to!");
            yield return null;

            pd.SetInt(R2BmcCom + this.GetType().Name + R2BmcSetNum, 1);
            pd.SetString(R2BmcCom + this.GetType().Name + R2BmcSetStatName + "0", "Boss Statue Name");
            pd.SetString(R2BmcCom + this.GetType().Name + R2BmcSetStatDesc + "0", "Boss Statue Description");
            pd.SetBool(R2BmcCom + this.GetType().Name + R2BmcSetCustomScene + "0", false);
            pd.SetString(R2BmcCom + this.GetType().Name + R2BmcSetCustomSceneName + "0", "GG_Hornet_2");
            pd.SetVariable<GameObject>(R2BmcCom + this.GetType().Name + R2BmcSetStatGo + "0", new GameObject("StatePrefabGO"));
        }

        private IEnumerator RegisterTimeout()
        {
            yield return new WaitForSecondsRealtime(3.0f);
            r2BmcTimeout = true;
            r2BmcSuccess = false;
            yield break;
        }
    }
}