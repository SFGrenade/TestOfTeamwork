using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using GlobalEnums;
using HutongGames.PlayMaker;
using On;
using Logger = Modding.Logger;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Profiling;
using ModCommon.Util;
using HutongGames.PlayMaker.Actions;
using TestOfTeamwork.MonoBehaviours;
using TestOfTeamwork.Consts;
using TestOfTeamwork.Utils;
using UObject = UnityEngine.Object;

namespace TestOfTeamwork
{
    public class SceneChanger : MonoBehaviour
    {
        private const bool _DEBUG = false;
        private const string _AB_PATH = "E:\\Github_Projects\\DreamKing Assets\\Assets\\AssetBundles\\";

        public AssetBundle AbOverallMat { get; private set; } = null;
        public AssetBundle AbTotScene { get; private set; } = null;
        public AssetBundle AbTotMat { get; private set; } = null;

        private GameObject wpFlyPrefab;
        private GameObject wpSawWithSoundPrefab;
        private GameObject wpSawNoSoundPrefab;
        private GameObject wpInfSoulTotemPrefab;
        private GameObject wpSpikesPrefab;
        private TinkEffect wpTinkEffectPrefab;
        private GameObject popAreaTitleCtrlPrefab;
        private GameObject popSobPartPrefab;
        private GameObject popTabletInspectPrefab;
        private GameObject popSceneManagerPrefab;
        private GameObject popPmU2dPrefab;
        private GameObject popMusicRegionPrefab;
        private MusicCue popEnterMusicCuePrefab;
        private GameObject popQuakeFloorPrefab;
        //private Shader popShaderPrefab;
        private GameObject whiteBenchPrefab;
        private AudioMixerSnapshot wp17AMS;
        private AudioMixerSnapshot wp19AMS;
        private AudioMixerSnapshot wp20AMS;
        private PlayMakerFSM wp20PMFSM;
        private GameObject wpLeverPrefab;
        private GameObject breakableWallPrefab;
        private GameObject hornet2BossPrefab;
        private GameObject shinyPrefab;

        private bool zaliantMusic = false;
        private Material[] blurPlaneMaterials;
        private Material sceneMapMaterial;
        private Material defaultSpriteMaterial;
        private Material litSpriteMaterial;

        /*
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
    new ValueTuple<string, string>("White_Palace_18", "BlurPlane (1)"),
    new ValueTuple<string, string>("White_Palace_17", "_SceneManager"),
    new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
    new ValueTuple<string, string>("White_Palace_17", "White_ Spikes"),
    new ValueTuple<string, string>("White_Palace_17", "Cave Spikes Invis"),
    new ValueTuple<string, string>("White_Palace_19", "_SceneManager"),
    new ValueTuple<string, string>("White_Palace_20", "_SceneManager"),
    new ValueTuple<string, string>("White_Palace_20", "Battle Scene"),
    new ValueTuple<string, string>("White_Palace_09", "Quake Floor"),
    new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
    new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette"),
    new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Outskirts Battle Encounter")
        */

        public SceneChanger(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.GameManager.RefreshTilemapInfo += OnGameManagerRefreshTilemapInfo;

            wpFlyPrefab = Instantiate(preloadedObjects["White_Palace_18"]["White Palace Fly"]);
            SetInactive(wpFlyPrefab);
            wpSawWithSoundPrefab = Instantiate(preloadedObjects["White_Palace_18"]["saw_collection/wp_saw"]);
            SetInactive(wpSawWithSoundPrefab);
            wpSawNoSoundPrefab = Instantiate(preloadedObjects["White_Palace_18"]["saw_collection/wp_saw (2)"]);
            SetInactive(wpSawNoSoundPrefab);
            wpInfSoulTotemPrefab = Instantiate(preloadedObjects["White_Palace_18"]["Soul Totem white_Infinte"]);
            SetInactive(wpInfSoulTotemPrefab);
            wpSpikesPrefab = Instantiate(preloadedObjects["White_Palace_17"]["White_ Spikes"]);
            SetInactive(wpSpikesPrefab);
            #region TinkEffect
            wpTinkEffectPrefab = new TinkEffect();
            var tmp = Instantiate(preloadedObjects["White_Palace_17"]["Cave Spikes Invis"]);
            var tmpTE = tmp.GetComponent<TinkEffect>();
            wpTinkEffectPrefab.blockEffect = tmpTE.blockEffect;
            wpTinkEffectPrefab.useNailPosition = tmpTE.useNailPosition;
            wpTinkEffectPrefab.sendFSMEvent = tmpTE.sendFSMEvent;
            wpTinkEffectPrefab.FSMEvent = tmpTE.FSMEvent;
            wpTinkEffectPrefab.fsm = tmpTE.fsm;
            wpTinkEffectPrefab.sendDirectionalFSMEvents = tmpTE.sendDirectionalFSMEvents;
            #endregion
            popAreaTitleCtrlPrefab = Instantiate(preloadedObjects["White_Palace_18"]["Area Title Controller"]);
            SetInactive(popAreaTitleCtrlPrefab);
            popSobPartPrefab = Instantiate(preloadedObjects["White_Palace_18"]["glow response lore 1/Glow Response Object (11)"]);
            SetInactive(popSobPartPrefab);
            popTabletInspectPrefab = Instantiate(preloadedObjects["White_Palace_18"]["Inspect Region"]);
            SetInactive(popTabletInspectPrefab);
            popSceneManagerPrefab = Instantiate(preloadedObjects["White_Palace_18"]["_SceneManager"]);
            SetInactive(popSceneManagerPrefab);
            popPmU2dPrefab = Instantiate(preloadedObjects["White_Palace_18"]["_Managers/PlayMaker Unity 2D"]);
            SetInactive(popPmU2dPrefab);
            popMusicRegionPrefab = Instantiate(preloadedObjects["White_Palace_18"]["Music Region (1)"]);
            SetInactive(popMusicRegionPrefab);
            popEnterMusicCuePrefab = Instantiate(popMusicRegionPrefab.GetComponent<MusicRegion>().enterMusicCue);
            SetInactive(popEnterMusicCuePrefab);
            //popShaderPrefab = Instantiate(preloadedObjects["White_Palace_18"]["BlurPlane (1)"]).GetComponent<MeshRenderer>().material.shader;
            //SetInactive(popShaderPrefab);
            whiteBenchPrefab = Instantiate(preloadedObjects["White_Palace_03_hub"]["WhiteBench"]);
            SetInactive(whiteBenchPrefab);
            wp17AMS = Instantiate(preloadedObjects["White_Palace_17"]["_SceneManager"]).GetComponent<SceneManager>().GetAttr<SceneManager, AudioMixerSnapshot>("musicSnapshot");
            SetInactive(wp17AMS);
            wp19AMS = Instantiate(preloadedObjects["White_Palace_19"]["_SceneManager"]).GetComponent<SceneManager>().GetAttr<SceneManager, AudioMixerSnapshot>("musicSnapshot");
            SetInactive(wp19AMS);
            wp20PMFSM = Instantiate(preloadedObjects["White_Palace_20"]["Battle Scene"]).LocateMyFSM("Battle Control");
            wp20AMS = Instantiate(preloadedObjects["White_Palace_20"]["_SceneManager"]).GetComponent<SceneManager>().GetAttr<SceneManager, AudioMixerSnapshot>("musicSnapshot");
            SetInactive(wp20PMFSM);
            SetInactive(wp20AMS);
            wpLeverPrefab = Instantiate(preloadedObjects["White_Palace_17"]["WP Lever"]);
            SetInactive(wpLeverPrefab);
            breakableWallPrefab = Instantiate(preloadedObjects["Crossroads_07"]["Breakable Wall_Silhouette"]);
            SetInactive(breakableWallPrefab);
            popQuakeFloorPrefab = Instantiate(preloadedObjects["White_Palace_09"]["Quake Floor"]);
            SetInactive(popQuakeFloorPrefab);
            hornet2BossPrefab = Instantiate(preloadedObjects["Deepnest_East_Hornet_boss"]["Hornet Outskirts Battle Encounter"]);
            SetInactive(hornet2BossPrefab);
            shinyPrefab = Instantiate(preloadedObjects["Grimm_Divine"]["Charm Holder"]);
            SetInactive(shinyPrefab);

            #region Memory Usage
            long allocatedBefore;
            long reservedBefore;
            long allocatedAfter;
            long reservedAfter;
            long allocatedDelta;
            long reservedDelta;
            Assembly _asm;

            allocatedBefore = Profiler.GetTotalAllocatedMemoryLong();
            reservedBefore = Profiler.GetTotalReservedMemoryLong();
            Log($"Profiler.GetTotalAllocatedMemoryLong: \"{allocatedBefore}\"");
            Log($"Profiler.GetTotalReservedMemoryLong:  \"{reservedBefore}\"");
            #endregion

            #region Load AssetBundles
#pragma warning disable CS0162 // Unerreichbarer Code wurde entdeckt.
            Log("Loading AssetBundles");
            _asm = Assembly.GetExecutingAssembly();
            if (AbOverallMat == null)
            {
                if (!_DEBUG)
                {
                    using (Stream s = _asm.GetManifestResourceStream("TestOfTeamwork.Resources.overall_materials_tot"))
                    {
                        if (s != null)
                        {
                            AbOverallMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbOverallMat = AssetBundle.LoadFromFile(_AB_PATH + "overall_materials_tot");
                }
            }
            if (AbTotScene == null)
            {
                if (!_DEBUG)
                {
                    using (Stream s = _asm.GetManifestResourceStream("TestOfTeamwork.Resources.test_of_teamwork_scenes"))
                    {
                        if (s != null)
                        {
                            AbTotScene = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbTotScene = AssetBundle.LoadFromFile(_AB_PATH + "test_of_teamwork_scenes");
                }
            }
            if (AbTotMat == null)
            {
                if (!_DEBUG)
                {
                    using (Stream s = _asm.GetManifestResourceStream("TestOfTeamwork.Resources.test_of_teamwork_materials"))
                    {
                        if (s != null)
                        {
                            AbTotMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbTotMat = AssetBundle.LoadFromFile(_AB_PATH + "test_of_teamwork_materials");
                }
            }
            Log("Finished loading AssetBundles");
#pragma warning restore CS0162 // Unerreichbarer Code wurde entdeckt.
            #endregion

            #region Memory Usage
            allocatedAfter = Profiler.GetTotalAllocatedMemoryLong();
            reservedAfter = Profiler.GetTotalReservedMemoryLong();
            Log($"Profiler.GetTotalAllocatedMemoryLong: \"{allocatedAfter}\"");
            Log($"Profiler.GetTotalReservedMemoryLong:  \"{reservedAfter}\"");

            allocatedDelta = allocatedAfter - allocatedBefore;
            reservedDelta = reservedAfter - reservedBefore;

            Log($"Delta GetTotalAllocatedMemoryLong:    \"{((allocatedDelta >= 0) ? '+' : '-')}{allocatedDelta}\"");
            Log($"Delta GetTotalReservedMemoryLong:     \"{((reservedDelta >= 0) ? '+' : '-')}{reservedDelta}\"");
            #endregion

            blurPlaneMaterials = new Material[1];
            //blurPlaneMaterials[0] = new Material(popShaderPrefab);
            blurPlaneMaterials[0] = new Material(Shader.Find("UI/Blur/UIBlur"));
            blurPlaneMaterials[0].SetColor(Shader.PropertyToID("_TintColor"), new Color(1.0f, 1.0f, 1.0f, 0.0f));
            blurPlaneMaterials[0].SetFloat(Shader.PropertyToID("_Size"), 53.7f);
            //blurPlaneMaterials[0].SetFloat(Shader.PropertyToID("_Size"), 107.4f);
            blurPlaneMaterials[0].SetFloat(Shader.PropertyToID("_Vibrancy"), 0.2f);
            //blurPlaneMaterials[0].SetFloat(Shader.PropertyToID("_Vibrancy"), 1.0f);
            blurPlaneMaterials[0].SetInt(Shader.PropertyToID("_StencilComp"), 8);
            blurPlaneMaterials[0].SetInt(Shader.PropertyToID("_Stencil"), 0);
            blurPlaneMaterials[0].SetInt(Shader.PropertyToID("_StencilOp"), 0);
            blurPlaneMaterials[0].SetInt(Shader.PropertyToID("_StencilWriteMask"), 255);
            blurPlaneMaterials[0].SetInt(Shader.PropertyToID("_StencilReadMask"), 255);
            sceneMapMaterial = new Material(Shader.Find("tk2d/BlendVertexColor"));
            defaultSpriteMaterial = new Material(Shader.Find("Sprites/Default"));
            defaultSpriteMaterial.SetColor(Shader.PropertyToID("_Color"), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            defaultSpriteMaterial.SetInt(Shader.PropertyToID("PixelSnap"), 0);
            defaultSpriteMaterial.SetFloat(Shader.PropertyToID("_EnableExternalAlpha"), 0.0f);
            defaultSpriteMaterial.SetInt(Shader.PropertyToID("_StencilComp"), 8);
            defaultSpriteMaterial.SetInt(Shader.PropertyToID("_Stencil"), 0);
            defaultSpriteMaterial.SetInt(Shader.PropertyToID("_StencilOp"), 0);
            defaultSpriteMaterial.SetInt(Shader.PropertyToID("_StencilWriteMask"), 255);
            defaultSpriteMaterial.SetInt(Shader.PropertyToID("_StencilReadMask"), 255);
            litSpriteMaterial = new Material(Shader.Find("Sprites/Lit"));
            litSpriteMaterial.SetColor(Shader.PropertyToID("_Color"), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            litSpriteMaterial.SetInt(Shader.PropertyToID("PixelSnap"), 0);
            litSpriteMaterial.SetFloat(Shader.PropertyToID("_EnableExternalAlpha"), 0.0f);
            litSpriteMaterial.SetInt(Shader.PropertyToID("_StencilComp"), 8);
            litSpriteMaterial.SetInt(Shader.PropertyToID("_Stencil"), 0);
            litSpriteMaterial.SetInt(Shader.PropertyToID("_StencilOp"), 0);
            litSpriteMaterial.SetInt(Shader.PropertyToID("_StencilWriteMask"), 255);
            litSpriteMaterial.SetInt(Shader.PropertyToID("_StencilReadMask"), 255);
        }

        private void OnGameManagerRefreshTilemapInfo(On.GameManager.orig_RefreshTilemapInfo orig, GameManager self, string targetScene)
        {
            orig(self, targetScene);
            if (targetScene == TransitionGateNames.Tot01)
            {
                float width = 192;
                float height = 64;
                self.tilemap.width = (int)width;
                self.tilemap.height = (int)height;
                self.sceneWidth = width;
                self.sceneHeight = height;
                FindObjectOfType<GameMap>().SetManualTilemap(0, 0, width, height);
            }
            else if (targetScene == TransitionGateNames.Tot02)
            {
                float width = 128;
                float height = 64;
                self.tilemap.width = (int)width;
                self.tilemap.height = (int)height;
                self.sceneWidth = width;
                self.sceneHeight = height;
                FindObjectOfType<GameMap>().SetManualTilemap(0, 0, width, height);
            }
            else if (targetScene == TransitionGateNames.Tot03)
            {
                float width = 96;
                float height = 96;
                self.tilemap.width = (int)width;
                self.tilemap.height = (int)height;
                self.sceneWidth = width;
                self.sceneHeight = height;
                FindObjectOfType<GameMap>().SetManualTilemap(0, 0, width, height);
            }
            else if (targetScene == TransitionGateNames.TotEndless)
            {
                float width = 32;
                float height = 32;
                self.tilemap.width = (int)width;
                self.tilemap.height = (int)height;
                self.sceneWidth = width;
                self.sceneHeight = height;
                FindObjectOfType<GameMap>().SetManualTilemap(0, 0, width, height);
            }
        }

        public void CR_Change_Room_temple(Scene scene)
        {
            if (scene.name != "Room_temple")
                return;

            Log("CR_Change_Room_temple()");
            //yield return null;

            #region Hornet NPC FSM

            GameObject hornetNpcGo = scene.FindRoot("Hornet Black Egg NPC");
            PlayMakerFSM hornetNpcFsm = hornetNpcGo.LocateMyFSM("Conversation Control");
            FsmVariables hornetNpcFsmVar = hornetNpcFsm.FsmVariables;

            hornetNpcFsm.CopyState("Greet", "Give Item");
            hornetNpcFsm.RemoveAction("Give Item", 0);

            hornetNpcFsm.GetAction<CallMethodProper>("Give Item", 0).parameters = new FsmVar[]
            {
                new FsmVar(typeof(string)) { stringValue = "CUSTOM_HORNET_PRE_FINAL_BATTLE" },
                new FsmVar(typeof(string)) { stringValue = "Hornet" }
            };

            var pdbtAction = new PlayerDataBoolTest();
            pdbtAction.gameObject = hornetNpcFsm.GetAction<PlayerDataBoolTest>("Choice", 1).gameObject;
            pdbtAction.boolName = nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkHornetCompanion);
            pdbtAction.isFalse = FsmEvent.GetFsmEvent("ABSENT");

            hornetNpcFsm.InsertAction("Choice", pdbtAction, 1);

            hornetNpcFsm.AddTransition("Choice", "ABSENT", "Give Item", false);

            #endregion

            #region Shiny FSM

            GameObject shinyParent = GameObject.Instantiate(shinyPrefab);
            shinyParent.name = "Necklace";
            shinyParent.SetActive(false);
            shinyParent.transform.GetChild(0).gameObject.SetActive(true);
            shinyParent.transform.position = new Vector3(29.0f, 4.3f, 0.0f);
            GameObject.Destroy(shinyParent.transform.GetChild(2));
            GameObject.Destroy(shinyParent.transform.GetChild(1));

            hornetNpcFsm.CopyState("Box Up", "Give Item Spawn");

            hornetNpcFsm.ChangeTransition("Give Item Spawn", FsmEvent.Finished.Name, "Talk Finish");
            hornetNpcFsm.GetState("Give Item").Transitions.First(x => x.ToState == "Talk Finish").ToState = "Give Item Spawn";

            hornetNpcFsm.RemoveAction("Give Item Spawn", 5);
            hornetNpcFsm.RemoveAction("Give Item Spawn", 4);
            hornetNpcFsm.RemoveAction("Give Item Spawn", 3);
            hornetNpcFsm.RemoveAction("Give Item Spawn", 2);
            hornetNpcFsm.RemoveAction("Give Item Spawn", 1);
            hornetNpcFsm.RemoveAction("Give Item Spawn", 0);

            var agoAction = new ActivateGameObject();
            agoAction.gameObject = new FsmOwnerDefault();
            agoAction.gameObject.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
            agoAction.gameObject.GameObject = shinyParent;
            agoAction.activate = true;
            agoAction.recursive = false;
            agoAction.resetOnExit = false;
            agoAction.everyFrame = false;
            hornetNpcFsm.AddAction("Give Item Spawn", agoAction);
            hornetNpcFsm.AddAction("Give Item Spawn", new NextFrameEvent() { sendEvent = FsmEvent.Finished });

            PlayMakerFSM shinyFsm = shinyParent.transform.GetChild(0).gameObject.LocateMyFSM("Shiny Control");
            FsmVariables shinyFsmVars = shinyFsm.FsmVariables;
            shinyFsmVars.FindFsmInt("Charm ID").Value = 0;
            shinyFsmVars.FindFsmInt("Type").Value = 0;
            shinyFsmVars.FindFsmBool("Activated").Value = false;
            shinyFsmVars.FindFsmBool("Charm").Value = false;
            shinyFsmVars.FindFsmBool("Dash Cloak").Value = false;
            shinyFsmVars.FindFsmBool("Exit Dream").Value = false;
            shinyFsmVars.FindFsmBool("Fling L").Value = false;
            shinyFsmVars.FindFsmBool("Fling On Start").Value = true;
            shinyFsmVars.FindFsmBool("Journal").Value = false;
            shinyFsmVars.FindFsmBool("King's Brand").Value = false;
            shinyFsmVars.FindFsmBool("Mantis Claw").Value = false;
            shinyFsmVars.FindFsmBool("Pure Seed").Value = false;
            shinyFsmVars.FindFsmBool("Quake").Value = false;
            shinyFsmVars.FindFsmBool("Show Charm Tute").Value = false;
            shinyFsmVars.FindFsmBool("Slug Fling").Value = false;
            shinyFsmVars.FindFsmBool("Super Dash").Value = false;
            shinyFsmVars.FindFsmString("Item Name").Value = LanguageStrings.HornetInvNameKey;
            shinyFsmVars.FindFsmString("PD Bool Name").Value = nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkHornetCompanion);

            IntSwitch isAction = shinyFsm.GetAction<IntSwitch>("Trinket Type", 0);
            var tmpCompareTo = new List<FsmInt>(isAction.compareTo);
            tmpCompareTo.Add(tmpCompareTo.Count + 1);
            isAction.compareTo = tmpCompareTo.ToArray();
            shinyFsmVars.FindFsmInt("Trinket Num").Value = tmpCompareTo.Count;
            var tmpSendEvent = new List<FsmEvent>(isAction.sendEvent);
            tmpSendEvent.Add(FsmEvent.FindEvent("PURE SEED"));
            isAction.sendEvent = tmpSendEvent.ToArray();

            shinyFsm.CopyState("Love Key", "Necklace");

            shinyFsm.GetAction<SetPlayerDataBool>("Necklace", 0).boolName = nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkHornetCompanion);
            shinyFsm.GetAction<SetSpriteRendererSprite>("Necklace", 1).sprite = TestOfTeamwork.Instance.SpriteDict.Get(TextureStrings.InvHornetKey);
            shinyFsm.GetAction<GetLanguageString>("Necklace", 2).convName = LanguageStrings.HornetInvNameKey;

            shinyFsm.AddTransition("Trinket Type", "PURE SEED", "Necklace", false);

            #endregion

            Log("~CR_Change_Room_temple()");
        }

        public void CR_Change_White_Palace_06(Scene scene)
        {
            if (scene.name != "White_Palace_06")
                return;

            Log("CR_Change_White_Palace_06()");
            //yield return null;

            var endless = UnityEngine.Random.Range(0.0f, 1.0f) < 0.001f;
            if (!endless)
            {
                CreateGateway(TransitionGateNames.Wp06Tot01, new Vector2(50.5f, 8), new Vector2(1, 4), TransitionGateNames.Tot01, TransitionGateNames.Tot01Wp06,
                              new Vector2(-3, 0), false, true, false, GameManager.SceneLoadVisualizations.Default);
            }
            else
            {
                CreateGateway(TransitionGateNames.Wp06Tot01, new Vector2(50.5f, 8), new Vector2(1, 4), TransitionGateNames.TotEndless, TransitionGateNames.Tot01Wp06,
                              new Vector2(-3, 0), false, true, false, GameManager.SceneLoadVisualizations.Default);
            }
            CreateBreakableWall(scene.name, "SF_ToT_Breakable_Wall_ToT", new Vector3(30.0f, 8.0f, 0.05f), Vector3.zero, Vector3.one, new Vector2(6.0f, 4.0f), nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkTotOpened));
            //yield return null;

            zaliantMusic = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;
            //Log("Zaliant? " + zaliantMusic.ToString());

            #region Edit White_Palace_06 for new Transition
            GameObject sceneMap = null;
            GameObject chunk00 = null;
            GameObject chunk01 = null;
            foreach (GameObject rgo in scene.GetRootGameObjects())
            {
                if (rgo.name == "TileMap Render Data")
                {
                    sceneMap = rgo.transform.Find("Scenemap").gameObject;

                    chunk00 = sceneMap.transform.Find("Chunk 0 0").gameObject;
                    chunk01 = sceneMap.transform.Find("Chunk 0 1").gameObject;
                }
            }
            //Log("Found Chunks to edit");
            #region Chunk 0 0
            EdgeCollider2D[] edgeCollider2Ds = chunk00.GetComponents<EdgeCollider2D>();
            //Log("Found " + edgeCollider2Ds.Length + " EdgeCollider2D(s)");
            Vector2[] changedPoints = new Vector2[edgeCollider2Ds[2].points.Length];
            Array.Copy(edgeCollider2Ds[2].points, 0, changedPoints, 0, edgeCollider2Ds[2].points.Length);

            changedPoints[3].y = 6; // before: 32 - after: 6
            changedPoints[4].y = 6; // before: 32 - after: 6
            edgeCollider2Ds[2].points = changedPoints;

            //Log("Edited EdgeCollider2D, adding new one");
            chunk00.AddComponent<EdgeCollider2D>();
            edgeCollider2Ds = chunk00.GetComponents<EdgeCollider2D>();

            //Log("Constructing new Point Array");
            Vector2[] points = new Vector2[5];
            points[0] = new Vector2(27, 10);
            points[1] = new Vector2(32, 10);
            points[2] = new Vector2(32, 32);
            points[3] = new Vector2(27, 32);
            points[4] = new Vector2(27, 10);

            edgeCollider2Ds[3].edgeRadius = edgeCollider2Ds[2].edgeRadius;
            edgeCollider2Ds[3].points = points;
            #endregion

            #region Chunk 0 1
            edgeCollider2Ds = chunk01.GetComponents<EdgeCollider2D>();
            //Log("Found " + edgeCollider2Ds.Length + " EdgeCollider2D(s)");

            changedPoints = new Vector2[edgeCollider2Ds[0].points.Length];
            Array.Copy(edgeCollider2Ds[0].points, 0, changedPoints, 0, edgeCollider2Ds[0].points.Length);
            changedPoints[0].y = 10; // before: 1 - after: 10
            changedPoints[1].y = 10; // before: 0 - after: 10
            changedPoints[2].y = 10; // before: 0 - after: 10
            changedPoints[5].y = 10; // before: 1 - after: 10
            edgeCollider2Ds[0].points = changedPoints;

            //Log("Edited EdgeCollider2D, adding new one");
            chunk01.AddComponent<EdgeCollider2D>();
            edgeCollider2Ds = chunk01.GetComponents<EdgeCollider2D>();
            //Log("Constructing new Point Array");
            points = new Vector2[5];
            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(18, 0);
            points[2] = new Vector2(18, 6);
            points[3] = new Vector2(0, 6);
            points[4] = new Vector2(0, 0);
            edgeCollider2Ds[1].edgeRadius = edgeCollider2Ds[0].edgeRadius;
            edgeCollider2Ds[1].points = points;
            #endregion
            #endregion

            Log("CR_Change_White_Palace_06 Done");
        }

        public void CR_Change_ToT01(Scene scene)
        {
            Log("CR_Change_ToT01()");

            // left towards wp06
            CreateGateway(TransitionGateNames.Tot01Wp06, new Vector2(-0.5f, 14), new Vector2(1, 4), TransitionGateNames.Wp06, TransitionGateNames.Wp06Tot01,
                          new Vector2(16, 0), true, false, false, GameManager.SceneLoadVisualizations.Default);
            // up towards tot02
            CreateGateway(TransitionGateNames.Tot01Tot02Shortcut, new Vector2(42, 64.5f), new Vector2(4, 1), TransitionGateNames.Tot02, TransitionGateNames.Tot02Tot01Shortcut,
                          new Vector2(0, -5), false, true, false, GameManager.SceneLoadVisualizations.Default);
            // up towards tot03
            CreateGateway(TransitionGateNames.Tot01Tot03, new Vector2(171, 64.5f), new Vector2(4, 1), TransitionGateNames.Tot03, TransitionGateNames.Tot03Tot01,
                          new Vector2(0, -5), true, false, false, GameManager.SceneLoadVisualizations.Default);
            //yield return null;

            PatchMisc(scene);
            PatchTotLever(scene);
            PatchBlocker(scene);

            //GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchTotLoreElements(scene));
            //GameManager.instance.StartCoroutine(PatchBlurPlane(scene));
            //GameManager.instance.StartCoroutine(PatchDamageResetColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHazardRespawnTrigger(scene));
            //GameManager.instance.StartCoroutine(PatchCameraLockAreas(scene));
            //GameManager.instance.StartCoroutine(PatchEnemies(scene));
            //GameManager.instance.StartCoroutine(PatchHornetPickupColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHornetRefreshColliders(scene));
            //GameManager.instance.StartCoroutine(PatchTotMusicRegions(scene));
            GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            PatchTotLoreElements(scene);
            PatchBlurPlane(scene);
            PatchDamageResetColliders(scene);
            PatchHazardRespawnTrigger(scene);
            PatchCameraLockAreas(scene);
            PatchEnemies(scene);
            PatchHornetPickupColliders(scene);
            PatchHornetRefreshColliders(scene);
            PatchTotMusicRegions(scene);
        }

        public void CR_Change_ToT02(Scene scene)
        {
            Log("CR_Change_ToT02()");

            // down towards tot01
            CreateGateway(TransitionGateNames.Tot02Tot01Shortcut, new Vector2(42, -0.5f), new Vector2(4, 1), TransitionGateNames.Tot01, TransitionGateNames.Tot01Tot02Shortcut,
                          new Vector2(3, 4), true, false, false, GameManager.SceneLoadVisualizations.Default);
            // right towards tot03
            CreateGateway(TransitionGateNames.Tot02Tot03, new Vector2(128.5f, 17), new Vector2(1, 12), TransitionGateNames.Tot03, TransitionGateNames.Tot03Tot02,
                          new Vector2(-8.5f, 0), false, true, false, GameManager.SceneLoadVisualizations.Default);
            //yield return null;

            PatchMisc(scene);
            PatchTotLever(scene);
            PatchBlocker(scene);
            PatchBoss(scene);

            //GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchTotLoreElements(scene));
            //GameManager.instance.StartCoroutine(PatchBlurPlane(scene));
            //GameManager.instance.StartCoroutine(PatchDamageResetColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHazardRespawnTrigger(scene));
            //GameManager.instance.StartCoroutine(PatchCameraLockAreas(scene));
            //GameManager.instance.StartCoroutine(PatchEnemies(scene));
            //GameManager.instance.StartCoroutine(PatchHornetPickupColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHornetRefreshColliders(scene));
            //GameManager.instance.StartCoroutine(PatchTotMusicRegions(scene));
            GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            PatchTotLoreElements(scene);
            PatchBlurPlane(scene);
            PatchDamageResetColliders(scene);
            PatchHazardRespawnTrigger(scene);
            PatchCameraLockAreas(scene);
            PatchEnemies(scene);
            PatchHornetPickupColliders(scene);
            PatchHornetRefreshColliders(scene);
            PatchTotMusicRegions(scene);
        }

        public void CR_Change_ToT03(Scene scene)
        {
            Log("CR_Change_ToT03()");

            // down towards tot01
            CreateGateway(TransitionGateNames.Tot03Tot01, new Vector2(43, -0.5f), new Vector2(4, 1), TransitionGateNames.Tot01, TransitionGateNames.Tot01Tot03,
                          new Vector2(3, 4), true, false, false, GameManager.SceneLoadVisualizations.Default);
            // left towards tot02
            CreateGateway(TransitionGateNames.Tot03Tot02, new Vector2(-0.5f, 17), new Vector2(1, 12), TransitionGateNames.Tot02, TransitionGateNames.Tot02Tot03,
                          new Vector2(8.5f, 0), true, false, false, GameManager.SceneLoadVisualizations.Default);
            //yield return null;

            PatchMisc(scene);
            PatchTotLever(scene);
            PatchBlocker(scene);

            //GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchTotLoreElements(scene));
            //GameManager.instance.StartCoroutine(PatchBlurPlane(scene));
            //GameManager.instance.StartCoroutine(PatchDamageResetColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHazardRespawnTrigger(scene));
            //GameManager.instance.StartCoroutine(PatchCameraLockAreas(scene));
            //GameManager.instance.StartCoroutine(PatchEnemies(scene));
            //GameManager.instance.StartCoroutine(PatchHornetPickupColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHornetRefreshColliders(scene));
            //GameManager.instance.StartCoroutine(PatchTotMusicRegions(scene));
            GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            PatchTotLoreElements(scene);
            PatchBlurPlane(scene);
            PatchDamageResetColliders(scene);
            PatchHazardRespawnTrigger(scene);
            PatchCameraLockAreas(scene);
            PatchEnemies(scene);
            PatchHornetPickupColliders(scene);
            PatchHornetRefreshColliders(scene);
            PatchTotMusicRegions(scene);
        }

        public void CR_Change_ToTEndless(Scene scene)
        {
            Log("CR_Change_ToTEndless()");

            // left towards wp06
            CreateGateway(TransitionGateNames.Tot01Wp06, new Vector2(-0.5f, 14), new Vector2(1, 4), TransitionGateNames.Wp06, TransitionGateNames.Wp06Tot01,
                          new Vector2(16, 0), true, false, false, GameManager.SceneLoadVisualizations.Default);
            //yield return null;

            PatchMisc(scene);
            PatchTotLever(scene);
            PatchBlocker(scene);

            //GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            //GameManager.instance.StartCoroutine(PatchTotLoreElements(scene));
            //GameManager.instance.StartCoroutine(PatchBlurPlane(scene));
            //GameManager.instance.StartCoroutine(PatchDamageResetColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHazardRespawnTrigger(scene));
            //GameManager.instance.StartCoroutine(PatchCameraLockAreas(scene));
            //GameManager.instance.StartCoroutine(PatchEnemies(scene));
            //GameManager.instance.StartCoroutine(PatchHornetPickupColliders(scene));
            //GameManager.instance.StartCoroutine(PatchHornetRefreshColliders(scene));
            //GameManager.instance.StartCoroutine(PatchTotMusicRegions(scene));
            GameManager.instance.StartCoroutine(PatchLitSpriteMaterials(scene));
            GameManager.instance.StartCoroutine(PatchDefaultSpriteMaterials(scene));
            PatchTotLoreElements(scene);
            PatchBlurPlane(scene);
            PatchDamageResetColliders(scene);
            PatchHazardRespawnTrigger(scene);
            PatchCameraLockAreas(scene);
            PatchEnemies(scene);
            PatchHornetPickupColliders(scene);
            PatchHornetRefreshColliders(scene);
            PatchTotMusicRegions(scene);

            HeroController.instance.gameObject.AddComponent<SceneExpander>();
            var tmp = HeroController.instance.gameObject.GetComponent<SceneExpander>();
            tmp.scene = scene;
            tmp.expandingAction = ExpandEndless;
        }
        private void ExpandEndless(Scene scene, int total)
        {
            var part = Instantiate(scene.Find($"Part {total - 1}"));
            part.transform.SetPosition2D(32 * total, 0);
            part.name = $"Part {total}";
        }

        private void PatchMisc(Scene scene)
        {
            Log("!Misc");
            #region Area Title Controller
            GameObject tmpPMU2D = GameObject.Instantiate(popPmU2dPrefab, scene.GetRootGameObjects()[6].transform);
            tmpPMU2D.SetActive(true);
            tmpPMU2D.name = "PlayMaker Unity 2D";
            if (scene.name == TransitionGateNames.Tot01)
            {
                GameObject atc = GameObject.Instantiate(popAreaTitleCtrlPrefab);
                atc.SetActive(true);
                atc.transform.localPosition = Vector3.zero;
                atc.transform.localEulerAngles = Vector3.zero;
                atc.transform.localScale = Vector3.one;

                PlayMakerFSM atcFsm = atc.GetComponent<PlayMakerFSM>();
                atcFsm.FsmVariables.GetFsmFloat("Unvisited Pause").Value = 3f;
                atcFsm.FsmVariables.GetFsmFloat("Visited Pause").Value = 3f;

                atcFsm.FsmVariables.GetFsmBool("Always Visited").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Display Right").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Only On Revisit").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Sub Area").Value = true;
                atcFsm.FsmVariables.GetFsmBool("Visited Area").Value = TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkVisitedTestOfTeamwork;
                atcFsm.FsmVariables.GetFsmBool("Wait for Trigger").Value = false;

                atcFsm.FsmVariables.GetFsmString("Area Event").Value = Language.Language.Get("TotAreaTitle_Event", "MISC");
                atcFsm.FsmVariables.GetFsmString("Visited Bool").Value = nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkVisitedTestOfTeamwork);

                atcFsm.FsmVariables.GetFsmGameObject("Area Title").Value = GameObject.Find("Area Title");

                atcFsm.SendEvent("DISPLAY");

                atc.AddComponent<NonBouncer>();

                TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkVisitedTestOfTeamwork = true;
            }
            #endregion

            #region Scene Manager
            if (scene.name.StartsWith(TransitionGateNames.Tot))
            {
                GameObject tmp = GameObject.Instantiate(popSceneManagerPrefab);
                tmp.name = "_SceneManager";
                tmp.SetActive(true);

                var sm = tmp.GetComponent<SceneManager>();
                sm.SetAttr("musicTransitionTime", 3.0f);
                sm.environmentType = 7;
            }
            #endregion
            Log("~Misc");
        }

        private /*IEnumerator*/ void PatchBlurPlane(Scene scene)
        {
            //yield return null;
            Log("!BlurPlane");

            GameObject plane = scene.Find("BlurPlane");
            //plane.GetComponent<MeshRenderer>().material = new Material(Shader.Find("UI/Blur/UIBlur"));
            var bp = plane.AddComponent<BlurPlane>();
            var mr = bp.gameObject.GetComponent<MeshRenderer>();
            mr.materials = blurPlaneMaterials;
            mr.material = blurPlaneMaterials[0];
            bp.SetPlaneMaterial(mr.materials[0]);
            bp.SetPlaneVisibility(true);

            sceneMapMaterial.SetTexture(Shader.PropertyToID("_MainTex"), AbOverallMat.LoadAsset<Texture2D>("Black Tile tot") as Texture);
            GameObject scenemap = scene.Find("Scenemap");
            foreach (var cMr in scenemap.GetComponentsInChildren<MeshRenderer>(false))
            {
                cMr.material = sceneMapMaterial;
            }
            //for (int i = 0; i < scenemap.transform.childCount; i++)
            //{
            //    GameObject tmpGo = scenemap.transform.GetChild(i).gameObject;
            //    if (tmpGo.activeInHierarchy)
            //    {
            //        tmpGo.GetComponent<MeshRenderer>().material = sceneMapMaterial;
            //    }
            //}

            Log("~BlurPlane");
        }

        private IEnumerator PatchLitSpriteMaterials(Scene scene, float scale)
        {
            yield return null;
            Log("!Lit Sprites");

            string[] sprites_lit = new string[] {
                "_Enemies",
                "Wall_Middle",
                "Wall_Decoration",
                "Seal_Of_Binding_Wall",
                "BlockerList"
            };
            int i = 0;
            foreach (var str_lit in sprites_lit)
            {
                GameObject parent = scene.Find(str_lit);
                if ((parent != null) && parent.activeInHierarchy)
                {
                    foreach (SpriteRenderer sr in parent.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sr.gameObject.transform.localScale = sr.gameObject.transform.localScale * scale;
                        if (sr.gameObject.name.Contains("deepnest_fog_02"))
                        {
                            sr.gameObject.transform.localScale = sr.gameObject.transform.localScale * 4;
                        }
                        sr.material = litSpriteMaterial;
                        if ((++i) >= 100)
                        {
                            i = 0;
                            yield return null;
                        }
                    }
                }
            }
            yield return null;
            Log("~Lit Sprites");
        }
        private IEnumerator PatchLitSpriteMaterials(Scene scene)
        {
            return PatchLitSpriteMaterials(scene, 1.0f);
        }
        private IEnumerator PatchDefaultSpriteMaterials(Scene scene, float scale)
        {
            yield return null;
            Log("!Default Sprites");

            string[] sprites_default = new string[] {
                "Clouds",
                "level370_Clouds",
                "Damage Colliders",
                "Hornet Pickup Colliders",
                "Hornet Refresh Colliders",
                "Organ_Wall",
                "Thorns"
            };
            int i = 0;
            foreach (var str_def in sprites_default)
            {
                GameObject parent = scene.Find(str_def);
                if ((parent != null) && parent.activeInHierarchy)
                {
                    foreach (SpriteRenderer sr in parent.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sr.gameObject.transform.localScale = sr.gameObject.transform.localScale * scale;
                        sr.material = defaultSpriteMaterial;
                        if ((++i) >= 100)
                        {
                            i = 0;
                            yield return null;
                        }
                    }
                }
            }
            yield return null;
            Log("~Default Sprites");
        }
        private IEnumerator PatchDefaultSpriteMaterials(Scene scene)
        {
            return PatchDefaultSpriteMaterials(scene, 1.0f);
        }

        private /*IEnumerator*/ void PatchTotLoreElements(Scene scene)
        {
            if (!scene.name.Contains(TransitionGateNames.Tot))
                return; //yield break;

            //yield return null;
            Log("!Seal of Binding");

            #region Seal of Binding
            GameObject parent = scene.Find("Seal_Of_Binding_Symbol");
            GameObject gro;
            GlowResponse gr;
            if ((parent != null) && parent.activeInHierarchy)
            {
                try
                {
                    foreach (SpriteRenderer sr in parent.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if ((sr.gameObject != null) && sr.gameObject.activeInHierarchy)
                        {
                            sr.material = defaultSpriteMaterial;
                        }
                    }
                    popSobPartPrefab.SetActive(true);
                    ParticleSystemRenderer tmpPSR = popSobPartPrefab.GetComponentInChildren<ParticleSystemRenderer>();
                    foreach (ParticleSystemRenderer psr in parent.GetComponentsInChildren<ParticleSystemRenderer>())
                    {
                        if ((psr.gameObject != null) && psr.gameObject.activeInHierarchy)
                        {
                            //// ToDo need to make particles round
                            //psr.material = loreMaterial;
                            // Renderer stuff
                            psr.sharedMaterial = tmpPSR.sharedMaterial;
                            psr.material = tmpPSR.material;
                            psr.realtimeLightmapScaleOffset = tmpPSR.realtimeLightmapScaleOffset;
                            psr.lightmapScaleOffset = tmpPSR.lightmapScaleOffset;
                            psr.realtimeLightmapIndex = tmpPSR.realtimeLightmapIndex;
                            psr.lightmapIndex = tmpPSR.lightmapIndex;
                            psr.probeAnchor = tmpPSR.probeAnchor;
                            psr.lightProbeProxyVolumeOverride = tmpPSR.lightProbeProxyVolumeOverride;
                            psr.allowOcclusionWhenDynamic = tmpPSR.allowOcclusionWhenDynamic;
                            psr.sortingOrder = tmpPSR.sortingOrder;
                            psr.sortingLayerID = tmpPSR.sortingLayerID;
                            psr.sortingLayerName = tmpPSR.sortingLayerName;
                            psr.reflectionProbeUsage = tmpPSR.reflectionProbeUsage;
                            psr.lightProbeUsage = tmpPSR.lightProbeUsage;
                            psr.motionVectorGenerationMode = tmpPSR.motionVectorGenerationMode;
                            psr.receiveShadows = tmpPSR.receiveShadows;
                            psr.shadowCastingMode = tmpPSR.shadowCastingMode;
                            psr.enabled = tmpPSR.enabled;
                            psr.materials = tmpPSR.materials;
                            psr.sharedMaterials = tmpPSR.sharedMaterials;
                            // ParticleSystemRenderer stuff
                            psr.trailMaterial = tmpPSR.trailMaterial;
                            psr.mesh = tmpPSR.mesh;
                            psr.maxParticleSize = tmpPSR.maxParticleSize;
                            psr.minParticleSize = tmpPSR.minParticleSize;
                            psr.sortingFudge = tmpPSR.sortingFudge;
                            psr.sortMode = tmpPSR.sortMode;
                            psr.pivot = tmpPSR.pivot;
                            psr.alignment = tmpPSR.alignment;
                            psr.normalDirection = tmpPSR.normalDirection;
                            psr.cameraVelocityScale = tmpPSR.cameraVelocityScale;
                            psr.velocityScale = tmpPSR.velocityScale;
                            psr.lengthScale = tmpPSR.lengthScale;
                            psr.renderMode = tmpPSR.renderMode;
                            psr.maskInteraction = tmpPSR.maskInteraction;
                        }
                    }
                    GlowResponse tmpGR = popSobPartPrefab.GetComponentInChildren<GlowResponse>();
                    gro = parent.transform.GetChild(0).gameObject;
                    if ((gro != null) && gro.activeInHierarchy)
                    {
                        gr = gro.AddComponent<GlowResponse>();
                        gr = gro.GetComponent<GlowResponse>();
                        gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                        gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                        gr.fadeTime = tmpGR.fadeTime;
                        gr.light = tmpGR.light;
                        gr.audioPlayerPrefab = tmpGR.audioPlayerPrefab;
                        gr.soundEffect = tmpGR.soundEffect;
                    }
                    gro = parent.transform.GetChild(1).gameObject;
                    if ((gro != null) && gro.activeInHierarchy)
                    {
                        gr = gro.AddComponent<GlowResponse>();
                        gr = gro.GetComponent<GlowResponse>();
                        gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                        gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                        gr.fadeTime = tmpGR.fadeTime;
                        gr.light = tmpGR.light;
                        gr.audioPlayerPrefab = tmpGR.audioPlayerPrefab;
                        gr.soundEffect = tmpGR.soundEffect;
                    }
                    popSobPartPrefab.SetActive(false);
                }
                catch (Exception e)
                {
                    Log("PatchToT1SealOfBinding - " + e.ToString());
                }
            }
            #endregion
            #region Lore Tablet
            parent = null;
            parent = scene.Find("LoreTablet");
            gro = null;
            //popTabletInspectPrefab
            if ((parent != null) && parent.activeInHierarchy)
            {
                try
                {
                    foreach (SpriteRenderer sr in parent.transform.Find("WP_Lore_Tablet").gameObject.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if ((sr.gameObject != null) && sr.gameObject.activeInHierarchy)
                        {
                            sr.material = defaultSpriteMaterial;
                        }
                    }
                    foreach (SpriteRenderer sr in parent.transform.Find("WP_Lore_Tablet_Plateau").gameObject.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if ((sr.gameObject != null) && sr.gameObject.activeInHierarchy)
                        {
                            sr.material = litSpriteMaterial;
                        }
                    }
                    popTabletInspectPrefab.SetActive(true);
                    GameObject inspect = GameObject.Instantiate(popTabletInspectPrefab, parent.transform);
                    popTabletInspectPrefab.SetActive(false);
                    inspect.transform.localPosition = Vector3.zero;
                    inspect.transform.localEulerAngles = Vector3.zero;
                    inspect.transform.localScale = Vector3.one;
                    inspect.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(0, -1.6875f);
                    inspect.GetComponentInChildren<BoxCollider2D>().size = new Vector2(4, 0.625f);

                    inspect.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmString("Game Text Convo").Value = LanguageStrings.LoreTabletTextKey;
                    inspect.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("Hero Always Left").Value = true;
                    inspect.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("Hero Always Right").Value = false;
                }
                catch (Exception e)
                {
                    Log("PatchToT1SealOfBinding - " + e.ToString());
                }
            }
            #endregion
            #region Credits Binding
            parent = null;
            parent = scene.Find("CreditsTablet");
            gro = null;
            if ((parent != null) && parent.activeInHierarchy)
            {
                try
                {
                    foreach (SpriteRenderer sr in parent.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if ((sr.gameObject != null) && sr.gameObject.activeInHierarchy)
                        {
                            sr.material = defaultSpriteMaterial;
                        }
                    }
                    popSobPartPrefab.SetActive(true);
                    ParticleSystemRenderer tmpPSR = popSobPartPrefab.GetComponentInChildren<ParticleSystemRenderer>();
                    foreach (ParticleSystemRenderer psr in parent.GetComponentsInChildren<ParticleSystemRenderer>())
                    {
                        if ((psr.gameObject != null) && psr.gameObject.activeInHierarchy)
                        {
                            //// ToDo need to make particles round
                            //psr.material = loreMaterial;
                            // Renderer stuff
                            psr.sharedMaterial = tmpPSR.sharedMaterial;
                            psr.material = tmpPSR.material;
                            psr.realtimeLightmapScaleOffset = tmpPSR.realtimeLightmapScaleOffset;
                            psr.lightmapScaleOffset = tmpPSR.lightmapScaleOffset;
                            psr.realtimeLightmapIndex = tmpPSR.realtimeLightmapIndex;
                            psr.lightmapIndex = tmpPSR.lightmapIndex;
                            psr.probeAnchor = tmpPSR.probeAnchor;
                            psr.lightProbeProxyVolumeOverride = tmpPSR.lightProbeProxyVolumeOverride;
                            psr.allowOcclusionWhenDynamic = tmpPSR.allowOcclusionWhenDynamic;
                            psr.sortingOrder = tmpPSR.sortingOrder;
                            psr.sortingLayerID = tmpPSR.sortingLayerID;
                            psr.sortingLayerName = tmpPSR.sortingLayerName;
                            psr.reflectionProbeUsage = tmpPSR.reflectionProbeUsage;
                            psr.lightProbeUsage = tmpPSR.lightProbeUsage;
                            psr.motionVectorGenerationMode = tmpPSR.motionVectorGenerationMode;
                            psr.receiveShadows = tmpPSR.receiveShadows;
                            psr.shadowCastingMode = tmpPSR.shadowCastingMode;
                            psr.enabled = tmpPSR.enabled;
                            psr.materials = tmpPSR.materials;
                            psr.sharedMaterials = tmpPSR.sharedMaterials;
                            // ParticleSystemRenderer stuff
                            psr.trailMaterial = tmpPSR.trailMaterial;
                            psr.mesh = tmpPSR.mesh;
                            psr.maxParticleSize = tmpPSR.maxParticleSize;
                            psr.minParticleSize = tmpPSR.minParticleSize;
                            psr.sortingFudge = tmpPSR.sortingFudge;
                            psr.sortMode = tmpPSR.sortMode;
                            psr.pivot = tmpPSR.pivot;
                            psr.alignment = tmpPSR.alignment;
                            psr.normalDirection = tmpPSR.normalDirection;
                            psr.cameraVelocityScale = tmpPSR.cameraVelocityScale;
                            psr.velocityScale = tmpPSR.velocityScale;
                            psr.lengthScale = tmpPSR.lengthScale;
                            psr.renderMode = tmpPSR.renderMode;
                            psr.maskInteraction = tmpPSR.maskInteraction;
                        }
                    }
                    GlowResponse tmpGR = popSobPartPrefab.GetComponentInChildren<GlowResponse>();
                    gro = parent.transform.GetChild(0).gameObject;
                    if ((gro != null) && gro.activeInHierarchy)
                    {
                        gr = gro.AddComponent<GlowResponse>();
                        gr = gro.GetComponent<GlowResponse>();
                        gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                        gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                        gr.fadeTime = tmpGR.fadeTime;
                        gr.light = tmpGR.light;
                        gr.audioPlayerPrefab = tmpGR.audioPlayerPrefab;
                        gr.soundEffect = tmpGR.soundEffect;
                    }
                    gro = parent.transform.GetChild(1).gameObject;
                    if ((gro != null) && gro.activeInHierarchy)
                    {
                        gr = gro.AddComponent<GlowResponse>();
                        gr = gro.GetComponent<GlowResponse>();
                        gr.FadeSprites.AddRange(gro.GetComponentsInChildren<SpriteRenderer>());
                        gr.particles = gro.GetComponentInChildren<ParticleSystem>();
                        gr.fadeTime = tmpGR.fadeTime;
                        gr.light = tmpGR.light;
                        gr.audioPlayerPrefab = tmpGR.audioPlayerPrefab;
                        gr.soundEffect = tmpGR.soundEffect;
                    }
                    popSobPartPrefab.SetActive(false);

                    popTabletInspectPrefab.SetActive(true);
                    GameObject inspect = GameObject.Instantiate(popTabletInspectPrefab, parent.transform);
                    popTabletInspectPrefab.SetActive(false);
                    inspect.transform.localPosition = new Vector3(-1, 0, 0);
                    inspect.transform.localEulerAngles = Vector3.zero;
                    inspect.transform.localScale = Vector3.one;
                    inspect.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(-0.5f, -2.6875f);
                    inspect.GetComponentInChildren<BoxCollider2D>().size = new Vector2(5f, 0.625f);

                    inspect.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmString("Game Text Convo").Value = LanguageStrings.CreditsTabletTextKey;
                    inspect.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("Hero Always Left").Value = true;
                    inspect.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("Hero Always Right").Value = false;
                }
                catch (Exception e)
                {
                    Log("PatchToT1SealOfBinding - " + e.ToString());
                }
            }
            #endregion
            Log("~Seal of Binding");
        }

        private /*IEnumerator*/ void PatchDamageResetColliders(Scene scene)
        {
            //yield return null;
            Log("!Damage Colliders");

            GameObject parent = scene.Find("Damage Colliders");
            Transform ch;
            for (int a = 0; a < parent.transform.childCount; a++)
            {
                ch = parent.transform.GetChild(a);

                if ((ch != null) && ch.gameObject.activeInHierarchy)
                {
                    try
                    {
                        var dh = ch.gameObject.AddComponent<DamageHero>();
                        dh.damageDealt = 1;
                        dh.shadowDashHazard = false;
                        dh.resetOnEnable = false;
                        dh.hazardType = (int)HazardType.ACID;

                        string name = ch.gameObject.name.ToLower();
                        if (name.Contains("thorn"))
                        {
                            ch.gameObject.AddComponent<NonBouncer>();
                        }
                        else if (name.Contains("pit"))
                        {
                            //dh.hazardType = (int)HazardType.PIT;
                            ch.gameObject.AddComponent<NonBouncer>();
                        }
                        else if (name.Contains("spikes"))
                        {
                            var te = ch.gameObject.AddComponent<TinkEffect>();
                            te = ch.gameObject.GetComponent<TinkEffect>();
                            te.blockEffect = wpTinkEffectPrefab.blockEffect;
                            te.useNailPosition = wpTinkEffectPrefab.useNailPosition;
                            te.sendFSMEvent = wpTinkEffectPrefab.sendFSMEvent;
                            te.FSMEvent = wpTinkEffectPrefab.FSMEvent;
                            te.fsm = wpTinkEffectPrefab.fsm;
                            te.sendDirectionalFSMEvents = wpTinkEffectPrefab.sendDirectionalFSMEvents;
                        }
                        else if (name.Contains("saw"))
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("PatchDamageResetColliders - " + ex.ToString());
                    }
                }
            }
            Log("~Damage Colliders");
        }

        private /*IEnumerator*/ void PatchHazardRespawnTrigger(Scene scene)
        {
            //yield return new WaitWhile(() => !scene.Find("Hazard Respawn Trigger List"));
            Log("!Hazard Respawn Triggers");
            GameObject markers = scene.Find("Hazard Respawn Trigger List");
            if (markers == null)
            {
                return; //yield break;
            }
            Transform tf;
            GameObject go;
            HazardRespawnMarker hrm;
            HazardRespawnTrigger hrt;
            for (int i = 0; i < markers.transform.childCount; i++)
            {
                tf = markers.transform.GetChild(i);
                go = tf.gameObject;
                if (go.name.Contains("Hazard Respawn Trigger v2"))
                {
                    #region Add HazardRespawnMarker to Child: Hazard Respawn Marker
                    hrm = go.transform.GetChild(0).gameObject.AddComponent<HazardRespawnMarker>();
                    hrm = go.transform.GetChild(0).gameObject.GetComponent<HazardRespawnMarker>();
                    if (go.name.ToLower().Contains("left"))
                        hrm.respawnFacingRight = false;
                    else
                        hrm.respawnFacingRight = true;
                    #endregion
                    #region Add HazardRespawnTrigger to Parent: Hazard Respawn Trigger v2
                    hrt = go.AddComponent<HazardRespawnTrigger>();
                    hrt = go.GetComponent<HazardRespawnTrigger>();
                    hrt.respawnMarker = hrm;
                    hrt.fireOnce = false;
                    #endregion
                }
            }

            Log("~Hazard Respawn Triggers");
        }

        private /*IEnumerator*/ void PatchCameraLockAreas(Scene scene)
        {
            //yield return new WaitWhile(() => !scene.FindRoot("_Camera Lock Zones"));
            Log("!Camera Lock Areas");

            GameObject areas = scene.FindRoot("_Camera Lock Zones");
            if (areas == null)
            {
                return; //yield break;
            }
            Transform tf;
            GameObject go;
            BoxCollider2D bc2d;
            CameraLockArea cla;
            for (int i = 0; i < areas.transform.childCount; i++)
            {
                tf = areas.transform.GetChild(i);
                go = tf.gameObject;
                cla = go.AddComponent<CameraLockArea>();
                cla = go.GetComponent<CameraLockArea>();
                bc2d = go.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
                cla.cameraXMin = bc2d.bounds.min.x + 14.6f;
                cla.cameraYMin = bc2d.bounds.min.y + 8.3f;
                cla.cameraXMax = bc2d.bounds.max.x - 14.6f;
                cla.cameraYMax = bc2d.bounds.max.y - 8.3f;
                cla.preventLookUp = go.name.Contains("nlu") || go.name.Contains("nldu");
                cla.preventLookDown = go.name.Contains("nld") || go.name.Contains("nlud");
                cla.maxPriority = false;
                //cla.respawnFacingRight = true;
                if (scene.name == TransitionGateNames.Tot01)
                {
                    if ((go.name == "Hazard Respawn Trigger v2 (1)") || (go.name == "Hazard Respawn Trigger v2 (3)"))
                        go.name = go.name;
                    //cla.respawnFacingRight = false;
                }
                else if (scene.name == TransitionGateNames.Tot02)
                {
                }
            }

            Log("~Camera Lock Areas");
        }

        private /*IEnumerator*/ void PatchEnemies(Scene scene)
        {
            //yield return new WaitWhile(() => !scene.FindRoot("_Enemies"));
            Log("!Enemies");

            GameObject enemies = scene.FindRoot("_Enemies");
            if (enemies == null)
            {
                return; //yield break;
            }
            Transform tf_list, tf;
            GameObject go;
            for (int i = 0; i < enemies.transform.childCount; i++)
            {
                tf_list = enemies.transform.GetChild(i);
                for (int j = (tf_list.childCount - 1); j >= 0; j--)
                {
                    tf = tf_list.GetChild(j);
                    go = tf.gameObject;
                    if (!go.activeInHierarchy)
                    {
                        continue;
                    }
                    if (tf_list.gameObject.name == "White Palace Fly Points")
                    {
                        GameObject tmp = GameObject.Instantiate(this.wpFlyPrefab, tf_list);
                        tmp.name = "White Palace Fly";
                        tmp.transform.localPosition = go.transform.localPosition;
                        tmp.transform.localScale = go.transform.localScale / 1.4f;
                        tmp.transform.localEulerAngles = go.transform.localEulerAngles;
                        tmp.SetActive(true);
                        GameObject.Destroy(go);
                    }
                    else if (tf_list.gameObject.name == "Saw No Sound Points")
                    {
                        GameObject tmp = GameObject.Instantiate(this.wpSawNoSoundPrefab, tf_list);
                        tmp.name = "Saw No Sound";
                        tmp.transform.localPosition = go.transform.localPosition;
                        tmp.transform.localScale = go.transform.localScale / 1.56f;
                        tmp.transform.localEulerAngles = go.transform.localEulerAngles;
                        tmp.SetActive(true);
                        GameObject.Destroy(go);
                    }
                    else if (tf_list.gameObject.name == "Saw With Sound Points")
                    {
                        GameObject tmp = GameObject.Instantiate(this.wpSawWithSoundPrefab, tf_list);
                        tmp.name = "Saw With Sound";
                        tmp.transform.localPosition = go.transform.localPosition;
                        tmp.transform.localScale = go.transform.localScale / 1.56f;
                        tmp.transform.localEulerAngles = go.transform.localEulerAngles;
                        tmp.SetActive(true);
                        GameObject.Destroy(go);
                    }
                    else if (tf_list.gameObject.name == "Moving Saw With Sound Points")
                    {
                        GameObject tmp = go.transform.GetChild(0).gameObject;
                        GameObject tmpPrefab = GameObject.Instantiate(this.wpSawWithSoundPrefab, go.transform);
                        tmpPrefab.name = "Moving Saw With Sound";
                        tmpPrefab.transform.localPosition = tmp.transform.localPosition;
                        tmpPrefab.transform.localScale = tmp.transform.localScale / 1.56f;
                        tmpPrefab.transform.localEulerAngles = tmp.transform.localEulerAngles;
                        tmpPrefab.SetActive(true);
                        GameObject.Destroy(tmp);
                    }
                    else if (tf_list.gameObject.name == "Infinite Soul Totem Points")
                    {
                        //GameObject tmp = GameObject.Instantiate(this.wpInfSoulTotemPrefab, tf_list);
                        GameObject tmp = GameObject.Instantiate(this.wpInfSoulTotemPrefab);
                        tmp.name = "Soul Totem white_Infinte";
                        tmp.transform.localPosition = go.transform.localPosition;
                        tmp.transform.localScale = go.transform.localScale / 1.5f;
                        tmp.transform.localEulerAngles = go.transform.localEulerAngles;
                        tmp.SetActive(true);
                        //tmp.transform.Find("Glower").gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        //(tmp.LocateMyFSM("soul_totem").GetState("Init").Actions[3] as SetMaterialColor).color.Value = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        var tmpFsm = tmp.LocateMyFSM("soul_totem");
                        tmpFsm.SetState("Pause Frame");
                        tmpFsm.SendEvent("RESET");
                        GameObject.Destroy(go);
                    }
                    else if (tf_list.gameObject.name == "White Spikes Points")
                    {
                        GameObject tmp = GameObject.Instantiate(this.wpSpikesPrefab, tf_list);
                        tmp.name = "White_Spike";
                        tmp.transform.localPosition = go.transform.localPosition;
                        tmp.transform.localScale = new Vector3(go.transform.localScale.x / 1.625f, go.transform.localScale.y / 1.45f, go.transform.localScale.z / 1.5f);
                        tmp.transform.localEulerAngles = go.transform.localEulerAngles;
                        tmp.SetActive(true);
                        GameObject.Destroy(go);
                    }
                }
            }

            Log("~Enemies");
        }

        private /*IEnumerator*/ void PatchHornetPickupColliders(Scene scene)
        {
            //yield return new WaitWhile(() => !scene.Find("Hornet Pickup Colliders"));
            Log("!Hornet Pickup Colliders");

            GameObject hornetColliders = scene.Find("Hornet Pickup Colliders");
            if (hornetColliders == null)
            {
                return; //yield break;
            }
            CircleCollider2D[] circleColliders = hornetColliders.GetComponentsInChildren<CircleCollider2D>();
            //HeroController hero = GameManager.instance.hero_ctrl;
            //hero.current_velocity = new Vector2(0, 0);
            HornetPickupPoint helper;
            foreach (var cc in circleColliders)
            {
                if (!cc.gameObject.activeInHierarchy)
                    continue;
                helper = cc.gameObject.AddComponent<HornetPickupPoint>();
                helper = cc.gameObject.GetComponent<HornetPickupPoint>();
                if (helper != null)
                    helper.points = cc.gameObject.transform.GetChild(0).gameObject.GetComponent<EdgeCollider2D>().points;
                else
                    Log("ToT01HornetCollider - Helper is NULL although it should not be!");

                if (cc.gameObject.name.Contains("First Stalactite Hornet"))
                {
                    if (helper != null)
                    {
                        helper.speed = 20f;
                        helper.secondsDelayBeforeInputAccepting = 0.75f;
                    }
                    else
                    {
                        Log("ToT01HornetCollider - Helper is NULL although it should not be!");
                    }
                }
                else if (cc.gameObject.name.Contains("Open Space Hornet"))
                {
                    if (helper != null)
                    {
                        helper.speed = 20f;
                        helper.secondsDelayBeforeInputAccepting = 0.25f;
                    }
                    else
                    {
                        Log("ToT01HornetCollider - Helper is NULL although it should not be!");
                    }
                }
            }

            Log("~Hornet Pickup Colliders");
        }

        private /*IEnumerator*/ void PatchHornetRefreshColliders(Scene scene)
        {
            //yield return new WaitWhile(() => !scene.Find("Hornet Refresh Colliders"));
            Log("!Hornet Refresh Colliders");

            GameObject hornetColliders = scene.Find("Hornet Refresh Colliders");
            if (hornetColliders == null)
            {
                return; //yield break;
            }
            CircleCollider2D[] circleColliders = hornetColliders.GetComponentsInChildren<CircleCollider2D>();
            //HeroController hero = GameManager.instance.hero_ctrl;
            //hero.current_velocity = new Vector2(0, 0);
            HornetRefreshPoint helper;
            foreach (var cc in circleColliders)
            {
                if (!cc.gameObject.activeInHierarchy)
                    continue;
                helper = cc.gameObject.AddComponent<HornetRefreshPoint>();
                helper = cc.gameObject.GetComponent<HornetRefreshPoint>();
            }

            Log("~Hornet Refresh Colliders");
        }

        private /*IEnumerator*/ void PatchTotMusicRegions(Scene scene)
        {
            if (!scene.name.StartsWith(TransitionGateNames.Tot))
                return; //yield break;
            //yield return new WaitWhile(() => !scene.Find("MusicRegionList"));
            Log("!Music Regions");

            GameObject musicRegionList = scene.Find("MusicRegionList");
            if (musicRegionList == null)
                return; //yield break;

            GameObject musicRegionGOTemplate;
            for (int i = musicRegionList.transform.childCount - 1; i >= 0; i--)
            {
                musicRegionGOTemplate = musicRegionList.transform.GetChild(i).gameObject;
                if (!musicRegionGOTemplate.activeInHierarchy)
                    continue;

                try
                {
                    GameObject tmpMusicRegionGO = GameObject.Instantiate(popMusicRegionPrefab);
                    tmpMusicRegionGO.SetActive(true);

                    MusicRegion musicRegion = tmpMusicRegionGO.GetComponent<MusicRegion>();

                    //musicRegion.enterMusicCue = Instantiate(popMusicRegionPrefab.GetComponent<MusicRegion>().enterMusicCue);
                    musicRegion.enterMusicCue = popEnterMusicCuePrefab;

                    MusicCue.MusicChannelInfo[] musicChannelInfos = musicRegion.enterMusicCue.GetAttr<MusicCue, MusicCue.MusicChannelInfo[]>("channelInfos");

                    string[] splitName = musicRegionGOTemplate.name.Split(' ');
                    int mrID = int.Parse(splitName[1]);
                    if (mrID > 6)
                    {
                        GameObject.Destroy(tmpMusicRegionGO);
                        continue;
                    }
                    musicChannelInfos[(int)MusicChannels.Main].SetAttr("sync", MusicChannelSync.Implicit);
                    musicChannelInfos[(int)MusicChannels.Action].SetAttr("sync", MusicChannelSync.ExplicitOn);
                    musicChannelInfos[(int)MusicChannels.Sub].SetAttr("sync", MusicChannelSync.ExplicitOn);
                    musicChannelInfos[(int)MusicChannels.Tension].SetAttr("sync", MusicChannelSync.ExplicitOn);
                    musicChannelInfos[(int)MusicChannels.MainAlt].SetAttr("sync", MusicChannelSync.ExplicitOn);
                    musicChannelInfos[(int)MusicChannels.Extra].SetAttr("sync", MusicChannelSync.ExplicitOn);

                    if (!zaliantMusic)
                    {
                        musicChannelInfos[(int)MusicChannels.Main].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT01Key));
                        musicChannelInfos[(int)MusicChannels.Action].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT02Key));
                        musicChannelInfos[(int)MusicChannels.Sub].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT04Key));
                        musicChannelInfos[(int)MusicChannels.Tension].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT05Key));
                        musicChannelInfos[(int)MusicChannels.MainAlt].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT06Key));
                    }
                    else
                    {
                        musicChannelInfos[(int)MusicChannels.Main].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT01KeyZ));
                        musicChannelInfos[(int)MusicChannels.Action].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT02KeyZ));
                        musicChannelInfos[(int)MusicChannels.Sub].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT04KeyZ));
                        musicChannelInfos[(int)MusicChannels.Tension].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT05KeyZ));
                        musicChannelInfos[(int)MusicChannels.MainAlt].SetAttr("clip", TestOfTeamwork.GetAudio(AudioStrings.ToT06KeyZ));
                    }
                    musicChannelInfos[(int)MusicChannels.Extra].SetAttr("clip", null as AudioClip);

                    if (mrID >= 1)
                    {
                    }
                    if (mrID >= 2)
                    {
                        //musicRegion.enterMusicSnapshot = Instantiate(wp17AMS);
                        musicRegion.enterMusicSnapshot = wp17AMS;
                    }
                    if (mrID >= 3)
                    {
                        //musicRegion.enterMusicSnapshot = Instantiate(wp19AMS);
                        musicRegion.enterMusicSnapshot = wp19AMS;
                    }
                    if (mrID >= 4)
                    {
                        //musicRegion.enterMusicSnapshot = Instantiate(wp20AMS);
                        musicRegion.enterMusicSnapshot = wp20AMS;
                    }
                    if (mrID >= 5)
                    {
                        //AudioMixerSnapshot tmp = (AudioMixerSnapshot)(wp20PMFSM.GetAction<TransitionToAudioSnapshot>("Start", 2).snapshot.RawValue);
                        //musicRegion.enterMusicSnapshot = Instantiate(tmp);
                        musicRegion.enterMusicSnapshot = wp20PMFSM.GetAction<TransitionToAudioSnapshot>("Start", 2).snapshot.RawValue as AudioMixerSnapshot;
                    }
                    if (mrID >= 6)
                    {
                        //AudioMixerSnapshot tmp = (AudioMixerSnapshot)(wp20PMFSM.GetAction<TransitionToAudioSnapshot>("Start", 2).snapshot.RawValue);
                        //musicRegion.enterMusicSnapshot = Instantiate(tmp);
                        musicRegion.enterMusicSnapshot = wp20PMFSM.GetAction<TransitionToAudioSnapshot>("Start", 2).snapshot.RawValue as AudioMixerSnapshot;
                    }

                    if (musicRegion.exitMusicCue != null)
                    {
                        musicRegion.exitMusicCue.SetAttr<MusicCue, MusicCue.MusicChannelInfo[]>("channelInfos", null);
                    }
                    musicRegion.dirtmouth = false;
                    musicRegion.minesDelay = false;
                    musicRegion.enterTrackEvent = "CROSSROADS";
                    musicRegion.enterTransitionTime = 3;
                    musicRegion.exitMusicCue = null;
                    musicRegion.exitMusicSnapshot = null;
                    musicRegion.exitTrackEvent = "";
                    musicRegion.exitTransitionTime = 0;

                    musicRegion.enterMusicCue.SetAttr<MusicCue, string>("originalMusicEventName", "CROSSROADS");
                    musicRegion.enterMusicCue.SetAttr<MusicCue, int>("originalMusicTrackNumber", 2);

                    // BoxCollider2D
                    tmpMusicRegionGO.transform.position = musicRegionGOTemplate.transform.position;
                    tmpMusicRegionGO.transform.eulerAngles = musicRegionGOTemplate.transform.eulerAngles;
                    tmpMusicRegionGO.transform.localScale = musicRegionGOTemplate.transform.localScale;
                    tmpMusicRegionGO.GetComponent<BoxCollider2D>().offset = musicRegionGOTemplate.GetComponent<BoxCollider2D>().offset;
                    tmpMusicRegionGO.GetComponent<BoxCollider2D>().size = musicRegionGOTemplate.GetComponent<BoxCollider2D>().size;

                    GameObject.Destroy(musicRegionGOTemplate);
                }
                catch (Exception)
                {
                    Log("MusicRegion \"" + musicRegionGOTemplate.name + "\" couldn't be parsed!");
                }
            }

            Log("~Music Regions");
        }

        private void PatchTotLever(Scene scene)
        {
            if (!scene.name.StartsWith(TransitionGateNames.Tot))
                return;
            Log("!PatchTotLever");

            GameObject markers = null;
            foreach (var rootGO in scene.GetRootGameObjects())
                if (rootGO.name == "_Markers")
                    markers = rootGO;
            // SFGrenadeDreamKing_TotOpenedShortcut
            GameObject leverList = markers.transform.Find("LeverList").gameObject;
            GameObject lever;
            for (int i = (leverList.transform.childCount - 1); i >= 0; i--)
            {
                Transform tmpTransform = leverList.transform.GetChild(i);
                if (tmpTransform == null)
                    continue;
                lever = leverList.transform.GetChild(i).gameObject;
                if (lever.activeInHierarchy)
                {
                    GameObject actualLever = GameObject.Instantiate(wpLeverPrefab);
                    actualLever.name = lever.name;
                    actualLever.SetActive(true);
                    actualLever.transform.position = lever.transform.position;
                    actualLever.transform.eulerAngles = lever.transform.eulerAngles;
                    actualLever.transform.localScale = lever.transform.lossyScale;

                    PlayMakerFSM leverFsm = actualLever.LocateMyFSM("Switch Control");
                    PersistentBoolItem leverPbi = actualLever.GetComponent<PersistentBoolItem>();
                    if (lever.name == "SFGrenadeTestOfTeamwork_TotOpenedShortcut")
                    {
                        leverFsm.FsmVariables.GetFsmBool("Activated").Value = TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkTotOpenedShortcut;
                        leverFsm.FsmVariables.GetFsmBool("SetPlayerData").Value = true;
                        leverFsm.FsmVariables.GetFsmString("Player Data").Value = nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkTotOpenedShortcut);

                        leverPbi.semiPersistent = false;
                        var tmpPbd = new PersistentBoolData();
                        tmpPbd.id = lever.name;
                        tmpPbd.sceneName = TransitionGateNames.Tot02;
                        tmpPbd.activated = TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkTotOpenedShortcut;
                        tmpPbd.semiPersistent = false;
                        leverPbi.persistentBoolData = tmpPbd;
                    }

                    GameObject.Destroy(lever);
                }
            }

            Log("~PatchTotLever");
        }

        private void PatchBlocker(Scene scene)
        {
            if (!scene.name.StartsWith(TransitionGateNames.Tot))
                return;
            Log("!PatchBlocker");


            // SFGrenadeDreamKing_TotOpenedShortcut
            GameObject blockerList = scene.Find("BlockerList");
            foreach (var sr in blockerList.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material = litSpriteMaterial;
            }

            GameObject blocker;
            for (int i = (blockerList.transform.childCount - 1); i >= 0; i--)
            {
                Transform tmpTransform = blockerList.transform.GetChild(i);
                if (tmpTransform == null)
                    continue;
                blocker = blockerList.transform.GetChild(i).gameObject;
                if (blocker.activeInHierarchy)
                {
                    if (blocker.name.StartsWith("Block - "))
                    {
                        DeactivateIfPlayerdataTrue dipt = blocker.AddComponent<DeactivateIfPlayerdataTrue>();
                        dipt = blocker.GetComponent<DeactivateIfPlayerdataTrue>();
                        dipt.boolName = blocker.name.Replace("Block - ", "");

                        //GameObject actualBlocker = GameObject.Instantiate(blocker, blockerList.transform, true);
                        GameObject actualBlocker = GameObject.Instantiate(blocker);
                        actualBlocker.transform.localPosition = blocker.transform.localPosition;
                        actualBlocker.transform.localEulerAngles = blocker.transform.localEulerAngles;
                        actualBlocker.transform.localScale = blocker.transform.localScale;
                        actualBlocker.name = blocker.name;

                        GameObject.Destroy(blocker);
                    }
                    //if (blocker.name.StartsWith("Floor - "))
                    //{
                    //    var pdBool = blocker.name.Replace("Floor - ", "");
                    //    var pdBoolVal = PlayerData.instance.GetBool(pdBool);

                    //    //Log("Floor Val: " + pdBool);
                    //    //Log("Floor Val: " + pdBoolVal);

                    //    //GameObject actualBlocker = GameObject.Instantiate(popQuakeFloorPrefab, blockerList.transform, true);
                    //    GameObject actualBlocker = GameObject.Instantiate(popQuakeFloorPrefab);
                    //    actualBlocker.SetActive(true);
                    //    actualBlocker.transform.position = blocker.transform.position;
                    //    actualBlocker.transform.localScale = blocker.transform.lossyScale;
                    //    actualBlocker.transform.eulerAngles = blocker.transform.eulerAngles;
                    //    GameObject.Destroy(actualBlocker.transform.Find("Active").gameObject);
                    //    GameObject.Destroy(actualBlocker.transform.Find("Inactive").gameObject);
                    //    GameObject.Instantiate(blocker.transform.Find("Active").gameObject, actualBlocker.transform, true);
                    //    var inactiveTmp = GameObject.Instantiate(blocker.transform.Find("Inactive").gameObject, actualBlocker.transform, true);
                    //    actualBlocker.name = blocker.name;

                    //    inactiveTmp.SetActive(false);

                    //    var pbi = actualBlocker.GetComponent<PersistentBoolItem>();
                    //    pbi.semiPersistent = false;
                    //    var pbd = pbi.persistentBoolData;
                    //    pbd.id = actualBlocker.name;
                    //    pbd.sceneName = scene.name;
                    //    pbd.activated = pdBoolVal;
                    //    pbd.semiPersistent = false;

                    //    var blockerFsm = actualBlocker.LocateMyFSM("quake_floor");
                    //    var blockerFsmVars = blockerFsm.FsmVariables;
                    //    blockerFsmVars.GetFsmBool("Activated").Value = pdBoolVal;
                    //    blockerFsmVars.GetFsmBool("Glass").Value = false;
                    //    blockerFsmVars.GetFsmString("Playerdata Bool").Value = pdBool;

                    //    GameObject.Destroy(blocker);
                    //}
                }
            }

            Log("~PatchBlocker");
        }

        private void PatchBoss(Scene scene)
        {
            if (!scene.name.StartsWith(TransitionGateNames.Tot))
                return;
            Log("!PatchBoss");

            var beforeFightPrefab = Instantiate(hornet2BossPrefab);
            beforeFightPrefab.transform.position = scene.FindRoot("Boss").transform.position;
            beforeFightPrefab.SetActive(true);

            var encounterFsm = beforeFightPrefab.LocateMyFSM("Encounter");
            encounterFsm.GetAction<PlayerDataBoolTest>("Init", 1).boolName = nameof(TestOfTeamwork.Instance.Settings.SFGrenadeTestOfTeamworkEncounterBeforeBoss);

            encounterFsm.RemoveAction("Point", 2);

            var dialogueVars1 = new HutongGames.PlayMaker.FsmVar[] { new HutongGames.PlayMaker.FsmVar(typeof(string)), new HutongGames.PlayMaker.FsmVar(typeof(string)) };
            dialogueVars1[0].SetValue(LanguageStrings.HornetMonologueKey);
            dialogueVars1[1].SetValue("Hornet");
            encounterFsm.GetAction<CallMethodProper>("Dialogue", 1).parameters = dialogueVars1;

            var dialogueVars2 = new HutongGames.PlayMaker.FsmVar[] { new HutongGames.PlayMaker.FsmVar(typeof(string)), new HutongGames.PlayMaker.FsmVar(typeof(string)) };
            dialogueVars2[0].SetValue(LanguageStrings.HornetMonologueKey);
            dialogueVars2[1].SetValue("Hornet");
            encounterFsm.GetAction<CallMethodProper>("Dialogue 2", 0).parameters = dialogueVars2;

            encounterFsm.RemoveAction("Start Fight", 0);

            Log("~PatchBoss");
        }
        private void CreateBreakableWall(string sceneName, string name, Vector3 pos, Vector3 angles, Vector3 scale, Vector2 size, string playerDataBool)
        {
            Log("!CreateBreakableWall");
            //SFGrenadeDreamKing_TotOpened

            GameObject breakableWall = GameObject.Instantiate(breakableWallPrefab);
            breakableWall.SetActive(true);
            breakableWall.name = name;
            breakableWall.transform.position = pos;
            breakableWall.transform.eulerAngles = angles;
            breakableWall.transform.localScale = scale;

            var wallBc2d = breakableWall.GetComponent<BoxCollider2D>();
            wallBc2d.size = size;
            wallBc2d.offset = Vector2.zero;

            var wallFsm = breakableWall.LocateMyFSM("breakable_wall_v2");
            wallFsm.FsmVariables.GetFsmBool("Activated").Value = false;
            wallFsm.FsmVariables.GetFsmBool("Ruin Lift").Value = false;
            wallFsm.FsmVariables.GetFsmString("PlayerData Bool").Value = playerDataBool;

            GameObject.Destroy(breakableWall.GetComponent<PersistentBoolItem>());

            Log("~CreateBreakableWall");
        }

        private void CreateGateway(string gateName, Vector2 pos, Vector2 size, string toScene, string entryGate, Vector2 respawnPoint,
                                   bool right, bool left, bool onlyOut, GameManager.SceneLoadVisualizations vis)
        {
            Log("!Gateway");

            GameObject gate = new GameObject(gateName);
            gate.transform.SetPosition2D(pos);
            var tp = gate.AddComponent<TransitionPoint>();
            if (!onlyOut)
            {
                var bc = gate.AddComponent<BoxCollider2D>();
                bc.size = size;
                bc.isTrigger = true;
                tp.SetTargetScene(toScene);
                tp.entryPoint = entryGate;
            }
            tp.alwaysEnterLeft = left;
            tp.alwaysEnterRight = right;

            GameObject rm = new GameObject("Hazard Respawn Marker");
            rm.transform.parent = gate.transform;
            rm.transform.SetPosition2D(pos.x + respawnPoint.x, pos.y + respawnPoint.y);
            var tmp = rm.AddComponent<HazardRespawnMarker>();
            tmp.respawnFacingRight = right;
            tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
            tp.sceneLoadVisualization = vis;

            Log("~Gateway");
        }

        private void CreateBench(string benchName, Vector3 pos, string sceneName)
        {
            GameObject bench = GameObject.Instantiate(whiteBenchPrefab);
            bench.SetActive(true);
            bench.transform.position = pos;
            bench.name = benchName;
            var benchFsm = bench.LocateMyFSM("Bench Control");
            benchFsm.FsmVariables.FindFsmString("Scene Name").Value = sceneName;
            benchFsm.FsmVariables.FindFsmString("Spawn Name").Value = benchName;
        }

        private void printDebug(GameObject go, string tabindex = "")
        {
            Log(tabindex + "Name: " + go.name);
            foreach (var comp in go.GetComponents<Component>())
            {
                Log(tabindex + "Component: " + comp.GetType());
            }
            for (int i = 0; i < go.transform.childCount; i++)
            {
                printDebug(go.transform.GetChild(i).gameObject, tabindex + "\t");
            }
        }

        private void Log(String message)
        {
            Logger.Log($"[{this.GetType().FullName.Replace(".", "]:[")}] - {message}");
        }
        private void Log(System.Object message)
        {
            Logger.Log($"[{this.GetType().FullName.Replace(".", "]:[")}] - {message.ToString()}");
        }


        private static Transform findChild(GameObject parent, string name)
        {
            Transform ret = null;

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).name == name)
                {
                    ret = parent.transform.GetChild(i);
                }
            }

            return ret;
        }

        private static void SetInactive(GameObject go)
        {
            if (go != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
                go.SetActive(false);
            }
        }
        private static void SetInactive(UnityEngine.Object go)
        {
            if (go != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
        }
    }
}
