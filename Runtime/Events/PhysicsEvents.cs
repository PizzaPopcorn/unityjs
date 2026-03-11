using UniJS.Payloads;
using UnityEngine;

namespace UniJS.Events
{
    [JSExposedClass("physics.addForce")]
    public class Event_PhysicsAddForce : JSEventVoid<GameObject, Vector3Payload>
    {
        protected override void Invoke(GameObject target, Vector3Payload force)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(new Vector3(force.x, force.y, force.z));
            }
            else
            {
                JSInstance.LogError($"Failed to add force to GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.addTorque")]
    public class Event_PhysicsAddTorque : JSEventVoid<GameObject, Vector3Payload>
    {
        protected override void Invoke(GameObject target, Vector3Payload torque)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddTorque(new Vector3(torque.x, torque.y, torque.z));
            }
            else
            {
                JSInstance.LogError($"Failed to add torque to GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.setVelocity")]
    public class Event_PhysicsSetVelocity : JSEventVoid<GameObject, Vector3Payload>
    {
        protected override void Invoke(GameObject target, Vector3Payload velocity)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = new Vector3(velocity.x, velocity.y, velocity.z);
            }
            else
            {
                JSInstance.LogError($"Failed to set velocity on GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.getVelocity")]
    public class Event_PhysicsGetVelocity : JSEvent<GameObject, string, Vector3Payload>
    {
        protected override Vector3Payload Invoke(GameObject target, string _)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                var v = rb.velocity;
                return new Vector3Payload { x = v.x, y = v.y, z = v.z };
            }
            
            JSInstance.LogError($"Failed to get velocity from GameObject '{target.name}'. No Rigidbody component found.");
            return new Vector3Payload();
        }
    }

    [JSExposedClass("physics.setAngularVelocity")]
    public class Event_PhysicsSetAngularVelocity : JSEventVoid<GameObject, Vector3Payload>
    {
        protected override void Invoke(GameObject target, Vector3Payload velocity)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.angularVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
            }
            else
            {
                JSInstance.LogError($"Failed to set angular velocity on GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.getAngularVelocity")]
    public class Event_PhysicsGetAngularVelocity : JSEvent<GameObject, string, Vector3Payload>
    {
        protected override Vector3Payload Invoke(GameObject target, string _)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                var v = rb.angularVelocity;
                return new Vector3Payload { x = v.x, y = v.y, z = v.z };
            }
            
            JSInstance.LogError($"Failed to get angular velocity from GameObject '{target.name}'. No Rigidbody component found.");
            return new Vector3Payload();
        }
    }

    [JSExposedClass("physics.setUseGravity")]
    public class Event_PhysicsSetUseGravity : JSEventVoid<GameObject, bool>
    {
        protected override void Invoke(GameObject target, bool useGravity)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = useGravity;
            }
            else
            {
                JSInstance.LogError($"Failed to set useGravity on GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.setIsKinematic")]
    public class Event_PhysicsSetIsKinematic : JSEventVoid<GameObject, bool>
    {
        protected override void Invoke(GameObject target, bool isKinematic)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = isKinematic;
            }
            else
            {
                JSInstance.LogError($"Failed to set isKinematic on GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.setMass")]
    public class Event_PhysicsSetMass : JSEventVoid<GameObject, float>
    {
        protected override void Invoke(GameObject target, float mass)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.mass = mass;
            }
            else
            {
                JSInstance.LogError($"Failed to set mass on GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }

    [JSExposedClass("physics.setLinearDamping")]
    public class Event_PhysicsSetDrag : JSEventVoid<GameObject, float>
    {
        protected override void Invoke(GameObject target, float linearDamping)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearDamping = linearDamping;
            }
            else
            {
                JSInstance.LogError($"Failed to set linear damping (drag) on GameObject '{target.name}'. No Rigidbody component found.");
            }
        }
    }
}
