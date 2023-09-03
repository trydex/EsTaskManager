using System.Collections.Concurrent;
using System.Reflection;
using EsTaskManager.Domain.Events;

namespace EsTaskManager.Domain.Aggregates;

public record AggregateId
{
    public AggregateId(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public static AggregateId New() => new(Guid.NewGuid());
}

public abstract class Aggregate
{
    private readonly List<DomainEvent> _domainEvents = new();

    protected Aggregate(AggregateId id)
    {
        Id = id;
    }

    public AggregateId Id { get; protected set; }
    public long Version { get; protected set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

    protected void RaiseEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
        Dispatch(this, @event);
    }

    private void Dispatch(object aggregate, object eventMessage)
    {
        if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
        if (eventMessage == null) throw new ArgumentNullException(nameof(eventMessage));

        var handlerInfo = new ConventionEventHandlersFactory().Create(aggregate.GetType())
            .SingleOrDefault(a => a.ParameterType == eventMessage.GetType());

        if (handlerInfo != null)
        {
            handlerInfo.MethodInfo.Invoke(aggregate, new[] { eventMessage });
        }
        else
        {
            throw new Exception("Handler not found");
        }
    }

    public void Initialize(IEnumerable<object> events)
    {
        var enumerable = events as object[] ?? events.ToArray();
        foreach (var @event in enumerable)
        {
            Dispatch(this, @event);
        }

        Version = enumerable.Length;
    }

    public void SetCommited(long newVersion)
    {
        Version = newVersion;
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }
}

internal sealed class ConventionEventHandlersFactory
{
    private static readonly string ApplyMethodName = "Apply";

    private readonly ConcurrentDictionary<Type, ICollection<EventHandlerInfo>> _cache = new();

    public ICollection<EventHandlerInfo> Create(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return _cache.GetOrAdd(type, CreateHadlers(type));
    }

    private ICollection<EventHandlerInfo> CreateHadlers(Type type)
    {
        return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(
                m =>
                    m.ReturnParameter != null &&
                    (m.Name == ApplyMethodName && m.GetParameters().Length == 1 &&
                     m.ReturnParameter.ParameterType == typeof(void)))
            .Select(m => new EventHandlerInfo(m, m.GetParameters().Single().ParameterType)).ToList();
    }
}

internal sealed class EventHandlerInfo
{
    public EventHandlerInfo(MethodInfo methodInfo, Type parameterType)
    {
        ParameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
        MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
    }

    public MethodInfo MethodInfo { get; private set; }
    public Type ParameterType { get; private set; }
}