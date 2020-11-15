using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestOfTeamwork.Utils
{
    public static class USceneUtil
    {
        public static GameObject FindRoot(this Scene scene, string name)
        {
            return scene.GetRootGameObjects().FirstOrDefault(go => go.name == name);
        }

        public static GameObject Find(this Scene scene, string name)
        {
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == name)
                {
                    return go;
                }
                Transform ret = go.transform.Find(name);
                if (ret)
                {
                    return ret.gameObject;
                }
            }
            return null;
        }
    }
}