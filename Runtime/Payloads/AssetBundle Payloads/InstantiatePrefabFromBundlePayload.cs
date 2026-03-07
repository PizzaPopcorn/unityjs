namespace UniJS.Payloads
{
    [System.Serializable]
    public class InstantiatePrefabFromBundlePayload
    {
        public string eventId;
        public string bundleUrl;
        public string prefabName;
        public string parentKey;
    }
}