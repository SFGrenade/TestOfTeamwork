using System;
using Modding;
using UnityEngine;
using Logger = Modding.Logger;

namespace TestOfTeamwork.MonoBehaviours.Patcher;

public class PatchEnemyDreamnailReaction : MonoBehaviour
{
    public int ConvoAmount = 1;
    public string ConvoTitle = "";
    public bool StartSuppressed = false;
    public bool NoSoul = false;
    public bool AllowUseChildColliders = true;

    public void Start()
    {
        try
        {
            //PrefabHolder.PopSobPartPrefab.SetActive(false);
            EnemyDreamnailReaction edr = gameObject.AddComponent<EnemyDreamnailReaction>();
            ReflectionHelper.SetField<EnemyDreamnailReaction, int>(edr, "convoAmount", ConvoAmount);
            ReflectionHelper.SetField<EnemyDreamnailReaction, string>(edr, "convoTitle", ConvoTitle);
            ReflectionHelper.SetField<EnemyDreamnailReaction, bool>(edr, "startSuppressed", StartSuppressed);
            ReflectionHelper.SetField<EnemyDreamnailReaction, bool>(edr, "noSoul", NoSoul);
            ReflectionHelper.SetField<EnemyDreamnailReaction, GameObject>(edr, "dreamImpactPrefab", ReflectionHelper.GetField<EnemyDreamnailReaction, GameObject>(PrefabHolder.Hornet2BossPrefab.GetComponent<EnemyDreamnailReaction>(), "dreamImpactPrefab"));
            edr.allowUseChildColliders = AllowUseChildColliders;
        }
        catch (Exception e)
        {
            Debug.Log("PatchEnemyDreamnailReaction - " + e);
        }
    }
}