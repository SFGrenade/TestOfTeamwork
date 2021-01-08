using UnityEngine;
using HutongGames.PlayMaker;
using ModCommon.Util;
using HutongGames.PlayMaker.Actions;

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
                var pdBoolVal = PlayerData.instance.GetBool(pdBool);

                GameObject actualBlocker = GameObject.Instantiate(PrefabHolder.popQuakeFloorPrefab, transform.parent, true);
                actualBlocker.SetActive(false);
                actualBlocker.transform.position = transform.position;
                actualBlocker.transform.localScale = transform.lossyScale;
                actualBlocker.transform.eulerAngles = transform.eulerAngles;

                var t = actualBlocker.transform.Find("Active");
                var t2 = transform.Find("Active");
                for (int c = t2.childCount - 1; c >= 0; c--)
                {
                    t2.GetChild(c).SetParent(t, true);
                }
                t = actualBlocker.transform.Find("Inactive");
                t2 = transform.Find("Inactive");
                for (int c = t2.childCount - 1; c >= 0; c--)
                {
                    t2.GetChild(c).SetParent(t, true);
                }

                var blockerFsm = actualBlocker.LocateMyFSM("quake_floor");
                var blockerFsmVars = blockerFsm.FsmVariables;
                blockerFsmVars.GetFsmBool("Activated").Value = pdBoolVal;
                blockerFsmVars.GetFsmBool("Glass").Value = false;
                blockerFsmVars.GetFsmString("Playerdata Bool").Value = pdBool;

                var doAction = new DestroyObject();
                doAction.gameObject = blockerFsmVars.GetFsmGameObject("Inactive");
                doAction.delay = 0.0f;
                doAction.detachChildren = true;
                var agoAction = new ActivateGameObject();
                agoAction.gameObject = new FsmOwnerDefault();
                agoAction.gameObject.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
                agoAction.gameObject.GameObject = blockerFsmVars.GetFsmGameObject("Inactive");
                agoAction.activate = false;
                agoAction.recursive = false;
                agoAction.resetOnExit = false;
                agoAction.everyFrame = false;

                blockerFsm.InsertAction("Activate", doAction, 0);
                blockerFsm.InsertAction("Activate", agoAction, 0);
                blockerFsm.AddAction("Destroy", agoAction);
                blockerFsm.AddAction("Destroy", doAction);

                blockerFsm.SetState("Pause");

                t.gameObject.SetActive(false);

                actualBlocker.SetActive(true);

                GameObject.Destroy(gameObject);
            }
            else if (type == Type.WALL)
            {
            }
        }
    }
}
