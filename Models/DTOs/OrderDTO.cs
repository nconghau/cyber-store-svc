namespace CyberStoreSVC.Models.DTOs
{
    public class TryKafkaCreateOrderDTO
    {
        public bool PushNotify { get; set; } = false;
    }

    public class OrderDTO
    {
        public string? Id { get; set; }
        public required string CustomerName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public long OrderDate { get; set; }
        public required string OrderAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public required List<OrderItemDTO> OrderItems { get; set; }
        public bool PushNotify { get; set; } = false;
    }

    public class OrderItemDTO
    {
        public string? Id { get; set; }
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public required decimal Price { get; set; }
        public required int Qty { get; set; }
        public string? OrderId { get; set; }
    }
}

