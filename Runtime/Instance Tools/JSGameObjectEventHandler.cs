using System.Runtime.InteropServices;
using UnityEngine;

namespace UniJS.InstanceTools
{
    using Payloads;
    
    internal class JSGameObjectEventHandler
    {
        [DllImport("__Internal")]
        private static extern void Lib_SendGameObjectLifeCycleEvent(string key, string eventName);
        
        public static void SendGameObjectLifeCycleEvent(string key, string eventName)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(eventName))
            {
                JSInstance.LogError("Invalid key or eventName provided for life cycle event.");
                return;
            }
            
            Lib_SendGameObjectLifeCycleEvent(key, eventName);
        }
        
        public static JSGameObjectData InstantiateGameObjectInRoot(InstantiatePayload payload)
        {
            var position = new Vector3(payload.position.x, payload.position.y, payload.position.z);
            var rotation = new Quaternion(payload.rotation.x, payload.rotation.y, payload.rotation.z, payload.rotation.w);
            var instantiatedObject = GameObject.Instantiate(Resources.Load<GameObject>(payload.prefabPath), position, rotation);
            return new JSGameObjectData(instantiatedObject);
        }
    }
}