using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UniJS
{
    using Payloads;
    using InstanceTools;
    
    public class JSInstance : MonoBehaviour
    {
        private static JSInstance instance;
        
        [DllImport("__Internal")]
        private static extern void Lib_InstanceReady();
        
        [DllImport("__Internal")]
        private static extern void Lib_RegisterKeyGameObject(string key, string dataJson);
        
        [DllImport("__Internal")]
        private static extern void Lib_LogToJS(string verbosity, string message);
        
        private static Dictionary<string, GameObject> _keyGameObjects = new();
        
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            JSClientEventHandler.StartListeningToClientEvents();
            JSEventHub.Initialize();
            RegisterInstanceEvents();
            StartCoroutine(WaitForRestOfSceneAwake());
        }

        public static void RegisterKeyGameObject(string key, GameObject go)
        {
            _keyGameObjects[key] = go;
            var data = new JSGameObjectData(go);
            Lib_RegisterKeyGameObject(key, JsonUtility.ToJson(data));
        }

        public static Guid OnEvent<TPayload>(string eventName, Action<TPayload> callback)
        {
            return OnEvent<TPayload, string>(eventName, payload =>
            {
                callback.Invoke(payload);
                return "<ok>";
            });
        }
        
        public static Guid OnEvent<TPayload, TResult>(string eventName, Func<TPayload, TResult> callback)
        {
            return JSClientEventHandler.OnClientEventReceived(eventName, callback);
        }
        
        public static Guid OnEventAsync<TPayload>(string eventName, Func<TPayload, Task> callback)
        {
            return OnEventAsync<TPayload, string>(eventName, async payload =>
            {
                await callback.Invoke(payload);
                return "<ok>";
            });
        }
        
        public static Guid OnEventAsync<TPayload, TResult>(string eventName, Func<TPayload, Task<TResult>> callback)
        {
            return JSClientEventHandler.OnClientEventReceived<TPayload, PromisePayload>(eventName, payload =>
            {
                var promise = new PromisePayload();
                AsyncCallback(payload, promise.promiseId);
                return promise;
            });

            async Task AsyncCallback(TPayload payload, string promiseId)
            {
                await Task.Yield();
                var result = await callback(payload);
                InvokeEvent($"PromiseResolvedEvent:{promiseId}", result);
            }
        }
        
        public static void OffEvent(string eventName, Guid callbackId)
        {
            JSClientEventHandler.OffClientEventReceived(eventName, callbackId);
        }
        
        public static void InvokeEvent(string eventName, object payload)
        {
            JSClientEventHandler.SendEventToClient(eventName, payload);
        }
        
        public static void InvokeEvent(string eventName)
        {
            JSClientEventHandler.SendEventToClient(eventName,"");
        }
        
        public static void Log(string message)
        {
            Log("info", message);
        }
        
        public static void LogError(string message)
        {
            Log("error", message);
        }
        
        public static void LogWarning(string message)
        {
            Log("warning", message);
        }
        
        public static void LogInternal(string message)
        {
            Log("internal", message);
        }

        public static void Log(string verbosity, string message)
        {
            Lib_LogToJS(verbosity, message);
        }

        private static void RegisterInstanceEvents()
        {
            OnEvent<string>("InstanceEvent:WaitForEndOfFrame", eventId =>
            {
                if (instance == null)
                {
                    LogError("JSInstance is not initialized.");
                    return;
                }
                instance.StartCoroutine(SendEndOfFrameMessage(eventId));
            });

            OnEvent<string, string>("InstanceEvent:GetUnityVersion", _ => Application.unityVersion);
            OnEvent<string, string>("InstanceEvent:GetBuildVersion", _ => Application.version);

            OnEvent<string>("InstanceEvent:LoadScene", sceneName =>
            {
                LogInternal($"Loading scene {sceneName}");
                JSSceneManager.LoadSceneAsync(sceneName, () =>
                    {
                        LogInternal($"Scene {sceneName} loaded successfully");
                        InvokeEvent($"SceneLoadedEvent:{sceneName}");
                    },
                    error => LogError($"Failed to load scene {sceneName}: {error}"));
            });

            OnEvent<string, bool>("InstanceEvent:IsSceneLoading", _ => JSSceneManager.IsSceneLoading());
            OnEvent<string, float>("InstanceEvent:GetSceneLoadProgress", _ => JSSceneManager.GetSceneLoadProgress());
            
            OnEventAsync<string>("InstanceEvent:LoadBundle", async bundleUrl =>
            {
                await JSAssetBundleLoader.LoadBundle(bundleUrl);
            });

            OnEventAsync<InstantiatePrefabFromBundlePayload>("InstanceEvent:InstantiatePrefabFromBundle", async payload =>
            {
                if (!string.IsNullOrEmpty(payload.parentKey))
                {
                    if (_keyGameObjects.TryGetValue(payload.parentKey, out var parent))
                    {
                        await JSAssetBundleLoader.InstantiatePrefabFromBundle(payload.bundleUrl, payload.prefabName, parent.transform);
                        return;
                    }
                }
                await JSAssetBundleLoader.InstantiatePrefabFromBundle(payload.bundleUrl, payload.prefabName);
            });
        }
        
        private static IEnumerator WaitForRestOfSceneAwake()
        {
            yield return new WaitForEndOfFrame();
            Lib_InstanceReady();
        }

        private static IEnumerator SendEndOfFrameMessage(string eventId)
        {
            yield return new WaitForEndOfFrame();
            InvokeEvent($"EndOfFrameEvent:{eventId}");
        }
    }
}