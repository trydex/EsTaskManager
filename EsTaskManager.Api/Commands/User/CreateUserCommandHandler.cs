using EsTaskManager.Domain.Aggregates;
using EsTaskManager.Domain.Aggregates.User;
using EsTaskManager.Infrastructure;
using MediatR;

namespace EsTaskManager.Api.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AggregateId>
{
    private readonly EventStore _eventStore;

    public CreateUserCommandHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }
    public Task<AggregateId> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = AggregateId.New();
        var userAggregate = new UserAggregate(userId, request.Name, request.Email);
        _eventStore.Save(userAggregate);

        return Task.FromResult(userAggregate.Id);
    }
}