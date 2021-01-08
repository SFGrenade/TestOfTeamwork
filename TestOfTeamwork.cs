//#define DEBUG_CHARMS

using Modding;
using SFCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using HutongGames.PlayMaker.Actions;
using InControl;
using ModCommon.Util;
using TestOfTeamwork.Consts;
using TestOfTeamwork.MonoBehaviours;
using TestOfTeamwork.MonoBehaviours.Patcher;
using TestOfTeamwork.Utils;
using UnityEngine;
using UnityEngine.Audio;
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

#if DEBUG_CHARMS
        // DEBUG
        public CharmHelper charmHelper { get; private set; }
#endif

        public static AudioClip GetAudio(string name) => Instance.AudioDict.Get(name);

        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

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
            return $"{ver}-{hash.Substring(0, 6)}";
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
                new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
                new ValueTuple<string, string>("White_Palace_17", "White_ Spikes"),
                new ValueTuple<string, string>("White_Palace_17", "Cave Spikes Invis"),
                new ValueTuple<string, string>("White_Palace_09", "Quake Floor"),
                new ValueTuple<string, string>("Grimm_Divine", "Charm Holder"),
                new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
                new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette"),
                new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Outskirts Battle Encounter"),
                new ValueTuple<string, string>("White_Palace_03_hub", "door1"),
                new ValueTuple<string, string>("White_Palace_03_hub", "Dream Entry")
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

#if DEBUG_CHARMS
            charmHelper = new CharmHelper();
            charmHelper.customCharms = 6;
            charmHelper.customSprites = new Sprite[] { GetSprite(TextureStrings.YKey), GetSprite(TextureStrings.EKey), GetSprite(TextureStrings.EKey), GetSprite(TextureStrings.TKey), GetSprite(TextureStrings.GCKey), GetSprite(TextureStrings.GC2Key) };
#endif

            //GameManager.instance.StartCoroutine(DEBUG_Shade_Style());
            GameManager.instance.StartCoroutine(Register2BossModCore());

            Log("Initialized");
        }
        
        private void InitGlobalSettings()
        {
            // Found in a project, might help saving, don't know, but who cares
            // Global Settings
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

#if DEBUG_CHARMS
            // Charms
            Settings.gotCharms = Settings.gotCharms;
            Settings.newCharms = Settings.newCharms;
            Settings.equippedCharms = Settings.equippedCharms;
            Settings.charmCosts = Settings.charmCosts;
#endif
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
            else if (scene == TransitionGateNames.TotDropdown)
            {
                SceneChanger.CR_Change_ToTDropdown(to);
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
#if DEBUG_CHARMS
            // There probably is a better way to do this, but for now take this
            #region Custom Charms
            if (key.StartsWith("CHARM_NAME_"))
            {
                int charmNum = int.Parse(key.Split('_')[2]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return "CHARM NAME";
                }
            }
            if (key.StartsWith("CHARM_DESC_"))
            {
                int charmNum = int.Parse(key.Split('_')[2]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return "CHARM DESC";
                }
            }
            #endregion
#endif
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return Language.Language.GetInternal(key, sheet);
        }

        private bool OnGetPlayerBoolHook(string target)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                return (bool)tmpField.GetValue(Settings);
            }
#if DEBUG_CHARMS
            #region Custom Charms
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.gotCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.newCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.equippedCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            #endregion
#endif
            return PlayerData.instance.GetBoolInternal(target);
        }

        private void OnSetPlayerBoolHook(string target, bool val)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                tmpField.SetValue(Settings, val);
                return;
            }
#if DEBUG_CHARMS
            #region Custom Charms
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.gotCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.newCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.equippedCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            #endregion
#endif
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private int OnGetPlayerIntHook(string target)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                return (int)tmpField.GetValue(Settings);
            }
#if DEBUG_CHARMS
            #region Custom Charms
            if (target.StartsWith("charmCost_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.charmCosts[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            #endregion
#endif
            return PlayerData.instance.GetIntInternal(target);
        }

        private void OnSetPlayerIntHook(string target, int val)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                tmpField.SetValue(Settings, val);
            }
            else
            {
                PlayerData.instance.SetIntInternal(target, val);
            }
            if ((target == "royalCharmState") && (!Settings.SFGrenadeTestOfTeamworkStartQuest))
                Settings.SFGrenadeTestOfTeamworkStartQuest = (PlayerData.instance.royalCharmState >= 4);
        }

        #endregion Get/Set Hooks

        enum Commands
        {
            NumBosses,
            StatueName,
            StatueDescription,
            CustomScene,
            ScenePrefabName,
            StatueGO
        }
        private bool r2BmcTimeout;
        private bool r2BmcSuccess;
        private static readonly string R2BmcBmc = "BossModCore";
        private static readonly string R2BmcCom = $"{R2BmcBmc} - ";
        private static readonly string R2BmcSetNum = $" - {Commands.NumBosses}";
        private static readonly string R2BmcSetStatName = $" - {Commands.StatueName} - ";
        private static readonly string R2BmcSetStatDesc = $" - {Commands.StatueDescription} - ";
        private static readonly string R2BmcSetCustomScene = $" - {Commands.CustomScene} - ";
        private static readonly string R2BmcSetCustomSceneName = $" - {Commands.ScenePrefabName} - ";
        private static readonly string R2BmcSetStatGo = $" - {Commands.StatueGO} - ";

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