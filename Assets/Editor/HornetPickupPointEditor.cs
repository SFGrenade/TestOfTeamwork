using UnityEngine;
using System.Collections;
using UnityEditor;
using TestOfTeamwork.MonoBehaviours;

[CustomEditor(typeof(HornetPickupPoint))]
public class HornetPickupPointEditor : Editor 
{
    //public override void OnInspectorGUI()
    //{
    //    HornetPickupPoint hpup = (HornetPickupPoint)target;
    //
    //    hpup.speed = EditorGUILayout.FloatField("Speed", hpup.speed);
    //}

    void OnSceneGUI()
    {
        var hpup = target as HornetPickupPoint;
        var transform = hpup.transform;

        if (hpup.points == null)
            return;
        
        var positions = new Vector3[hpup.points.Length];
        for (int i = 0; i < hpup.points.Length; i++) {
            Vector3 t = hpup.points[i];
            positions[i] = transform.position + t;
        }
        Handles.DrawPolyLine(positions);
    }
}
