using Newtonsoft.Json;

namespace CyberStoreSVC.Utils
{
    public static class ObjectUtils
	{
        public static T CloneJson<T>(this T source)
        {
            if (ReferenceEquals(source, null)) return default;
            var deserializeSettings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }
}

