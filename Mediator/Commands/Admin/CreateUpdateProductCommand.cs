using CyberStoreSVC.Mappings;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using CyberStoreSVC.Utils;
using MediatR;

namespace CyberStoreSVC.Mediator.Commands.Admin;

public class CreateUpdateProductCommand : IRequest<JsonResponse<Product>>
{
    public required ProductDTO Data { get; set; }
}

public class CreateUpdateProductHandler : IRequestHandler<CreateUpdateProductCommand, JsonResponse<Product>>
{
    private readonly IPostgresRepository<Product, string> _productRepo;

    public CreateUpdateProductHandler(IPostgresRepository<Product, string> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<JsonResponse<Product>> Handle(CreateUpdateProductCommand request, CancellationToken cancellationToken)
    {
        var response = new JsonResponse<Product>();
        var dto = request.Data;

        if (string.IsNullOrEmpty(dto.Id))
        {
            var entity = dto.ToProduct();
            entity.Id = IdGenerator.GenerateId();

            var result = await _productRepo.AddAsync(entity);
            response.Success = true;
            response.Data = result;
            response.Message = "create.success";
            return response;
        }
        else
        {
            var existing = await _productRepo.FindByIdAsync(dto.Id);
            if (existing == null)
            {
                response.Message = "product.notFound";
                return response;
            }
            existing.UpdateWithDto(dto);
            var result = await _productRepo.UpdateAsync(existing);
            response.Success = true;
            response.Data = result;
            response.Message = "update.success";
            return response;
        }
    }
}