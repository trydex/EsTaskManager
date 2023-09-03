using EsTaskManager.Domain.Aggregates;
using EsTaskManager.Domain.Aggregates.User;
using EsTaskManager.Infrastructure;
using MediatR;

namespace EsTaskManager.Api.Commands.User;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserAggregate>
{
    private readonly EventStore _eventStore;

    public UpdateUserCommandHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }
    
    public Task<UserAggregate> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userAggregate = _eventStore.Get<UserAggregate>(new AggregateId(request.Id));
        userAggregate.UpdateUserData(request.Name, request.Email);
        _eventStore.Save(userAggregate);
        
        return Task.FromResult(userAggregate);
    }
}