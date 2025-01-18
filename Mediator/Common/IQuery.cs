﻿using MediatR;

namespace DotnetApiPostgres.Api.Mediator.Common
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }

    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
       where TQuery : IQuery<TResponse>
    {
    }
}

