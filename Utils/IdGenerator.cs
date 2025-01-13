namespace DotnetApiPostgres.Api.Utils
{
    public static class IdGenerator
    {
        public static string GenerateId()
        {
            // Generate a 24-character ID
            return Guid.NewGuid().ToString("N").Substring(0, 24);
        }
    }
}

