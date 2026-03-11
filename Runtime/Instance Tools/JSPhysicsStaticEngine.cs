using UnityEngine;
using UniJS.Payloads;
using System.Linq;

namespace UniJS.InstanceTools
{
    internal static class JSPhysicsStaticEngine
    {
        public static RaycastHitPayload Raycast(RaycastPayload payload)
        {
            var origin = new Vector3(payload.origin.x, payload.origin.y, payload.origin.z);
            var direction = new Vector3(payload.direction.x, payload.direction.y, payload.direction.z);

            if (Physics.Raycast(origin, direction, out var hit, payload.distance))
            {
                return new RaycastHitPayload
                {
                    hit = true,
                    point = new Vector3Payload { x = hit.point.x, y = hit.point.y, z = hit.point.z },
                    normal = new Vector3Payload { x = hit.normal.x, y = hit.normal.y, z = hit.normal.z },
                    distance = hit.distance,
                    gameObject = new JSGameObjectData(hit.collider.gameObject)
                };
            }

            return new RaycastHitPayload { hit = false };
        }

        public static OverlapResultsPayload OverlapSphere(OverlapSpherePayload payload)
        {
            var position = new Vector3(payload.position.x, payload.position.y, payload.position.z);
            var colliders = Physics.OverlapSphere(position, payload.radius, payload.layerMask);
            
            return new OverlapResultsPayload
            {
                results = colliders.Select(c => new JSGameObjectData(c.gameObject)).ToArray()
            };
        }

        public static OverlapResultsPayload OverlapBox(OverlapBoxPayload payload)
        {
            var center = new Vector3(payload.center.x, payload.center.y, payload.center.z);
            var halfExtents = new Vector3(payload.halfExtents.x, payload.halfExtents.y, payload.halfExtents.z);
            var orientation = new Quaternion(payload.orientation.x, payload.orientation.y, payload.orientation.z, payload.orientation.w);
            
            var colliders = Physics.OverlapBox(center, halfExtents, orientation, payload.layerMask);
            
            return new OverlapResultsPayload
            {
                results = colliders.Select(c => new JSGameObjectData(c.gameObject)).ToArray()
            };
        }
    }
}