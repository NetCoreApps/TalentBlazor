using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace TalentBlazor.ServiceModel;

[Icon(Svg = Icons.AppUser)]
[Alias("AspNetUsers")]
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    
    [Format(FormatMethods.IconRounded)]
    [Input(Type = "file"), UploadTo("users")]
    public string? ProfileUrl { get; set; }
}

