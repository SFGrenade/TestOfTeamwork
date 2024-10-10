using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher;

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
    }
}