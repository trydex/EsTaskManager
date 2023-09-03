using EsTaskManager.Domain.Aggregates;
using EsTaskManager.Domain.Aggregates.User;
using EsTaskManager.Infrastructure;
using MediatR;

namespace EsTaskManager.Api.Queries.User;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserAggregate>
{
    private readonly EventStore _eventStore;

    public GetUserQueryHandler(EventStore eventStore)
    {
        _eventStore = eventStore;
    }
    public Task<UserAggregate> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userAggregate = _eventStore.Get<UserAggregate>(new AggregateId(request.UserId));

        return Task.FromResult(userAggregate);
    }
}