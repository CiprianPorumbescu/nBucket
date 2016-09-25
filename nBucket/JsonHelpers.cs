using Newtonsoft.Json.Linq;

namespace nBucket
{
    public static class JsonHelpers
    {
        public static string TryGetValue(this JObject obj, string propertyName)
        {
            JToken value;
            return obj.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }
    }
}
