using DotnetApiPostgres.Api.Mappings;
using DotnetApiPostgres.Api.Mediator.Common;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Repository;
using FluentValidation;
using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Commands.Admin
{
    public sealed class InitCateroryCommand : ICommand<JsonResponse<List<Category>>>
    {
        public List<CategoryDTO> Datas { get; set; } = new List<CategoryDTO>();
    }

    public sealed class InitCateroryCommandValidator : AbstractValidator<InitCateroryCommand>
    {
        public InitCateroryCommandValidator()
        {
            RuleFor(x => x.Datas)
                .NotNull()
                .NotEmpty()
                .WithMessage("Datas must not be null or empty.");
        }
    }

    public sealed class InitCateroryCommandHandler : ICommandHandler<InitCateroryCommand, JsonResponse<List<Category>>>
    {
        private readonly IPostgresRepository<Category, string> _repository;
        private readonly IPostgresRepository<Order, string> _orderRepository;
        private readonly IMediator _mediator;
        public InitCateroryCommandHandler(
            IPostgresRepository<Category, string> repository,
            IPostgresRepository<Order, string> orderRepository,
            IMediator mediator
        )
        {
            _repository = repository;
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<JsonResponse<List<Category>>> Handle(InitCateroryCommand request, CancellationToken cancellationToken)
        {
            var response = new JsonResponse<List<Category>>();
            await _repository.DeleteAsync(f => true);

            var initDataResult = new List<Category>();

            foreach (var data in request.Datas)
            {
                var category = await _repository.AddAsync(data.ToCategory());
                if (category != null)
                {
                    initDataResult.Add(category);
                }
            }

            //var createdOrder = await _orderRepository.AddAsync(new Order()
            //{
            //    Id = IdGenerator.GenerateId(),
            //    Email = "1",
            //    CustomerName = "1",
            //    OrderDate = DateTime.UtcNow,
            //    TotalAmount = 1
            //});

            response.Success = true;
            response.Data = initDataResult;
            return response;
        }
    }
}

