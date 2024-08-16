namespace GameTools.BlazorClient.Components.Pages.ComponentServices
{
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
