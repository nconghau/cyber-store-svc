using CyberStoreSVC.Mappings;
using CyberStoreSVC.Mediator.Common;
using CyberStoreSVC.Models.Common;
using CyberStoreSVC.Models.DTOs;
using CyberStoreSVC.Models.Entities;
using CyberStoreSVC.Repository;
using FluentValidation;
using MediatR;

namespace CyberStoreSVC.Mediator.Commands.Admin
{
    public sealed class InitProductCommand : ICommand<JsonResponse<List<Product>>>
    {
        public List<ProductDTO> Datas { get; set; } = new List<ProductDTO>();
    }

    public sealed class InitProductCommandValidator : AbstractValidator<InitProductCommand>
    {
        public InitProductCommandValidator()
        {
            RuleFor(x => x.Datas)
                .NotNull()
                .NotEmpty()
                .WithMessage("Datas must not be null or empty.");
        }
    }

    public sealed class InitProductCommandHandler : ICommandHandler<InitProductCommand, JsonResponse<List<Product>>>
    {
        private readonly IPostgresRepository<Product, string> _repository;
        private readonly IMediator _mediator;
        public InitProductCommandHandler(
            IPostgresRepository<Product, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<List<Product>>> Handle(InitProductCommand request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<List<Product>>();
            await _repository.DeleteAsync(f => true);

            var initDataResult = new List<Product>();

            foreach (var data in request.Datas)
            {
                var product = await _repository.AddAsync(data.ToProduct());
                if (product != null)
                {
                    initDataResult.Add(product);
                }
            }
            response.Success = true;
            response.Data = initDataResult;
            return response;
        }
    }
}

