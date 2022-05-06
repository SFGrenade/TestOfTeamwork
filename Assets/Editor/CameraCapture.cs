using System.IO;
using UnityEngine;
using UnityEditor;
    
public class CameraCapture : MonoBehaviour
{
    protected static int fileCounter;
    
    [MenuItem("Camera/Render View")]
    static public void Capture()
    {
		Camera camera = GameObject.FindObjectOfType<Camera> ();

        if (camera.targetTexture == null)
        {
            camera.targetTexture = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        }

        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
    
        camera.Render();
    
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;
    
        byte[] bytes = image.EncodeToPNG();
        DestroyImmediate(image);
    
        File.WriteAllBytes("E:\\Github_Projects\\__Exports\\TMP\\"+fileCounter+".png", bytes);
        fileCounter++;
    }
}
