namespace UniJS.Payloads
{
    [System.Serializable]
    public class InstantiatePayload
    {
        public string prefabPath;
        public Vector3Payload position;
        public QuaternionPayload rotation;
    }
}