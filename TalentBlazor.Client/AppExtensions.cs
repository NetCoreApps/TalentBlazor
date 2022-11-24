using System;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using ServiceStack;

namespace TalentBlazor.Client;

public static class AppExtensions
{
    //https://jasonwatmore.com/post/2020/08/09/blazor-webassembly-get-query-string-parameters-with-navigation-manager
    public static NameValueCollection QueryString(this NavigationManager nav) =>
        System.Web.HttpUtility.ParseQueryString(new Uri(nav.Uri).Query);

    public static string? QueryString(this NavigationManager nav, string key) => nav.QueryString()[key];
    
}
