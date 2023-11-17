namespace TalentBlazor
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Microsoft.AspNetCore.Components;

    public class LayoutService : ILayoutService, INotifyPropertyChanged
    {

        public RenderFragment Header => HeaderSetter?.ChildContent;

        public SetHeader HeaderSetter
        {
            get => headerSetter;
            set
            {
                if (headerSetter == value) return;
                headerSetter = value;
                UpdateHeader();
            }
        }

        public void UpdateHeader() => NotifyPropertyChanged(nameof(Header));

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private SetHeader headerSetter;

    }

    public interface ILayoutService
    {
        RenderFragment Header { get; }
        SetHeader HeaderSetter { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        void UpdateHeader();
    }
    
    public class SetHeader : ComponentBase, IDisposable
    {
        [Inject]
        private ILayoutService Layout { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (Layout != null)
            {
                Layout.HeaderSetter = this;
            }
            base.OnInitialized();
        }

        protected override bool ShouldRender()
        {
            var shouldRender = base.ShouldRender();
            if (shouldRender)
            {
                Layout.UpdateHeader();
            }
            return shouldRender;
        }

        public void Dispose()
        {
            if (Layout != null)
            {
                Layout.HeaderSetter = null;
            }
        }

    }

}
