using Newtonsoft.Json;

namespace CyberStoreSVC.Utils
{
    public static class DataConvert
	{
        public static List<T> ReadFromFile<T>(string filePath)
        {
            // Read the file content as a string
            string jsonContent = File.ReadAllText(filePath);

            List<T> result = JsonConvert.DeserializeObject<List<T>>(jsonContent);

            if (result == null)
            {
                throw new InvalidOperationException("Deserialization failed.");
            }

            return result;
        }
    }
}

