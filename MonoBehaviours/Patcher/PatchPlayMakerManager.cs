using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    class PatchPlayMakerManager : MonoBehaviour
    {
        public Transform managerTransform;

        public void Awake()
        {
            GameObject tmpPMU2D = GameObject.Instantiate(PrefabHolder.popPmU2dPrefab, managerTransform);
            tmpPMU2D.SetActive(true);
            tmpPMU2D.name = "PlayMaker Unity 2D";
        }
    }
}
