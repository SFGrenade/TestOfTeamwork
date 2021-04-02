using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    class PatchAreaTitleController : MonoBehaviour
    {
        [Range(0, 10)]
        public float Pause = 3f;
        public bool AlwaysVisited = false;
        public bool DisplayRight = false;
        public bool OnlyOnRevisit = false;
        public bool SubArea = true;
        public bool WaitForTrigger = false;
        public string AreaEvent = "";
        public string VisitedBool = "";

        public void Awake()
        {
            GameObject atc = GameObject.Instantiate(PrefabHolder.popAreaTitleCtrlPrefab);
            atc.SetActive(false);
            atc.transform.localPosition = transform.position;
            atc.transform.localEulerAngles = transform.eulerAngles;
            atc.transform.localScale = transform.lossyScale;

            PlayMakerFSM atcFsm = atc.LocateMyFSM("Area Title Controller");
            atcFsm.FsmVariables.GetFsmFloat("Unvisited Pause").Value = Pause;
            atcFsm.FsmVariables.GetFsmFloat("Visited Pause").Value = Pause;

            atcFsm.FsmVariables.GetFsmBool("Always Visited").Value = AlwaysVisited;
            atcFsm.FsmVariables.GetFsmBool("Display Right").Value = DisplayRight;
            atcFsm.FsmVariables.GetFsmBool("Only On Revisit").Value = OnlyOnRevisit;
            atcFsm.FsmVariables.GetFsmBool("Sub Area").Value = SubArea;
            atcFsm.FsmVariables.GetFsmBool("Visited Area").Value = PlayerData.instance.GetBool("SFGrenadeTestOfTeamworkVisitedTestOfTeamwork");
            atcFsm.FsmVariables.GetFsmBool("Wait for Trigger").Value = WaitForTrigger;

            atcFsm.FsmVariables.GetFsmString("Area Event").Value = AreaEvent;
            atcFsm.FsmVariables.GetFsmString("Visited Bool").Value = VisitedBool;

            atcFsm.FsmVariables.GetFsmGameObject("Area Title").Value = GameObject.Find("Area Title");

            atc.AddComponent<NonBouncer>();

            atc.SetActive(true);

            GameObject.Destroy(this.gameObject);
        }
    }
}
