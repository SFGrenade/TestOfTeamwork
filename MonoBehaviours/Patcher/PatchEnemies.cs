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
        if (type == EnemyType.WP_FLY)
        {
            GameObject tmp = Instantiate(PrefabHolder.WpFlyPrefab, transform.parent);
            tmp.name = gameObject.name;
            tmp.transform.localPosition = transform.localPosition;
            tmp.transform.localScale = transform.localScale;
            tmp.transform.localEulerAngles = transform.localEulerAngles;
            tmp.SetActive(true);
            Destroy(gameObject);
        }
        else if (type == EnemyType.SAW_NO_SOUND)
        {
            GameObject tmp = Instantiate(PrefabHolder.WpSawNoSoundPrefab, transform.parent);
            tmp.name = gameObject.name;
            tmp.transform.localPosition = transform.localPosition;
            tmp.transform.localScale = transform.localScale;
            tmp.transform.localEulerAngles = transform.localEulerAngles;
            tmp.SetActive(true);
            Destroy(gameObject);
        }
        else if (type == EnemyType.SAW_SOUND)
        {
            GameObject tmp = Instantiate(PrefabHolder.WpSawWithSoundPrefab, transform.parent);
            tmp.name = gameObject.name;
            tmp.transform.localPosition = transform.localPosition;
            tmp.transform.localScale = transform.localScale;
            tmp.transform.localEulerAngles = transform.localEulerAngles;
            tmp.SetActive(true);
            Destroy(gameObject);
        }
        else if (type == EnemyType.MOVING_SAW_SOUND)
        {
            GameObject tmp = transform.GetChild(0).gameObject;
            GameObject tmpPrefab = Instantiate(PrefabHolder.WpSawWithSoundPrefab, transform);
            tmpPrefab.name = gameObject.name;
            tmpPrefab.transform.localPosition = tmp.transform.localPosition;
            tmpPrefab.transform.localScale = tmp.transform.localScale;
            tmpPrefab.transform.localEulerAngles = tmp.transform.localEulerAngles;
            tmpPrefab.SetActive(true);
            Destroy(tmp);
        }
        else if (type == EnemyType.INF_SOUL_TOTEM)
        {
            GameObject tmp = Instantiate(PrefabHolder.WpInfSoulTotemPrefab);
            tmp.name = gameObject.name;
            tmp.transform.localPosition = transform.localPosition;
            tmp.transform.localScale = transform.localScale;
            tmp.transform.localEulerAngles = transform.localEulerAngles;
            tmp.SetActive(true);
            var tmpFsm = tmp.LocateMyFSM("soul_totem");
            tmpFsm.SetState("Pause Frame");
            tmpFsm.SendEvent("RESET");
            Destroy(gameObject);
        }
        else if (type == EnemyType.WHITE_SPIKES)
        {
            GameObject tmp = Instantiate(PrefabHolder.WpSpikesPrefab, transform.parent);
            tmp.name = gameObject.name;
            tmp.transform.localPosition = transform.localPosition;
            tmp.transform.localScale = transform.localScale;
            tmp.transform.localEulerAngles = transform.localEulerAngles;
            tmp.SetActive(true);
            Destroy(gameObject);
        }
    }
}