using ModCommon.Util;
using UnityEngine;

namespace TestOfTeamwork.MonoBehaviours.Patcher
{
    public class PatchSceneManager : MonoBehaviour
    {
        public enum SceneType
        {
            GAMEPLAY,
            MENU,
            LOADING,
            CUTSCENE,
            TEST
        }
        public enum MapZone
        {
            NONE,
            TEST_AREA,
            KINGS_PASS,
            CLIFFS,
            TOWN,
            CROSSROADS,
            GREEN_PATH,
            ROYAL_GARDENS,
            FOG_CANYON,
            WASTES,
            DEEPNEST,
            HIVE,
            BONE_FOREST,
            PALACE_GROUNDS,
            MINES,
            RESTING_GROUNDS,
            CITY,
            DREAM_WORLD,
            COLOSSEUM,
            ABYSS,
            ROYAL_QUARTER,
            WHITE_PALACE,
            SHAMAN_TEMPLE,
            WATERWAYS,
            QUEENS_STATION,
            OUTSKIRTS,
            KINGS_STATION,
            MAGE_TOWER,
            TRAM_UPPER,
            TRAM_LOWER,
            FINAL_BOSS,
            SOUL_SOCIETY,
            ACID_LAKE,
            NOEYES_TEMPLE,
            MONOMON_ARCHIVE,
            MANTIS_VILLAGE,
            RUINED_TRAMWAY,
            DISTANT_VILLAGE,
            ABYSS_DEEP,
            ISMAS_GROVE,
            WYRMSKIN,
            LURIENS_TOWER,
            LOVE_TOWER,
            GLADE,
            BLUE_LAKE,
            PEAK,
            JONI_GRAVE,
            OVERGROWN_MOUND,
            CRYSTAL_MOUND,
            BEASTS_DEN,
            GODS_GLORY,
            GODSEEKER_WASTE
        }

        public SceneType sceneType = SceneType.GAMEPLAY;
        public MapZone mapZone = MapZone.WHITE_PALACE;
        public bool isWindy = false;
        public bool isTremorZone = false;
        public int environmentType = 0;
        public int darknessLevel = 0;
        public bool noLantern = false;
        public float saturation = 0.5f;
        public bool ignorePlatformSaturationModifiers = false;
        public AnimationCurve redChannel = new AnimationCurve(
            new Keyframe[]
            {
                new Keyframe(0, 0),
                new Keyframe(1, 1), 
            }
        );
        public AnimationCurve greenChannel = new AnimationCurve(
            new Keyframe[]
            {
                new Keyframe(0, 0),
                new Keyframe(1, 1),
            }
        );
        public AnimationCurve blueChannel = new AnimationCurve(
            new Keyframe[]
            {
                new Keyframe(0, 0),
                new Keyframe(1, 1),
            }
        );
        public Color defaultColor = new Color(1f, 1f, 1f, 1f);
        public float defaultIntensity = 0.9f;
        public Color heroLightColor = new Color(1f, 1f, 1f, 0.316f);
        public bool noParticles = false;
        public MapZone overrideParticlesWith = MapZone.NONE;
        public float transitionTime = 0.5f;
        public bool manualMapTrigger = false;
        public float musicTransitionTime = 3.0f;

        public void Awake()
        {
            GameObject actualSceneManager = GameObject.Instantiate(PrefabHolder.popSceneManagerPrefab);
            actualSceneManager.SetActive(false);
            actualSceneManager.name = "_SceneManager";

            var sm = actualSceneManager.GetComponent<SceneManager>();
            sm.sceneType = (GlobalEnums.SceneType)sceneType;
            sm.mapZone = (GlobalEnums.MapZone)mapZone;
            sm.isWindy = isWindy;
            sm.isTremorZone = isTremorZone;
            sm.environmentType = environmentType;
            sm.darknessLevel = darknessLevel;
            sm.noLantern = noLantern;
            sm.saturation = saturation;
            sm.ignorePlatformSaturationModifiers = ignorePlatformSaturationModifiers;
            sm.redChannel = redChannel;
            sm.greenChannel = greenChannel;
            sm.blueChannel = blueChannel;
            sm.defaultColor = defaultColor;
            sm.defaultIntensity = defaultIntensity;
            sm.heroLightColor = heroLightColor;
            sm.noParticles = noParticles;
            sm.overrideParticlesWith = (GlobalEnums.MapZone)overrideParticlesWith;
            sm.transitionTime = transitionTime;
            sm.manualMapTrigger = manualMapTrigger;
            sm.SetAttr("musicTransitionTime", musicTransitionTime);

            actualSceneManager.SetActive(true);
            GameObject.Destroy(gameObject);
        }
    }
}
