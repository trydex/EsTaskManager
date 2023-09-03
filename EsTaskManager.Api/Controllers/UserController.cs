using EsTaskManager.Api.Commands.User;
using EsTaskManager.Api.Queries.User;
using EsTaskManager.Domain.Aggregates;
using EsTaskManager.Domain.Aggregates.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EsTaskManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController  : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<UserAggregate> Get([FromQuery] GetUserQuery query)
    {
        return await _mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<AggregateId> Create(CreateUserCommand command)
    {
        return await _mediator.Send(command);
    }
    
    [HttpPut]
    public async Task<UserAggregate> Update(UpdateUserCommand command)
    {
        return await _mediator.Send(command);
    }
}