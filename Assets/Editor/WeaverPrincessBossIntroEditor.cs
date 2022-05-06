using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using TestOfTeamwork.MonoBehaviours;

[CustomEditor(typeof(WeaverPrincessBossIntro))]
public class WeaverPrincessBossIntroEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("EncounterPdBoolName"), new GUIContent("Encounter PD Bool Name"));
        GUIStyle style = new GUIStyle();
        style.margin = new RectOffset(2, 2, 2, 2);
        style.padding = new RectOffset(2, 2, 2, 2);

        Rect r = EditorGUILayout.BeginVertical(style);
        GUI.Box(r, GUIContent.none);
        EditorGUILayout.LabelField("Convo 01:");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Dialogue1ConvoSheet"), new GUIContent("Sheet"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Dialogue1ConvoKey"), new GUIContent("Key"));
        EditorGUILayout.EndVertical();
        Rect r2 = EditorGUILayout.BeginVertical(style);
        GUI.Box(r2, GUIContent.none);
        EditorGUILayout.LabelField("Convo 02:");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Dialogue2ConvoSheet"), new GUIContent("Sheet"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Dialogue2ConvoKey"), new GUIContent("Key"));
        EditorGUILayout.EndVertical();
        
        //Rect r3 = EditorGUILayout.BeginVertical(style);
        //GUI.Box(r3, GUIContent.none);
        //EditorGUILayout.LabelField("Audio:");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FightSnapShot"), new GUIContent("Audio Snapshot"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FightAudioClips"), new GUIContent("Audio Clips"));
        //EditorGUILayout.EndVertical();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BossGo"), new GUIContent("Boss Game Object"));
        
        //Save all changes made on the inspector
        serializedObject.ApplyModifiedProperties();
    }
}
