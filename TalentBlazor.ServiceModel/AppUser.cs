using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TalentBlazor.ServiceModel;

[Icon(Svg = Icons.AppUser)]
public class AppUser : IUserAuth
{
    [AutoIncrement]
    public int Id { get; set; }
    public string DisplayName { get; set; }

    [Index]
    [Format(FormatMethods.LinkEmail)]
    public string Email { get; set; }

    // Custom Properties
    [Format(FormatMethods.IconRounded)]
    [Input(Type = "file"), UploadTo("users")]
    public string ProfileUrl { get; set; }

    public Department Department { get; set; }
    public string Title { get; set; }
    public string JobArea { get; set; }
    public string Location { get; set; }
    public int Salary { get; set; }
    public string About { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? ArchivedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string LastLoginIp { get; set; }

    // UserAuth Properties
    public string UserName { get; set; }
    public string PrimaryEmail { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public string Country { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string BirthDateRaw { get; set; }
    public string Address { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Culture { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string Language { get; set; }
    public string MailAddress { get; set; }
    public string Nickname { get; set; }
    public string PostalCode { get; set; }
    public string TimeZone { get; set; }
    [IgnoreDataMember]
    public string Salt { get; set; }
    [IgnoreDataMember]
    public string PasswordHash { get; set; }
    [IgnoreDataMember]
    public string DigestHa1Hash { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int InvalidLoginAttempts { get; set; }
    public DateTime? LastLoginAttempt { get; set; }
    public DateTime? LockedDate { get; set; }
    [IgnoreDataMember]
    public string RecoveryToken { get; set; }

    //Custom Reference Data
    public int? RefId { get; set; }
    public string RefIdStr { get; set; }
    public Dictionary<string, string> Meta { get; set; }
}

public enum Department
{
    None,
    Marketing,
    Accounts,
    Legal,
    HumanResources,
}
