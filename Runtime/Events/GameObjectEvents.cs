using System;
using UnityEngine;
using UniJS.Payloads;

namespace UniJS.Events
{
    [ExposeJSEvent("gameObject.setActive")]
    public class Event_GameObjectSetActive : JSEventVoid<GameObject, bool> 
    {
        protected override void Invoke(GameObject target, bool state)
        {
            target.SetActive(state);
        }
    }
    
    [ExposeJSEvent("gameObject.invokeMethod")]
    public class Event_GameObjectInvokeMethod : JSEventVoid<GameObject, MethodInvokePayload> 
    {
        protected override void Invoke(GameObject target, MethodInvokePayload payload)
        {
            if (string.IsNullOrEmpty(payload.parameterType))
            {
                target.SendMessage(payload.methodName);
                return;
            }

            var type = Type.GetType(payload.parameterType);
            if(type == null)
            {
                //TODO: Check why it's failing to parse class types
                JSInstance.LogError($"Failed to invoke method '{payload.methodName}' on GameObject '{target.name}' with parameter of type '{payload.parameterType}'. Parameter type failed to parse.");
                return;
            }
            
            if (PayloadParser.TryParse(payload.parameterValue, type, out var parameter))
            {
                target.SendMessage(payload.methodName, parameter);
            }
            else
            {
                JSInstance.LogError($"Failed to invoke method '{payload.methodName}' on GameObject '{target.name}' with parameter '{payload.parameterValue}' of type '{payload.parameterType}'. Parameter value failed to parse.");
            }
        }
    }
    
    [ExposeJSEvent("gameObject.getChild")]
    public class Event_GameObjectGetChild : JSEvent<GameObject, int, JSGameObjectData>
    {
        protected override JSGameObjectData Invoke(GameObject target, int childIndex)
        {
            var child = target.transform.GetChild(childIndex);
            return child == null ? null : new JSGameObjectData(child.gameObject);
        }
    }
    
    [ExposeJSEvent("gameObject.findChild")]
    public class Event_GameObjectFindChild : JSEvent<GameObject, string, JSGameObjectData>
    {
        protected override JSGameObjectData Invoke(GameObject target, string name)
        {
            var child = target.transform.Find(name);
            return child == null ? null : new JSGameObjectData(child.gameObject);
        }
    }
    
    [ExposeJSEvent("gameObject.destroy")]
    public class Event_GameObjectDestroy : JSEventVoid<GameObject, string> 
    {
        protected override void Invoke(GameObject target, string _)
        {
            GameObject.Destroy(target);
        }
    }
    
    [ExposeJSEvent("gameObject.hasComponent")]
    public class Event_GameObjectHasComponent : JSEvent<GameObject, string, bool>
    {
        protected override bool Invoke(GameObject target, string componentName)
        {
            return target.GetComponent(componentName) != null;
        }
    }
    
    [ExposeJSEvent("gameObject.getComponent")]
    public class Event_GameObjectGetComponent : JSEvent<GameObject, string, object>
    {
        protected override object Invoke(GameObject target, string componentName)
        {
            var component = target.GetComponent(componentName);
            return component == null ? null : ComponentPayloadConverter.Convert(component);
        }
    }
    
    [ExposeJSEvent("gameObject.addComponent")]
    public class Event_GameObjectAddComponent : JSEvent<GameObject, string, object>
    {
        protected override object Invoke(GameObject target, string componentName)
        {
            var component = target.AddComponent(Type.GetType(componentName));
            return component == null ? null : ComponentPayloadConverter.Convert(component);
        }
    }
    
    [ExposeJSEvent("gameObject.instantiate")]
    public class Event_GameObjectInstantiate : JSEvent<GameObject, InstantiatePayload, JSGameObjectData>
    {
        protected override JSGameObjectData Invoke(GameObject target, InstantiatePayload payload)
        {
            var position = new Vector3(payload.position.x, payload.position.y, payload.position.z);
            var rotation = new Quaternion(payload.rotation.x, payload.rotation.y, payload.rotation.z, payload.rotation.w);
            var instantiatedObject = GameObject.Instantiate(Resources.Load<GameObject>(payload.prefabPath), position, rotation, target.transform);
            return new JSGameObjectData(instantiatedObject);
        }
    }
    
}