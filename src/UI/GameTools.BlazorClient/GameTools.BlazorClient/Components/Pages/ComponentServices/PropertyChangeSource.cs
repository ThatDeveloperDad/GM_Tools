namespace GameTools.BlazorClient.Components.Pages.ComponentServices
{
    internal class PropertyChangeSource
    {
        private List<Func<PropertyChangedEvent, Task>> _handlers;

        public PropertyChangeSource()
        {
            _handlers = new List<Func<PropertyChangedEvent, Task>>();
        }

        public void RegisterPropertyListener(Func<PropertyChangedEvent, Task> handler)
        {
            _handlers.Add(handler);
        }

        public async Task NotifyPropertyChangedAsync(PropertyChangedEvent notification)
        {
            var tasks = _handlers.Select(h => h.Invoke(notification)).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
