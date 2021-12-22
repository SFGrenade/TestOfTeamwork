using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using Object = UnityEngine.Object;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    class PatchBlocker : MonoBehaviour
    {
        public enum Type
        {
            WALL,
            FLOOR
        }

        public Type type;
        public string pdBool;

        public void Start()
        {
            if (type == Type.FLOOR)
            {
                GameObject actualBlocker = Instantiate(PrefabHolder.PopQuakeFloorPrefab, transform.parent, true);
                actualBlocker.SetActive(false);
                actualBlocker.transform.position = transform.position;
                actualBlocker.transform.localScale = transform.lossyScale;
                actualBlocker.transform.eulerAngles = transform.eulerAngles;

                var t = actualBlocker.Find("Active").transform;
                var t2 = gameObject.Find("Active").transform;
                for (int c = t2.childCount - 1; c >= 0; c--)
                {
                    t2.GetChild(c).SetParent(t, true);
                }
                t = actualBlocker.Find("Inactive").transform;
                t2 = gameObject.Find("Inactive").transform;
                for (int c = t2.childCount - 1; c >= 0; c--)
                {
                    t2.GetChild(c).SetParent(t, true);
                }
                actualBlocker.Find("Inactive").SetActive(false);

                var blockerFsm = actualBlocker.LocateMyFSM("quake_floor");
                if (blockerFsm.GetState("Init").Fsm == null)
                {
                    blockerFsm.Preprocess();
                }
                blockerFsm.SetAttr("fsmTemplate", (FsmTemplate) null);
                var blockerFsmVars = blockerFsm.FsmVariables;
                blockerFsmVars.GetFsmBool("Glass").Value = false;
                blockerFsmVars.GetFsmString("Playerdata Bool").Value = pdBool;

                DestroyObject doAction = new DestroyObject
                {
                    gameObject = blockerFsmVars.GetFsmGameObject("Inactive"),
                    delay = 0.0f,
                    detachChildren = true
                };
                var agoAction = new ActivateGameObject
                {
                    gameObject = new FsmOwnerDefault
                    {
                        OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                        GameObject = blockerFsmVars.GetFsmGameObject("Inactive")
                    },
                    activate = false,
                    recursive = false,
                    resetOnExit = false,
                    everyFrame = false
                };
                blockerFsm.AddAction("Destroy", agoAction);
                blockerFsm.AddAction("Destroy", doAction);

                blockerFsm.RemoveAction("Activate", 0);
                blockerFsm.AddAction("Activate", new SetBoolValue()
                {
                    boolVariable = blockerFsmVars.GetFsmBool("Activated"),
                    boolValue = true,
                    everyFrame = false
                });
                blockerFsm.AddAction("Activate", new ActivateGameObject()
                {
                    gameObject = new FsmOwnerDefault()
                    {
                        OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                        GameObject = blockerFsmVars.GetFsmGameObject("Active")
                    },
                    activate = false,
                    recursive = false,
                    resetOnExit = false,
                    everyFrame = false
                });
                blockerFsm.AddAction("Activate", new ActivateGameObject()
                {
                    gameObject = new FsmOwnerDefault()
                    {
                        OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                        GameObject = blockerFsmVars.GetFsmGameObject("Inactive")
                    },
                    activate = true,
                    recursive = false,
                    resetOnExit = false,
                    everyFrame = false
                });
                blockerFsm.AddAction("Activate", new SetCollider()
                {
                    gameObject = new FsmOwnerDefault()
                    {
                        OwnerOption = OwnerDefaultOption.UseOwner
                    },
                    active = false
                });
                blockerFsm.AddAction("Init", new PlayerDataBoolTest()
                {
                    gameObject = new FsmOwnerDefault()
                    {
                        OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                        GameObject = GameManager.instance.gameObject
                    },
                    boolName = blockerFsmVars.GetFsmString("Playerdata Bool"),
                    isTrue = FsmEvent.GetFsmEvent("ACTIVATE"),
                    isFalse = FsmEvent.Finished
                });
                blockerFsm.RemoveAction("Init", 0);
                blockerFsm.RemoveAction("PD Bool?", 0);

                blockerFsm.SetState("Pause");

                //blockerFsm.MakeLog(true);

                actualBlocker.SetActive(true);
                Destroy(gameObject);
            }
            else if (type == Type.WALL)
            {
            }
        }
    }
}
