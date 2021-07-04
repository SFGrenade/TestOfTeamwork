using GlobalEnums;
using System;
using System.Collections;
using System.Reflection;
using HutongGames.PlayMaker;
using UnityEngine;
using Logger = Modding.Logger;
using SFCore.Utils;

namespace TestOfTeamwork.MonoBehaviours
{
    public class DreamFallCatcher : MonoBehaviour
    {
        private HeroController h;
        private GameObject hGo;
        private GameManager g;
        private CameraController c;
        private IEnumerator df;

        private void Start()
        {
            h = HeroController.instance;
            hGo = h.gameObject;
            g = GameManager.instance;
            c = g.cameraCtrl;
            df = null;
        }

        private void FixedUpdate()
        {
            if (h.transform.position.y > 0) return;
            if (df != null) return;

            df = DreamFall();
            StartCoroutine(df);
        }

        private IEnumerator DreamFall()
        {
            #region Freeze Camera
            c.FreezeInPlace(true);
            #endregion

            #region Fade Out

            foreach (var item in hGo.GetComponentsInChildren<PlayMakerFSM>())
            {
                item.SendEvent("FSM CANCEL");
            }
            hGo.LocateMyFSM("ProxyFSM").FsmVariables.GetFsmBool("No Charms").Value = false;
            h.RelinquishControl();
            // Spawn "Audio Player Actor" at the knight's position with the "dream_enter" audioclip, at 1-1 pitch with 1 volume, 0 delay, maybe store?
            h.StopAnimationControl();
            foreach (var item in GameObject.Find("HUD Blanker White").GetComponentsInChildren<PlayMakerFSM>())
                item.SendEvent("FADE IN");
            GameObject.Find("HUD Blanker White").LocateMyFSM("Blanker Control").FsmVariables.GetFsmFloat("Fade Time").Value = 0.9f;
            foreach (var item in GameObject.Find("HUD Blanker White").GetComponentsInChildren<PlayMakerFSM>())
                item.SendEvent("FADE IN");
            yield return new WaitForSeconds(1);
            #endregion

            #region New Scene
            g.TimePasses();
            g.ResetSemiPersistentItems();
            GameObject.Find("MainCamera").LocateMyFSM("CameraFade").FsmVariables.GetFsmBool("No Fade").Value = true;
            hGo.LocateMyFSM("Dream Return").FsmVariables.GetFsmBool("Dream Returning").Value = true;
            h.EnterWithoutInput(true);
            #endregion

            #region Waterways
            // begin the scene transition, 0 delay, enum 0, preventfade true
            #endregion
        }
    }
}