using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher;

class PatchTinkEffect : MonoBehaviour
{
    public void Start()
    {
        var te = gameObject.AddComponent<TinkEffect>();
        te = gameObject.GetComponent<TinkEffect>();
        te.blockEffect = PrefabHolder.WpTinkEffectPrefab.blockEffect;
        te.useNailPosition = PrefabHolder.WpTinkEffectPrefab.useNailPosition;
        te.sendFSMEvent = PrefabHolder.WpTinkEffectPrefab.sendFSMEvent;
        te.FSMEvent = PrefabHolder.WpTinkEffectPrefab.FSMEvent;
        te.fsm = PrefabHolder.WpTinkEffectPrefab.fsm;
        te.sendDirectionalFSMEvents = PrefabHolder.WpTinkEffectPrefab.sendDirectionalFSMEvents;
    }
}