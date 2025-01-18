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
        public List<CategoryDto> Datas { get; set; } = new List<CategoryDto>();
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
        private readonly IMediator _mediator;
        public InitCateroryCommandHandler(
            IPostgresRepository<Category, string> repository,
            IMediator mediator
        )
        {
            _repository = repository;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            response.Success = true;
            response.Data = initDataResult;
            return response;
        }
    }
}

