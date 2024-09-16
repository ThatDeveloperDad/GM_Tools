using Microsoft.AspNetCore.Components;

namespace GameTools.UI.Components.Wrapper
{
    public class WaitWrapperComponent:ComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        public bool Loading { get; private set; }

        private CancellationTokenSource cancellationTokenSource;
        public async Task<T> AwaitTask<T>(Task<T> task)
        {
            SetLoading(true);

            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                return await task.WaitAsync(cancellationTokenSource.Token);
            }
            catch (TaskCanceledException e)
            {
                e.Data.Add("CanceledTask", "true");
                throw;
            }
            finally
            {
                SetLoading(false);
                cancellationTokenSource = null;
            }
        }
        public void SetLoading(bool loadoing)
        {
            Loading = loadoing;
            StateHasChanged();
        }
    }
}
