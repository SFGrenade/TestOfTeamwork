using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    class PatchTinkEffect : MonoBehaviour
    {
        public void Start()
        {
            var te = gameObject.AddComponent<TinkEffect>();
            te = gameObject.GetComponent<TinkEffect>();
            te.blockEffect = PrefabHolder.wpTinkEffectPrefab.blockEffect;
            te.useNailPosition = PrefabHolder.wpTinkEffectPrefab.useNailPosition;
            te.sendFSMEvent = PrefabHolder.wpTinkEffectPrefab.sendFSMEvent;
            te.FSMEvent = PrefabHolder.wpTinkEffectPrefab.FSMEvent;
            te.fsm = PrefabHolder.wpTinkEffectPrefab.fsm;
            te.sendDirectionalFSMEvents = PrefabHolder.wpTinkEffectPrefab.sendDirectionalFSMEvents;
        }
    }
}
