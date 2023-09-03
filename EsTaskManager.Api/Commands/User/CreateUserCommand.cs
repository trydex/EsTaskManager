using EsTaskManager.Domain.Aggregates;
using MediatR;

namespace EsTaskManager.Api.Commands.User;

public class CreateUserCommand : IRequest<AggregateId>
{
    public string Name { get; set; }
    public string Email { get; set; }
}
