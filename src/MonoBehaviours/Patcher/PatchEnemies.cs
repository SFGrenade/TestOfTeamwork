using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher;

class PatchEnemies : MonoBehaviour
{
    public enum EnemyType
    {
        WP_FLY,
        SAW_NO_SOUND,
        SAW_SOUND,
        MOVING_SAW_SOUND,
        INF_SOUL_TOTEM,
        WHITE_SPIKES
    }

    public EnemyType type;

    public void Start()
    {
    }
}