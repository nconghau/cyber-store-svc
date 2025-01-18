namespace DotnetApiPostgres.Api.Models.DTOs
{
    public class ProductColor
    {
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;
    }

    public class ProductStorageOption
    {
        public string Capacity { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }

    public class ProductSpecification
    {
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class ProductDeliveryInfo
    {
        public string FreeDelivery { get; set; } = string.Empty;
        public string InStock { get; set; } = string.Empty;
        public string Guaranteed { get; set; } = string.Empty;
    }

    public class ProductAvailability
    {
        public bool AddToWishlist { get; set; }
        public bool AddToCart { get; set; }
    }
}

