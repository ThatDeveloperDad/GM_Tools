using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
    public partial class NpcList
    {
        [Inject]
        protected NpcServiceProxy? NpcServices { get; set; }

        protected override void OnInitialized()
        {
            
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if(firstRender)
            {
                ExecuteFilter().Wait();
                StateHasChanged();
            }
        }

        public NpcList()
        {
            FilteredNpcs = Array.Empty<NpcFilterRowModel>();
        }

        public NpcFilterRowModel[] FilteredNpcs { get; set; }

        private async Task ExecuteFilter()
        {
            GuardNpcServicesExist();

            NpcClientFilter filter = new NpcClientFilter();
            var proxyResult = await NpcServices!.FilterTownsfolk(filter);
            if (proxyResult == null)
            {
                throw new Exception("Something awful happened.");
            }
            else
            {
                if (proxyResult.WasSuccessful)
                {
                    FilteredNpcs = proxyResult.Payload?.ToArray()
                        ?? Array.Empty<NpcFilterRowModel>();
                }
                else
                {
                    //Handle any errors that came back.
                    var sb = new StringBuilder();
                    foreach(var kvp in proxyResult.Errors)
                    {
                        sb.AppendLine(kvp.Value);
                    }

                    throw new Exception($"Fetching the NPC list failed.  {Environment.NewLine}{sb.ToString()}");
                }
            }
        }

        private void GuardNpcServicesExist()
        {
            if (NpcServices == null)
            {
                throw new InvalidOperationException("Cannot execute the requested operation because the ServiceProxy is null.");
            }
        }
    }
}
