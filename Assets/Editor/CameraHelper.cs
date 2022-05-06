using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FixCameraLockArea
{
    [MenuItem("Camera/Fix CLAs")]
    static void FixCLAs()
    {
        CameraLockArea[] ec2dList = GameObject.FindObjectsOfType<CameraLockArea>();
        foreach (CameraLockArea ec2d in ec2dList) {
            if (ec2d.transform.childCount > 0) {
                var t = ec2d.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
                var u = t.bounds;
                ec2d.cameraXMin = u.min.x + 14.6f;
                ec2d.cameraYMin = u.min.y + 8.3f;
                ec2d.cameraXMax = u.max.x - 14.6f;
                ec2d.cameraYMax = u.max.y - 8.3f;
            }
        }
    }
}
