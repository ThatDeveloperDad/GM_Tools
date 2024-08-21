using GameTools.BlazorClient.Components.Pages.PageModels;

namespace GameTools.BlazorClient.Components.Pages.ComponentServices
{
    public record PageStateChangedEvent
    {

        public PageStateChangedEvent(string name, CreateNpcPageStates value)
        {
            PropertyName = name;
            NewValue = value;
        }

        public string PropertyName { get; set; }

        public CreateNpcPageStates NewValue { get; set; }
    }
}
