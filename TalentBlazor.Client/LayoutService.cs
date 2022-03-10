namespace TalentBlazor.Client
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

}
