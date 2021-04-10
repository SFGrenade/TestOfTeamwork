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

namespace TestOfTeamwork
{
    public class TestOfTeamwork : FullSettingsMod<TotSaveSettings, TotGlobalSettings>
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

            LangStrings = new LanguageStrings();
            SpriteDict = new TextureStrings();

            AchievementHelper.Initialize();
            AchievementHelper.AddAchievement(AchievementStrings.DefeatedWeaverPrincess_Key, TestOfTeamwork.GetSprite(TextureStrings.AchievementWeaverPrincessKey), LanguageStrings.AchievementDefeatedWeaverPrincessTitleKey, LanguageStrings.AchievementDefeatedWeaverPrincessTextKey, true);

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
            ItemHelper.init();
            ItemHelper.AddNormalItem(StateNames.InvStateHornet, TestOfTeamwork.GetSprite(TextureStrings.InvHornetKey), nameof(_saveSettings.SFGrenadeTestOfTeamworkHornetCompanion), LanguageStrings.HornetInvNameKey, LanguageStrings.HornetInvDescKey);
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
            var tmpField = _saveSettingsType.GetField(target);
            if (tmpField != null)
            {
                return (bool)tmpField.GetValue(_saveSettings);
            }
            return PlayerData.instance.GetBoolInternal(target);
        }

        private void OnSetPlayerBoolHook(string target, bool val)
        {
#if DEBUG_CHARMS
            if (target.StartsWith("gotCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.gotCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            if (target.StartsWith("newCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.newCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
            if (target.StartsWith("equippedCharm_"))
            {
                int charmNum = int.Parse(target.Split('_')[1]);
                if (charmHelper.charmIDs.Contains(charmNum))
                {
                    Settings.equippedCustomCharms[charmHelper.charmIDs.IndexOf(charmNum)] = val;
                    return;
                }
            }
#endif
            var tmpField = _saveSettingsType.GetField(target);
            if (tmpField != null)
            {
                tmpField.SetValue(_saveSettings, val);
                return;
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private int OnGetPlayerIntHook(string target)
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
            var tmpField = _saveSettingsType.GetField(target);
            if (tmpField != null)
            {
                return (int)tmpField.GetValue(_saveSettings);
            }
            return PlayerData.instance.GetIntInternal(target);
        }

        private void OnSetPlayerIntHook(string target, int val)
        {
            var tmpField = _saveSettingsType.GetField(target);
            if (tmpField != null)
            {
                tmpField.SetValue(_saveSettings, val);
            }
            else
            {
                PlayerData.instance.SetIntInternal(target, val);
            }
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