using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Utils;

namespace DotnetApiPostgres.Api.Mappings
{
    public static class OrderMappings
    {
        // Map Order entity to OrderDTO
        public static OrderDTO ToOrderDto(this Order entity)
        {
            return new OrderDTO
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                Email = entity.Email,
                TotalAmount = entity.TotalAmount,
                OrderDate = entity.OrderDate,
                //ShipMethod = entity.ShippingAddress != null ? entity.ShippingAddress.Country : string.Empty, // Example of how you can map shipMethod
                //Items = entity.Items?.Select(item => item.ToItemDto()).ToList(), // Map Items
                //Address = entity.ShippingAddress?.ToAddressDto() // Map Address
            };
        }

        // Map OrderDTO to Order entity
        public static Order ToOrder(this OrderDTO dto)
        {
            return new Order
            {
                Id = dto.Id ?? IdGenerator.GenerateId(),
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                TotalAmount = dto.TotalAmount,
                OrderDate = dto.OrderDate,
                //ShippingAddress = dto.Address?.ToAddress(), // Map Address
                //Items = dto.Items?.Select(itemDto => itemDto.ToOrderItem()).ToList() // Map Items
            };
        }

        // Map Item entity to ItemDTO
        public static ItemDTO ToItemDto(this OrderItem entity)
        {
            return new ItemDTO
            {
                ProductId = entity.OrderItemId,
                ProductName = entity.ProductName,
                Quantity = entity.Quantity,
                Price = entity.Price
            };
        }

        // Map ItemDTO to OrderItem entity
        public static OrderItem ToOrderItem(this ItemDTO dto)
        {
            return new OrderItem
            {
                OrderItemId = dto.ProductId,
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                Price = dto.Price
            };
        }

        // Map Address entity to AddressDTO
        public static AddressDTO ToAddressDto(this Address entity)
        {
            return new AddressDTO
            {
                Street = entity.Street,
                City = entity.City,
                State = entity.State,
                PostalCode = entity.PostalCode,
                Country = entity.Country
            };
        }

        // Map AddressDTO to Address entity
        public static Address ToAddress(this AddressDTO dto)
        {
            return new Address
            {
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            };
        }
    }
}
