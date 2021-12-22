using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using SFCore.Utils;

namespace TestOfTeamwork
{
    class PrefabHolder
    {
        public static GameObject WpFlyPrefab { get; private set; }
        public static GameObject WpSawWithSoundPrefab { get; private set; }
        public static GameObject WpSawNoSoundPrefab { get; private set; }
        public static GameObject WpInfSoulTotemPrefab { get; private set; }
        public static GameObject WpSpikesPrefab { get; private set; }
        public static TinkEffect WpTinkEffectPrefab { get; private set; }
        public static GameObject PopAreaTitleCtrlPrefab { get; private set; }
        public static GameObject PopSobPartPrefab { get; private set; }
        public static GameObject PopTabletInspectPrefab { get; private set; }
        public static GameObject PopSceneManagerPrefab { get; private set; }
        public static GameObject PopPmU2dPrefab { get; private set; }
        public static GameObject PopMusicRegionPrefab { get; private set; }
        public static MusicCue PopEnterMusicCuePrefab { get; private set; }
        public static GameObject PopQuakeFloorPrefab { get; private set; }
        public static GameObject WhiteBenchPrefab { get; private set; }
        public static GameObject WpLeverPrefab { get; private set; }
        public static GameObject BreakableWallPrefab { get; private set; }
        public static GameObject Hornet2BossEncounterPrefab { get; private set; }
        public static GameObject Hornet2BossPrefab { get; private set; }
        public static GameObject ShinyPrefab { get; private set; }
        public static GameObject Wp03Door { get; private set; }
        public static GameObject Wp03Dream { get; private set; }

        public static void Preloaded(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            WpFlyPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["White Palace Fly"]);
            SetInactive(WpFlyPrefab);
            WpSawWithSoundPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["saw_collection/wp_saw"]);
            SetInactive(WpSawWithSoundPrefab);
            WpSawNoSoundPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["saw_collection/wp_saw (2)"]);
            SetInactive(WpSawNoSoundPrefab);
            WpInfSoulTotemPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["Soul Totem white_Infinte"]);
            SetInactive(WpInfSoulTotemPrefab);
            WpSpikesPrefab = UObject.Instantiate(preloadedObjects["White_Palace_17"]["White_ Spikes"]);
            SetInactive(WpSpikesPrefab);
            WpTinkEffectPrefab = new TinkEffect();
            {
                var tmp = UObject.Instantiate(preloadedObjects["White_Palace_17"]["Cave Spikes Invis"]);
                var tmpTe = tmp.GetComponent<TinkEffect>();
                WpTinkEffectPrefab.blockEffect = tmpTe.blockEffect;
                WpTinkEffectPrefab.useNailPosition = tmpTe.useNailPosition;
                WpTinkEffectPrefab.sendFSMEvent = tmpTe.sendFSMEvent;
                WpTinkEffectPrefab.FSMEvent = tmpTe.FSMEvent;
                WpTinkEffectPrefab.fsm = tmpTe.fsm;
                WpTinkEffectPrefab.sendDirectionalFSMEvents = tmpTe.sendDirectionalFSMEvents;
            }
            PopAreaTitleCtrlPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["Area Title Controller"]);
            SetInactive(PopAreaTitleCtrlPrefab);
            PopSobPartPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["glow response lore 1/Glow Response Object (11)"]);
            SetInactive(PopSobPartPrefab);
            PopTabletInspectPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["Inspect Region"]);
            SetInactive(PopTabletInspectPrefab);
            PopSceneManagerPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["_SceneManager"]);
            {
                var sm = PopSceneManagerPrefab.GetComponent<SceneManager>();
                sm.SetAttr("musicTransitionTime", 3.0f);
            }
            SetInactive(PopSceneManagerPrefab);
            PopPmU2dPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["_Managers/PlayMaker Unity 2D"]);
            SetInactive(PopPmU2dPrefab);
            PopMusicRegionPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["Music Region (1)"]);
            SetInactive(PopMusicRegionPrefab);
            PopEnterMusicCuePrefab = UObject.Instantiate(PopMusicRegionPrefab.GetComponent<MusicRegion>().enterMusicCue);
            SetInactive(PopEnterMusicCuePrefab);
            WhiteBenchPrefab = UObject.Instantiate(preloadedObjects["White_Palace_03_hub"]["WhiteBench"]);
            SetInactive(WhiteBenchPrefab);
            WpLeverPrefab = UObject.Instantiate(preloadedObjects["White_Palace_17"]["WP Lever"]);
            {
                UObject.Destroy(WpLeverPrefab.GetComponent<PersistentBoolItem>());
            }
            SetInactive(WpLeverPrefab);
            BreakableWallPrefab = UObject.Instantiate(preloadedObjects["Crossroads_07"]["Breakable Wall_Silhouette"]);
            {
                UObject.Destroy(BreakableWallPrefab.GetComponent<PersistentBoolItem>());
            }
            SetInactive(BreakableWallPrefab);
            PopQuakeFloorPrefab = UObject.Instantiate(preloadedObjects["White_Palace_09"]["Quake Floor"]);
            {
                UObject.Destroy(PopQuakeFloorPrefab.GetComponent<PersistentBoolItem>());
                var t = PopQuakeFloorPrefab.transform.Find("Active");
                for (int c = t.childCount - 1; c >= 0; c--)
                {
                    UObject.Destroy(t.GetChild(c).gameObject);
                }
                t = PopQuakeFloorPrefab.transform.Find("Inactive");
                for (int c = t.childCount - 1; c >= 0; c--)
                {
                    UObject.Destroy(t.GetChild(c).gameObject);
                }
            }
            SetInactive(PopQuakeFloorPrefab);
            Hornet2BossEncounterPrefab = UObject.Instantiate(preloadedObjects["Deepnest_East_Hornet_boss"]["Hornet Outskirts Battle Encounter"]);
            SetInactive(Hornet2BossEncounterPrefab);
            Hornet2BossPrefab = UObject.Instantiate(preloadedObjects["Deepnest_East_Hornet_boss"]["Hornet Boss 2"]);
            SetInactive(Hornet2BossPrefab);
            ShinyPrefab = UObject.Instantiate(preloadedObjects["Grimm_Divine"]["Charm Holder"]);
            {
                UObject.Destroy(ShinyPrefab.transform.GetChild(2));
                UObject.Destroy(ShinyPrefab.transform.GetChild(1));
                UObject.Destroy(ShinyPrefab.transform.GetChild(0).gameObject.GetComponent<PersistentBoolItem>());
            }
            SetInactive(ShinyPrefab);
            Wp03Door = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["door1"]);
            SetInactive(Wp03Door);
            Wp03Dream = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["Dream Entry"]);
            SetInactive(Wp03Dream);
        }
        private static void SetInactive(GameObject go)
        {
            if (go != null)
            {
                UObject.DontDestroyOnLoad(go);
                go.SetActive(false);
            }
        }
        private static void SetInactive(UObject go)
        {
            if (go != null)
            {
                UObject.DontDestroyOnLoad(go);
            }
        }
    }
}
