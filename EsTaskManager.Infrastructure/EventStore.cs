using System.Reflection;
using System.Runtime.CompilerServices;
using EsTaskManager.Domain.Aggregates;

namespace EsTaskManager.Infrastructure;

public class EventStore
{
    private object _locker = new();
    private readonly IEventDispatcher _eventDispatcher;

    private Dictionary<AggregateId, List<object>> _id2events = new();

    public EventStore(IEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher;
    }

    public void Save(Aggregate aggregate)
    {
        lock (_locker)
        {
            if (!_id2events.TryAdd(aggregate.Id, aggregate.DomainEvents.Cast<object>().ToList()))
            {
                _id2events[aggregate.Id].AddRange(aggregate.DomainEvents.Cast<object>().ToList());
            }

            aggregate.SetCommited(aggregate.Version + aggregate.DomainEvents.Count);
        }

        foreach (var aggregateDomainEvent in aggregate.DomainEvents)
        {
            _eventDispatcher.Dispatch(aggregateDomainEvent);
        }

        aggregate.ClearEvents();
    }

    public TAggregate Get<TAggregate>(AggregateId id) where TAggregate : Aggregate
    {
        var events = _id2events[id].ToList();
        var ctor = GetAggregateConstructor(typeof(TAggregate));
        var aggregate = (TAggregate)ctor.Invoke(new object[] { id });
        aggregate.Initialize(events);

        return aggregate;
    }
    
    private ConstructorInfo GetAggregateConstructor(Type type)
    {
        foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.Length == 1 && typeof (AggregateId).IsAssignableFrom(parameters[0].ParameterType))
                return constructor;
        }
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 2);
        interpolatedStringHandler.AppendLiteral("Constructor for aggregate with ");
        interpolatedStringHandler.AppendFormatted(type.FullName);
        interpolatedStringHandler.AppendLiteral(" not found. It must be public with parameter '");
        interpolatedStringHandler.AppendFormatted(typeof (AggregateId).FullName);
        interpolatedStringHandler.AppendLiteral("'");
        throw new InvalidOperationException(interpolatedStringHandler.ToStringAndClear());
    }
}