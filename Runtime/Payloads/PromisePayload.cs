using System;

namespace UniJS.Payloads
{
    [Serializable]
    public class PromisePayload
    {
        public string promiseId = Guid.NewGuid().ToString();
    }
}