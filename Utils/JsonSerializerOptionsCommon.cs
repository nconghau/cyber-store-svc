using System.Text.Json;
using System.Text.Json.Serialization;

namespace CyberStoreSVC.Utils
{
    public class JsonSerializerOptionCommon
	{
        public static JsonSerializerOptions Create()
        {
            return new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
    }
}

