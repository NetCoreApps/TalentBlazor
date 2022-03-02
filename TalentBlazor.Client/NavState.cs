using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentBlazor.Client
{
    public class BreadCrumbStateContainer
    {
        public NavigationManager NavigationManager { get; set; }

        public BreadCrumbStateContainer(NavigationManager navigationManager)
        {
            NavigationManager = navigationManager;
            navigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            //BreadCrumbs = new List<BreadCrumb>();
        }

        private List<BreadCrumb> breadCrumbs;
        public List<BreadCrumb> BreadCrumbs
        {
            get => breadCrumbs ?? new List<BreadCrumb>();
            set
            {
                breadCrumbs = value;
                NotifyStateChanged();
            }
        }

        public List<BreadCrumb> GetBreadCrumbs()
        {
            return BreadCrumbs;
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class BreadCrumb
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class StateContainer
    {
        private string? savedString;

        public string Property
        {
            get => savedString ?? string.Empty;
            set
            {
                savedString = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
