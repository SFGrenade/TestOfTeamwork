using Modding;
using SFCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Win32;
using TestOfTeamwork.Consts;
using TestOfTeamwork.MonoBehaviours;
using UnityEngine;
using UObject = UnityEngine.Object;
using SFCore.Generics;
using SFCore.Utils;
using HutongGames.PlayMaker.Actions;
using System.IO;
using Language;
using Newtonsoft.Json;
using TMPro;
using Random = UnityEngine.Random;

namespace TestOfTeamwork
{
    public class TestOfTeamwork : FullSettingsMod<TotSaveSettings, TotGlobalSettings>
    {
        internal static TestOfTeamwork Instance;

        public Consts.LanguageStrings LangStrings { get; private set; }
        public TextureStrings SpriteDict { get; private set; }
        public AudioStrings AudioDict { get; private set; }
        public SceneChanger SceneChanger { get; private set; }

#if DEBUG_CHARMS
        // DEBUG
        public CharmHelper charmHelper { get; private set; }
#endif

        public static AudioClip GetAudio(string name) => Instance.AudioDict.Get(name);

        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        public override string GetVersion() => SFCore.Utils.Util.GetVersion(Assembly.GetExecutingAssembly());

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

            LangStrings = new Consts.LanguageStrings();
            SpriteDict = new TextureStrings();

            AchievementHelper.AddAchievement(AchievementStrings.DefeatedWeaverPrincess_Key, TestOfTeamwork.GetSprite(TextureStrings.AchievementWeaverPrincessKey), Consts.LanguageStrings.AchievementDefeatedWeaverPrincessTitleKey, Consts.LanguageStrings.AchievementDefeatedWeaverPrincessTextKey, true);

            InitInventory();

#if DEBUG_CHARMS
            charmHelper = new CharmHelper();
            charmHelper.customCharms = 4;
            charmHelper.customSprites = new Sprite[] { GetSprite(TextureStrings.YKey), GetSprite(TextureStrings.EKey), GetSprite(TextureStrings.EKey), GetSprite(TextureStrings.TKey) };
#endif

            InitCallbacks();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");
            Instance = this;

            InitGlobalSettings();
            SceneChanger = new SceneChanger(preloadedObjects);
            AudioDict = new AudioStrings(SceneChanger);
            //UIManager.instance.RefreshAchievementsList();

            //GameManager.instance.StartCoroutine(DEBUG_Shade_Style());
            GameManager.instance.StartCoroutine(Register2BossModCore());

            #region Achievements

            foreach (var keyname in Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Team Cherry").OpenSubKey("Hollow Knight").GetValueNames())
            {
                if (keyname.Contains("_"))
                {
                    string paddedName = keyname.Substring(0, keyname.LastIndexOf('_'));
                    try
                    {
                        string decryptedName = Encryption.Decrypt(paddedName);
                        string ret = (string) typeof(PlayerPrefsSharedData).GetMethod("ReadEncrypted", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Platform.Current.EncryptedSharedData, new object[] { decryptedName });

                        Log($"Raw Key: '{decryptedName}': '{ret}'");
                    }
                    catch (Exception e)
                    {
                        string retString = PlayerPrefs.GetString(paddedName, "DOESN'T EXIST");
                        if (retString.Equals("DOESN'T EXIST"))
                        {
                            float retfloat = PlayerPrefs.GetFloat(paddedName, -123.456f);
                            if (retfloat.Equals(-123.456f))
                            {
                                Log($"Other Key: '{paddedName}': '{PlayerPrefs.GetInt(paddedName, 0)}'");
                            }
                            else
                            {
                                Log($"Float Key: '{paddedName}': '{retfloat}'");
                            }
                        }
                        else
                        {
                            Log($"String Key: '{paddedName}': '{retString}'");
                        }
                    }
                }
            }

            #endregion

            //Log("Loading Hugger 1");
            //memoryHugger1 = new int[536870912];
            //Log("Loading Hugger 2");
            //memoryHugger2 = new int[536870912];
            //Log("Loading Hugger 3");
            //memoryHugger3 = new int[536870912];
            //Log("Loading Hugger 4");
            //memoryHugger4 = new int[536870912];
            //Log("Loading Hugger 5");
            //memoryHugger5 = new int[536870912];

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
        }

        private void InitCallbacks()
        {
            // Hooks
            ModHooks.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.SetPlayerBoolHook += OnSetPlayerBoolHook;
            ModHooks.GetPlayerIntHook += OnGetPlayerIntHook;
            ModHooks.SetPlayerIntHook += OnSetPlayerIntHook;
            ModHooks.AfterSavegameLoadHook += InitSaveSettings;
            ModHooks.ApplicationQuitHook += SaveTotGlobalSettings;
            ModHooks.LanguageGetHook += OnLanguageGetHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void InitInventory()
        {
            ItemHelper.AddNormalItem(TestOfTeamwork.GetSprite(TextureStrings.InvHornetKey), nameof(_saveSettings.SFGrenadeTestOfTeamworkHornetCompanion), Consts.LanguageStrings.HornetInvNameKey, Consts.LanguageStrings.HornetInvDescKey);
        }

        private void OnSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            string scene = to.name;

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
            }
            else if (scene == TransitionGateNames.Tot01)
            {
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.Tot02)
            {
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.Tot03)
            {
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.TotEndless)
            {
                SceneChanger.CR_Change_ToTEndless(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.TotDropdown)
            {
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == "GG_Hornet_2")
            {
                var go = to.Find("Hornet Boss 2");
                var fsm = go.LocateMyFSM("Control");
                fsm.GetAction<IntCompare>("Escalation", 2).integer2 = 99999999;
                fsm.ChangeTransition("Refight Wake", "FINISHED", "Barb Antic");
                fsm.ChangeTransition("Barb Recover", "FINISHED", "Flourish?");
                go.GetComponent<HealthManager>().hp = 1500;
            }
        }

        private void SaveTotGlobalSettings()
        {
            SaveGlobalSettings();
        }

#region Get/Set Hooks

        private string OnLanguageGetHook(string key, string sheet, string orig)
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
            return orig;
        }

        private bool OnGetPlayerBoolHook(string target, bool orig)
        {
#if DEBUG_CHARMS
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.gotCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.newCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.equippedCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
#endif
            var tmpField = ReflectionHelper.GetFieldInfo(typeof(TotSaveSettings), target, false);
            if (tmpField != null)
            {
                return (bool)tmpField.GetValue(_saveSettings);
            }
            return orig;
        }

        private bool OnSetPlayerBoolHook(string target, bool orig)
        {
#if DEBUG_CHARMS
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.gotCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.newCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.equippedCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                }
            }
#endif
            var tmpField = ReflectionHelper.GetFieldInfo(typeof(TotSaveSettings), target, false);
            if (tmpField != null)
            {
                tmpField.SetValue(_saveSettings, orig);
            }
            return orig;
        }

        private int OnGetPlayerIntHook(string target, int orig)
        {
#if DEBUG_CHARMS
            if (target.StartsWith("charmCost_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    return Settings.customCharmCosts[charmHelper.charmIDs.IndexOf(charmNum)];
                }
            }
#endif
            var tmpField = ReflectionHelper.GetFieldInfo(typeof(TotSaveSettings), target, false);
            if (tmpField != null)
            {
                return (int) tmpField.GetValue(_saveSettings);
            }
            return orig;
        }

        private int OnSetPlayerIntHook(string target, int orig)
        {
            var tmpField = ReflectionHelper.GetFieldInfo(typeof(TotSaveSettings), target, false);
            if (tmpField != null)
            {
                tmpField.SetValue(_saveSettings, orig);
            }
            return orig;
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