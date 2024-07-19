using UnityEngine;
using Logger = Modding.Logger;
using USM = UnityEngine.SceneManagement.SceneManager;

namespace TestOfTeamwork.MonoBehaviours;

class SceneLogger : MonoBehaviour
{
    public void Update()
    {
        var loadedScenes = $"[\"{USM.GetSceneAt(0).name}\"";
        for (int i = 1; i < USM.sceneCount; i++)
        {
            loadedScenes += $", \"{USM.GetSceneAt(i).name}\"";
        }
        loadedScenes += "]";
        Log($"{USM.sceneCount} loaded scenes: {loadedScenes}");
    }

    private void Log(string message)
    {
        Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
    }

    private void Log(object message)
    {
        Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
    }
}