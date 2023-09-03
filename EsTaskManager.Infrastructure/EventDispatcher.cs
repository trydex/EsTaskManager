namespace EsTaskManager.Infrastructure;

public interface IEventDispatcher
{
    void Dispatch(object @event);
}

public class EventDispatcher : IEventDispatcher
{
    public void Dispatch(object @event)
    {
        
    }
}