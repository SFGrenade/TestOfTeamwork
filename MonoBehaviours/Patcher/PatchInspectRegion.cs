using System;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    [RequireComponent(typeof(BoxCollider2D))]
    class PatchInspectRegion : MonoBehaviour
    {
        public string GameTextConvo = "";
        public bool HeroAlwaysLeft = false;
        public bool HeroAlwaysRight = false;

        public void Start()
        {
            try
            {
                PrefabHolder.popTabletInspectPrefab.SetActive(true);
                GameObject inspect = GameObject.Instantiate(PrefabHolder.popTabletInspectPrefab, transform);
                PrefabHolder.popTabletInspectPrefab.SetActive(false);
                inspect.transform.localPosition = Vector3.zero;
                inspect.transform.localEulerAngles = Vector3.zero;
                inspect.transform.localScale = Vector3.one;

                var selfBc2d = GetComponent<BoxCollider2D>();
                var newBc2d = inspect.GetComponentInChildren<BoxCollider2D>();
                newBc2d.offset = selfBc2d.offset;
                newBc2d.size = selfBc2d.size;

                var fsm = inspect.LocateMyFSM("inspect_region");
                var fsmVar = fsm.FsmVariables;

                fsmVar.GetFsmString("Game Text Convo").Value = GameTextConvo;
                fsmVar.GetFsmBool("Hero Always Left").Value = HeroAlwaysLeft;
                fsmVar.GetFsmBool("Hero Always Right").Value = HeroAlwaysRight;
            }
            catch (Exception e)
            {
                Debug.Log("PatchInspectRegion - " + e.ToString());
            }
        }
    }
}
