using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using Logger = Modding.Logger;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker.Actions;
using SFCore.MonoBehaviours;
using TestOfTeamwork.MonoBehaviours;
using TestOfTeamwork.Consts;
using UObject = UnityEngine.Object;
using SFCore.Utils;

namespace TestOfTeamwork
{
    public class SceneChanger : MonoBehaviour
    {
        private const bool Debug = true;
        private const string AbPath = "E:\\Github_Projects\\TestOfTeamwork Assets\\Assets\\AssetBundles\\";

        public AssetBundle AbOverallMat { get; private set; } = null;
        public AssetBundle AbTotScene { get; private set; } = null;
        public AssetBundle AbTotMat { get; private set; } = null;

        public SceneChanger(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.GameManager.RefreshTilemapInfo += OnGameManagerRefreshTilemapInfo;

            PrefabHolder.Preloaded(preloadedObjects);

            Assembly asm;

            #region Load AssetBundles
#pragma warning disable CS0162 // Unerreichbarer Code wurde entdeckt.
            asm = Assembly.GetExecutingAssembly();
            if (AbOverallMat == null)
            {
                if (!Debug)
                {
                    using (Stream s = asm.GetManifestResourceStream("TestOfTeamwork.Resources.overall_materials_tot"))
                    {
                        if (s != null)
                        {
                            AbOverallMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbOverallMat = AssetBundle.LoadFromFile(AbPath + "overall_materials_tot");
                }
            }
            if (AbTotScene == null)
            {
                if (!Debug)
                {
                    using (Stream s = asm.GetManifestResourceStream("TestOfTeamwork.Resources.test_of_teamwork_scenes"))
                    {
                        if (s != null)
                        {
                            AbTotScene = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbTotScene = AssetBundle.LoadFromFile(AbPath + "test_of_teamwork_scenes");
                }
            }
            if (AbTotMat == null)
            {
                if (!Debug)
                {
                    using (Stream s = asm.GetManifestResourceStream("TestOfTeamwork.Resources.test_of_teamwork_materials"))
                    {
                        if (s != null)
                        {
                            AbTotMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbTotMat = AssetBundle.LoadFromFile(AbPath + "test_of_teamwork_materials");
                }
            }
#pragma warning restore CS0162 // Unerreichbarer Code wurde entdeckt.
            #endregion
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
            else if (targetScene == TransitionGateNames.TotDropdown)
            {
                float width = 32;
                float height = 864;
                self.tilemap.width = (int)width;
                self.tilemap.height = (int)height;
                self.sceneWidth = width;
                self.sceneHeight = height;
                FindObjectOfType<GameMap>().SetManualTilemap(0, 0, width, height);
            }
            else if (targetScene == TransitionGateNames.TotClaus)
            {
                float width = 330;
                float height = 17;
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

            #region Hornet NPC FSM

            GameObject hornetNpcGo = scene.FindRoot("Hornet Black Egg NPC");
            PlayMakerFSM hornetNpcFsm = hornetNpcGo.LocateMyFSM("Conversation Control");
            FsmVariables hornetNpcFsmVar = hornetNpcFsm.FsmVariables;

            hornetNpcFsm.CopyState("Greet", "Give Item");
            hornetNpcFsm.RemoveAction("Give Item", 0);

            hornetNpcFsm.GetAction<CallMethodProper>("Give Item", 0).parameters = new[]
            {
                new FsmVar(typeof(string)) { stringValue = "CUSTOM_HORNET_PRE_FINAL_BATTLE" },
                new FsmVar(typeof(string)) { stringValue = "Hornet" }
            };

            var pdbtAction = new PlayerDataBoolTest();
            pdbtAction.gameObject = hornetNpcFsm.GetAction<PlayerDataBoolTest>("Choice", 1).gameObject;
            pdbtAction.boolName = "SFGrenadeTestOfTeamworkHornetCompanion";
            pdbtAction.isFalse = FsmEvent.GetFsmEvent("ABSENT");

            hornetNpcFsm.InsertAction("Choice", pdbtAction, 1);

            hornetNpcFsm.AddTransition("Choice", "ABSENT", "Give Item");

            #endregion

            #region Shiny FSM

            GameObject shinyParent = Instantiate(PrefabHolder.ShinyPrefab);
            shinyParent.name = "Necklace";
            shinyParent.SetActive(false);
            shinyParent.transform.GetChild(0).gameObject.SetActive(true);
            shinyParent.transform.position = new Vector3(29.0f, 4.3f, 0.0f);

            hornetNpcFsm.CopyState("Box Up", "Give Item Spawn");

            hornetNpcFsm.ChangeTransition("Give Item Spawn", FsmEvent.Finished.Name, "Talk Finish");
            hornetNpcFsm.ChangeTransition("Give Item", "CONVO_FINISH", "Give Item Spawn");

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
            shinyFsmVars.FindFsmString("Item Name").Value = Consts.LanguageStrings.HornetInvNameKey;
            shinyFsmVars.FindFsmString("PD Bool Name").Value = "SFGrenadeTestOfTeamworkHornetCompanion";

            IntSwitch isAction = shinyFsm.GetAction<IntSwitch>("Trinket Type", 0);
            var tmpCompareTo = new List<FsmInt>(isAction.compareTo);
            tmpCompareTo.Add(tmpCompareTo.Count + 1);
            isAction.compareTo = tmpCompareTo.ToArray();
            shinyFsmVars.FindFsmInt("Trinket Num").Value = tmpCompareTo.Count;
            var tmpSendEvent = new List<FsmEvent>(isAction.sendEvent);
            tmpSendEvent.Add(FsmEvent.FindEvent("PURE SEED"));
            isAction.sendEvent = tmpSendEvent.ToArray();

            shinyFsm.CopyState("Love Key", "Necklace");

            shinyFsm.GetAction<SetPlayerDataBool>("Necklace", 0).boolName = "SFGrenadeTestOfTeamworkHornetCompanion";
            shinyFsm.GetAction<SetSpriteRendererSprite>("Necklace", 1).sprite = TestOfTeamwork.Instance.SpriteDict.Get(TextureStrings.InvHornetKey);
            shinyFsm.GetAction<GetLanguageString>("Necklace", 2).convName = Consts.LanguageStrings.HornetInvNameKey;

            shinyFsm.AddTransition("Trinket Type", "PURE SEED", "Necklace");

            #endregion
        }

        public void CR_Change_White_Palace_06(Scene scene)
        {
            if (scene.name != "White_Palace_06")
                return;

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
            CreateBreakableWall(scene.name, "SF_ToT_Breakable_Wall_ToT", new Vector3(30.0f, 8.0f, 0.05f), Vector3.zero, Vector3.one, new Vector2(6.0f, 4.0f), "SFGrenadeTestOfTeamworkTotOpened");
            //yield return null;

            PatchMusicRegions.altMusic = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;
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
        }

        public void CR_Change_ToTEndless(Scene scene)
        {
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

        private void CreateBreakableWall(string sceneName, string name, Vector3 pos, Vector3 angles, Vector3 scale, Vector2 size, string playerDataBool)
        {
            GameObject breakableWall = Instantiate(PrefabHolder.BreakableWallPrefab);
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
        }

        private void CreateGateway(string gateName, Vector2 pos, Vector2 size, string toScene, string entryGate, Vector2 respawnPoint,
                                   bool right, bool left, bool onlyOut, GameManager.SceneLoadVisualizations vis)
        {
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
        }

        private void CreateBench(string benchName, Vector3 pos, string sceneName)
        {
            GameObject bench = Instantiate(PrefabHolder.WhiteBenchPrefab);
            bench.SetActive(true);
            bench.transform.position = pos;
            bench.name = benchName;
            var benchFsm = bench.LocateMyFSM("Bench Control");
            benchFsm.FsmVariables.FindFsmString("Scene Name").Value = sceneName;
            benchFsm.FsmVariables.FindFsmString("Spawn Name").Value = benchName;
        }

        private void PrintDebug(GameObject go, string tabindex = "")
        {
            Log(tabindex + "Name: " + go.name);
            foreach (var comp in go.GetComponents<Component>())
            {
                Log(tabindex + "Component: " + comp.GetType());
            }
            for (int i = 0; i < go.transform.childCount; i++)
            {
                PrintDebug(go.transform.GetChild(i).gameObject, tabindex + "\t");
            }
        }

        private void Log(String message)
        {
            Logger.Log($"[{GetType().FullName.Replace(".", "]:[")}] - {message}");
        }
        private void Log(System.Object message)
        {
            Logger.Log($"[{GetType().FullName.Replace(".", "]:[")}] - {message}");
        }

        private static void SetInactive(GameObject go)
        {
            if (go != null)
            {
                DontDestroyOnLoad(go);
                go.SetActive(false);
            }
        }
        private static void SetInactive(UnityEngine.Object go)
        {
            if (go != null)
            {
                DontDestroyOnLoad(go);
            }
        }
    }
}
