using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Modding.Logger;

namespace TestOfTeamwork.MonoBehaviours
{
    public class SceneExpander : MonoBehaviour
    {
        public Action<Scene, int> expandingAction;
        public Scene scene;
        private GameManager gm;
        private CameraController cc;

        private IEnumerator Start()
        {
            yield break;
        }

        private void Awake()
        {
            gm = GameManager.instance;
            cc = FindObjectOfType<CameraController>();
        }

        public void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void Update()
        {
            if ((gameObject == null) || (!((gm.sceneWidth - transform.position.x) <= 30f))) return;

            expandingAction.Invoke(scene, (int)(gm.sceneWidth / 32));
            gm.tilemap.width += 32;
            gm.sceneWidth += 32;
            cc.xLimit += 32;
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