using TestOfTeamwork.Consts;
using UnityEngine;
using Logger = Modding.Logger;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    public class PatchDreamEnter : MonoBehaviour
    {
        public string EnterDoorName = "";
        public Vector2 EnterPosition = Vector2.zero;

        public void Start()
        {
            GameObject door1 = GameObject.Instantiate(PrefabHolder.wp03Door);
            door1.SetActive(true);
            door1.name = EnterDoorName;
            door1.transform.position = EnterPosition;

            GameObject dreamEntry = GameObject.Instantiate(PrefabHolder.wp03Dream);
            dreamEntry.SetActive(true);
            dreamEntry.name = "Dream Entry";
            dreamEntry.transform.position = Vector3.zero;
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
