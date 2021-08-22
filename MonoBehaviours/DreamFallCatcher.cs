using System.Collections;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours
{
    public class DreamFallCatcher : MonoBehaviour
    {
        private HeroController _h;
        private GameObject _hGo;
        private GameManager _g;
        private CameraController _c;
        private IEnumerator _df;

        private void Start()
        {
            _h = HeroController.instance;
            _hGo = _h.gameObject;
            _g = GameManager.instance;
            _c = _g.cameraCtrl;
            _df = null;
        }

        private void FixedUpdate()
        {
            if (_h.transform.position.y > 0) return;
            if (_df != null) return;

            _df = DreamFall();
            StartCoroutine(_df);
        }

        private IEnumerator DreamFall()
        {
            #region Freeze Camera
            _c.FreezeInPlace(true);
            #endregion

            #region Fade Out

            foreach (var item in _hGo.GetComponentsInChildren<PlayMakerFSM>())
            {
                item.SendEvent("FSM CANCEL");
            }
            _hGo.LocateMyFSM("ProxyFSM").FsmVariables.GetFsmBool("No Charms").Value = false;
            _h.RelinquishControl();
            // Spawn "Audio Player Actor" at the knight's position with the "dream_enter" audioclip, at 1-1 pitch with 1 volume, 0 delay, maybe store?
            _h.StopAnimationControl();
            foreach (var item in GameObject.Find("HUD Blanker White").GetComponentsInChildren<PlayMakerFSM>())
                item.SendEvent("FADE IN");
            GameObject.Find("HUD Blanker White").LocateMyFSM("Blanker Control").FsmVariables.GetFsmFloat("Fade Time").Value = 0.9f;
            foreach (var item in GameObject.Find("HUD Blanker White").GetComponentsInChildren<PlayMakerFSM>())
                item.SendEvent("FADE IN");
            yield return new WaitForSeconds(1);
            #endregion

            #region New Scene
            _g.TimePasses();
            _g.ResetSemiPersistentItems();
            GameObject.Find("MainCamera").LocateMyFSM("CameraFade").FsmVariables.GetFsmBool("No Fade").Value = true;
            _hGo.LocateMyFSM("Dream Return").FsmVariables.GetFsmBool("Dream Returning").Value = true;
            _h.EnterWithoutInput(true);
            #endregion

            #region Waterways
            // begin the scene transition, 0 delay, enum 0, preventfade true
            #endregion
        }
    }
}