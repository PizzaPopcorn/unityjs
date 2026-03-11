using UniJS;

namespace UniJS.Payloads
{
    [System.Serializable]
    public class RaycastPayload
    {
        public Vector3Payload origin;
        public Vector3Payload direction;
        public float distance = float.MaxValue;
    }

    [System.Serializable]
    public class RaycastHitPayload
    {
        public bool hit;
        public Vector3Payload point;
        public Vector3Payload normal;
        public float distance;
        public JSGameObjectData gameObject;
    }

    [System.Serializable]
    public class OverlapSpherePayload
    {
        public Vector3Payload position;
        public float radius;
        public int layerMask = ~0; // All layers by default
    }

    [System.Serializable]
    public class OverlapBoxPayload
    {
        public Vector3Payload center;
        public Vector3Payload halfExtents;
        public QuaternionPayload orientation;
        public int layerMask = ~0;
    }

    [System.Serializable]
    public class OverlapResultsPayload
    {
        public JSGameObjectData[] results;
    }
}
