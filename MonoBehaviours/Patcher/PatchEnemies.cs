using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
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
                GameObject tmp = GameObject.Instantiate(PrefabHolder.wpFlyPrefab, transform.parent);
                tmp.name = gameObject.name;
                tmp.transform.localPosition = transform.localPosition;
                tmp.transform.localScale = transform.localScale / 1.4f;
                tmp.transform.localEulerAngles = transform.localEulerAngles;
                tmp.SetActive(true);
                GameObject.Destroy(gameObject);
            }
            else if (type == EnemyType.SAW_NO_SOUND)
            {
                GameObject tmp = GameObject.Instantiate(PrefabHolder.wpSawNoSoundPrefab, transform.parent);
                tmp.name = gameObject.name;
                tmp.transform.localPosition = transform.localPosition;
                tmp.transform.localScale = transform.localScale / 1.56f;
                tmp.transform.localEulerAngles = transform.localEulerAngles;
                tmp.SetActive(true);
                GameObject.Destroy(gameObject);
            }
            else if (type == EnemyType.SAW_SOUND)
            {
                GameObject tmp = GameObject.Instantiate(PrefabHolder.wpSawWithSoundPrefab, transform.parent);
                tmp.name = gameObject.name;
                tmp.transform.localPosition = transform.localPosition;
                tmp.transform.localScale = transform.localScale / 1.56f;
                tmp.transform.localEulerAngles = transform.localEulerAngles;
                tmp.SetActive(true);
                GameObject.Destroy(gameObject);
            }
            else if (type == EnemyType.MOVING_SAW_SOUND)
            {
                GameObject tmp = transform.GetChild(0).gameObject;
                GameObject tmpPrefab = GameObject.Instantiate(PrefabHolder.wpSawWithSoundPrefab, transform);
                tmpPrefab.name = gameObject.name;
                tmpPrefab.transform.localPosition = tmp.transform.localPosition;
                tmpPrefab.transform.localScale = tmp.transform.localScale / 1.56f;
                tmpPrefab.transform.localEulerAngles = tmp.transform.localEulerAngles;
                tmpPrefab.SetActive(true);
                GameObject.Destroy(tmp);
            }
            else if (type == EnemyType.INF_SOUL_TOTEM)
            {
                GameObject tmp = GameObject.Instantiate(PrefabHolder.wpInfSoulTotemPrefab);
                tmp.name = gameObject.name;
                tmp.transform.localPosition = transform.localPosition;
                tmp.transform.localScale = transform.localScale / 1.5f;
                tmp.transform.localEulerAngles = transform.localEulerAngles;
                tmp.SetActive(true);
                var tmpFsm = tmp.LocateMyFSM("soul_totem");
                tmpFsm.SetState("Pause Frame");
                tmpFsm.SendEvent("RESET");
                GameObject.Destroy(gameObject);
            }
            else if (type == EnemyType.WHITE_SPIKES)
            {
                GameObject tmp = GameObject.Instantiate(PrefabHolder.wpSpikesPrefab, transform.parent);
                tmp.name = gameObject.name;
                tmp.transform.localPosition = transform.localPosition;
                tmp.transform.localScale = new Vector3(transform.localScale.x / 1.625f, transform.localScale.y / 1.45f, transform.localScale.z / 1.5f);
                tmp.transform.localEulerAngles = transform.localEulerAngles;
                tmp.SetActive(true);
                GameObject.Destroy(gameObject);
            }
        }
    }
}
