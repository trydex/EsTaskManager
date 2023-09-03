using EsTaskManager.Domain.Aggregates;
using EsTaskManager.Domain.Aggregates.User;
using MediatR;

namespace EsTaskManager.Api.Commands.User;

public class UpdateUserCommand : IRequest<UserAggregate>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}