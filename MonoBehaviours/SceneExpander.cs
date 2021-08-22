using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Modding.Logger;

namespace TestOfTeamwork.MonoBehaviours
{
    public class SceneExpander : MonoBehaviour
    {
        public Action<Scene, int> expandingAction;
        public Scene scene;
        private GameManager _gm;
        private CameraController _cc;

        private void Awake()
        {
            _gm = GameManager.instance;
            _cc = FindObjectOfType<CameraController>();
        }

        public void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void Update()
        {
            if ((gameObject == null) || (!((_gm.sceneWidth - transform.position.x) <= 30f))) return;

            expandingAction.Invoke(scene, (int)(_gm.sceneWidth / 32));
            _gm.tilemap.width += 32;
            _gm.sceneWidth += 32;
            _cc.xLimit += 32;
        }

        private void Log(string message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }

        private void Log(object message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }
    }
}