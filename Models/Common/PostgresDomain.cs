using System;
namespace DotnetApiPostgres.Api.Models.Common
{
    public class PostgresQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<PostgresCriteria> Criteria { get; set; } = new();
        public PostgresSort? Sort { get; set; }
        public string Operator { get; set; } = "And";
    }

    public class PostgresCriteria
    {
        public string Field { get; set; } = string.Empty;
        public object Value { get; set; } = new();
        public string Type { get; set; } = "equal"; // Supported: "equal", "like", "includes", "dateRange"
    }

    public class PostgresSort
    {
        public string Field { get; set; } = string.Empty;
        public string Order { get; set; } = "asc"; // asc or desc
    }

    public class PostgresDataSource<T>
    {
        public bool Success { get; set; }
        public int Total { get; set; }  // Total number of records in the database
        public int PageNumber { get; set; }  // Current page number
        public int PageSize { get; set; }  // Items per page
        public IEnumerable<T> Data { get; set; }  // List of data for the current page
        public string Message { get; set; }  // Optional message for the response
    }
}

