namespace GameTools.BlazorClient.Components.Pages.ComponentServices
{
    /// <summary>
    /// Simple implementation of a Mediator pattern.
    /// 
    /// Routes events that happen within a composed Blazor Page to its
    /// child components.
    /// 
    /// The Page "owns" the eventSink instance, and passes it as a parameter
    /// into child components that need to be notified when events happen outside
    /// of their own context.
    /// </summary>
    public class PageEventSink
    {
        private List<Func<PageStateChangedEvent, Task>> _handlers;

        public PageEventSink()
        {
            _handlers = new List<Func<PageStateChangedEvent, Task>>();
        }

        public void RegisterEventListener(Func<PageStateChangedEvent, Task> handler)
        {
            _handlers.Add(handler);
        }

        public async Task NotifyPropertyChangedAsync(PageStateChangedEvent notification)
        {
            var tasks = _handlers.Select(h => h.Invoke(notification)).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
