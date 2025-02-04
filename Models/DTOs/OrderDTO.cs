using DotnetApiPostgres.Api.Mediator.Commands.Web;

namespace DotnetApiPostgres.Api.Models.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
        public long OrderDate { get; set; }
        //public string ShipMethod { get; set; }
        //public List<ItemDTO> Items { get; set; }
        //public AddressDTO Address { get; set; }
    }

    public class ItemDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class AddressDTO
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

}

