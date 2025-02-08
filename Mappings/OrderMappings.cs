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
                Phone = entity.Phone,
                Email = entity.Email,
                OrderDate = entity.OrderDate,
                OrderAddress = entity.OrderAddress,
                TotalAmount = entity.TotalAmount,
                OrderItems = entity.OrderItems?.Select(oi => oi.ToOrderItemDto()).ToList()
            };
        }

        // Map OrderDTO to Order entity
        public static Order ToOrder(this OrderDTO dto)
        {
            return new Order
            {
                Id = dto.Id ?? IdGenerator.GenerateId(),
                CustomerName = dto.CustomerName,
                Phone = dto.Phone,
                Email = dto.Email,
                OrderDate = dto.OrderDate,
                OrderAddress = dto.OrderAddress,
                TotalAmount = dto.TotalAmount,
                OrderItems = dto.OrderItems?.Select(oi => oi.ToOrderItem()).ToList()
            };
        }

        // Map OrderItem entity to OrderItemDTO
        public static OrderItemDTO ToOrderItemDto(this OrderItem entity)
        {
            return new OrderItemDTO
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                Price = entity.Price,
                Qty = entity.Qty,
                OrderId = entity.OrderId
            };
        }

        // Map OrderItemDTO to OrderItem entity
        public static OrderItem ToOrderItem(this OrderItemDTO dto)
        {
            return new OrderItem
            {
                Id = dto.Id ?? IdGenerator.GenerateId(),
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                Price = dto.Price,
                Qty = dto.Qty,
                OrderId = dto.OrderId
            };
        }
    }
}