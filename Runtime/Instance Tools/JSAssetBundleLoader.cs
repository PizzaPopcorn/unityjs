using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace UniJS.InstanceTools
{
    public class JSAssetBundleLoader
    {
        /// <summary>
        /// This function loads the bundle, instantiates a prefab in it, and then unloads the bundle after instantiating the prefab, so you don't have to handle it manually.
        /// </summary>
        /// <param name="bundleUrl">The path of the bundle</param>
        /// <param name="prefabName">The name of the prefab to instantiate</param>
        /// <param name="parent">(Optional) The returned object will be parented to this transform if provided</param>
        /// <returns>The instantiated GameObject</returns>
        public static async Task<GameObject> InstantiatePrefabFromBundle(string bundleUrl, string prefabName, Transform parent = null)
        {
            var bundle = await LoadBundle(bundleUrl);
            if (bundle == null) return null;
            var prefab = bundle.LoadAsset<GameObject>(prefabName);
            var go = parent == null 
                ? Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) 
                : Object.Instantiate(prefab, parent);
            bundle.Unload(false);
            return go;
        }

        public static async Task<AssetBundle> LoadBundle(string bundleUrl)
        {
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
            {
                var result = www.SendWebRequest();

                while (!result.isDone)
                {
                    await Task.Yield();
                }
     
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }

                return DownloadHandlerAssetBundle.GetContent(www);
            }
        }
    }
}