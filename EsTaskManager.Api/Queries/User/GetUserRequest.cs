using EsTaskManager.Domain.Aggregates.User;
using MediatR;

namespace EsTaskManager.Api.Queries.User;

public class GetUserQuery : IRequest<UserAggregate>
{
    public Guid UserId { get; set; }
}