namespace GameTools.BlazorClient.Components.Pages.ComponentServices
{
    public record PropertyChangedEvent
    {

        public PropertyChangedEvent(string name, object value)
        {
            PropertyName = name;
            NewValue = value;
        }

        public string PropertyName { get; set; }

        public object NewValue { get; set; }
    }
}
