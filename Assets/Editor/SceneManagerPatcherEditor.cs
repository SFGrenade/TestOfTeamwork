using UnityEngine;
using System.Collections;
using UnityEditor;
using SFCore.MonoBehaviours;

[CustomEditor(typeof(SceneManagerPatcher))]
[CanEditMultipleObjects]
public class SceneManagerPatcherEditor : Editor
{
    string[] _musicChoices = new []
    {
        "Normal",
        "Normal Alt",
        "Normal Soft",
        "Normal Softer",
        "Normal Flange",
        "Normal Flangier",
        "Action",
        "Action and Sub",
        "Sub Area",
        "Silent",
        "Silent Flange",
        "Off",
        "Tension Only",
        "Normal - Gramaphone",
        "Action Only",
        "Main Only",
        "HK Decline 2",
        "HK Decline 3",
        "HK Decline 4",
        "HK Decline 5",
        "HK Decline 6"
    };
    string[] _atmosChoices = new []
    {
        "at None",
        "at Cave",
        "at Surface",
        "at Surface Interior",
        "at Surface Basement",
        "at Surface Nook",
        "at Rainy Indoors",
        "at Rainy Outdoors",
        "at Distant Rain",
        "at Distant Rain Room",
        "at Greenpath",
        "at Queens Gardens",
        "at Fungus",
        "at Fog Canyon",
        "at Waterways Flowing",
        "at Waterways",
        "at Greenpath Interior",
        "at Fog Canyon Minor",
        "at Mines Crystal",
        "at Mines Machinery",
        "at Deepnest",
        "at Deepnest Quiet",
        "at Wind Tunnel",
        "at Misc Wind"
    };
    string[] _enviroChoices = new []
    {
        "en Cave",
        "en Spa",
        "en Cliffs",
        "en Room",
        "en Arena",
        "en Sewerpipe",
        "en Fog Canyon",
        "en Dream",
        "en Silent"
    };
    string[] _actorChoices = new []
    {
        "On",
        "Off"
    };
    string[] _shadeChoices = new []
    {
        "Away",
        "Close"
    };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("mapZone"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isWindy"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isTremorZone"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("environmentType"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("darknessLevel"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("noLantern"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("saturation"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ignorePlatformSaturationModifiers"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("redChannel"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("greenChannel"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("blueChannel"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultColor"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultIntensity"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("heroLightColor"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("noParticles"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("overrideParticlesWith"), true);

        // --- AUDIO ---------------------------------------

        EditorGUILayout.PropertyField(serializedObject.FindProperty("AtmosCueSet"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AtmosCueSnapshotIndex"), new GUIContent("Atmos Cue Snapshot"));
        serializedObject.FindProperty("AtmosCueSnapshotName").stringValue = _atmosChoices[serializedObject.FindProperty("AtmosCueSnapshotIndex").intValue];
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AtmosCueIsChannelEnabled"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("MusicCueSet"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MusicCueSnapshotIndex"), new GUIContent("Music Cue Snapshot"));
        serializedObject.FindProperty("MusicCueSnapshotName").stringValue = _musicChoices[serializedObject.FindProperty("MusicCueSnapshotIndex").intValue];
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MusicCueChannelInfoClips"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MusicCueChannelInfoSyncs"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("InfectedMusicCueSet"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("InfectedMusicCueSnapshotIndex"), new GUIContent("Infected Music Cue Snapshot"));
        serializedObject.FindProperty("InfectedMusicCueSnapshotName").stringValue = _musicChoices[serializedObject.FindProperty("InfectedMusicCueSnapshotIndex").intValue];
        EditorGUILayout.PropertyField(serializedObject.FindProperty("InfectedMusicCueChannelInfoClips"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("InfectedMusicCueChannelInfoSyncs"), true);

        // --- AUDIO ---------------------------------------

        EditorGUILayout.PropertyField(serializedObject.FindProperty("musicDelayTime"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("musicTransitionTime"), true);

        // --- AUDIO ---------------------------------------
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MsSnapshotIndex"), new GUIContent("Music Snapshot"));
        serializedObject.FindProperty("MsSnapshotName").stringValue = _musicChoices[serializedObject.FindProperty("MsSnapshotIndex").intValue];
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AtsSnapshotIndex"), new GUIContent("Atmos Snapshot"));
        serializedObject.FindProperty("AtsSnapshotName").stringValue = _atmosChoices[serializedObject.FindProperty("AtsSnapshotIndex").intValue];
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EsSnapshotIndex"), new GUIContent("Enviro Snapshot"));
        serializedObject.FindProperty("EsSnapshotName").stringValue = _enviroChoices[serializedObject.FindProperty("EsSnapshotIndex").intValue];
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AcsSnapshotIndex"), new GUIContent("Actor Snapshot"));
        serializedObject.FindProperty("AcsSnapshotName").stringValue = _actorChoices[serializedObject.FindProperty("AcsSnapshotIndex").intValue];
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SsSnapshotIndex"), new GUIContent("Shade Snapshot"));
        serializedObject.FindProperty("SsSnapshotName").stringValue = _shadeChoices[serializedObject.FindProperty("SsSnapshotIndex").intValue];

        // --- AUDIO ---------------------------------------

        EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionTime"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("manualMapTrigger"), true);

        //Save all changes made on the inspector
        serializedObject.ApplyModifiedProperties();
    }
}
