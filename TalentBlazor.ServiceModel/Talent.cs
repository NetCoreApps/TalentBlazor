using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TalentBlazor.ServiceModel;

public class Contact : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string ProfileUrl { get; set; }

    public int? SalaryExpectation { get; set; }

    public string JobType { get; set; }
    public int AvailabilityWeeks { get; set; }
    public EmployementType PreferredWorkType { get; set; }
    public string PreferredLocation { get; set; }


    public string Email { get; set; }
    public string Phone { get; set; }

    public string About { get; set; }

    [Reference]
    public List<JobApplication> Applications { get; set; }
}

public class Job : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [Reference]
    public List<JobApplication> Applications { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public int SalaryRangeLower { get; set; }
    public int SalaryRangeUpper { get; set; }

    public EmployementType EmployementType { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }

    public DateTime Closing { get; set; }

}

public enum EmployementType
{
    FullTime,
    PartTime,
    Casual,
    Contract
}

public class JobApplication : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [References(typeof(Job))]
    public int JobId { get; set; }
    [References(typeof(Contact))]
    public int ContactId { get; set; }

    [Reference]
    public Job Position { get; set; }

    [Reference]
    public Contact Applicant { get; set; }

    public DateTime AppliedDate { get; set; }

    public JobApplicationStatus ApplicationStatus { get; set; }

    [Reference]
    public List<JobApplicationAttachment> Attachments { get; set; }

    [Reference]
    public List<JobApplicationEvent> Events { get; set; }

    [Reference]
    public PhoneScreen PhoneScreen { get; set; }

    [Reference]
    public Interview Interview { get; set; }
}

public class JobApplicationEvent : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [References(typeof(JobApplication))]
    public int JobApplicationId { get; set; }

    [References(typeof(ApiAppUser))]
    public int ApiAppUserId { get; set; }

    [Reference]
    public ApiAppUser AppUser { get; set; }

    public JobApplicationStatus? Status { get; set; }

    public string Description { get; set; }

    public DateTime EventDate { get; set; }

}

public class PhoneScreen : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [References(typeof(JobApplication))]
    public int JobApplicationId { get; set; }

    [References(typeof(ApiAppUser))]
    public int ApiAppUserId { get; set; }

    [Reference]
    public ApiAppUser AppUser { get; set; }

    public string Notes { get; set; }
}

public class Interview : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    public DateTime BookingTime { get;set; }

    [References(typeof(JobApplication))]
    public int JobApplicationId { get; set; }

    [References(typeof(ApiAppUser))]
    public int ApiAppUserId { get; set; }

    [Reference]
    public ApiAppUser AppUser { get; set; }

    public string Notes { get; set; }
}

public enum JobApplicationStatus
{
    Applied,
    PhoneScreening,
    PhoneScreeningCompleted,
    Interview,
    InterviewCompleted,
    Offer,
    Disqualified
}

[Alias("AppUser")]
public class ApiAppUser
{
    
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PrimaryEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string Company { get; set; }
    public string Title { get; set; }
    public string JobArea { get; set; }
    public string Location { get; set; }
    public int Salary { get; set; }
    public string About { get; set; }

    public string Department { get; set; }
    public string? ProfileUrl { get; set; }
}

public enum Department
{
    None,
    Marketing,
    Accounts,
    Legal,
    HumanResources,
}

public class JobApplicationAttachment : AuditBase
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [References(typeof(JobApplication))]
    public int JobApplicationId { get; set; }

    public string FileName { get; set; }
    public string FileLocation { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditQuery)]
public class QueryContacts : QueryDb<Contact>
{
    public int? Id { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditCreate)]
public class CreateContact : ICreateDb<Contact>, IReturn<Contact>
{
    [ValidateNotEmpty]
    public string Name { get; set; }
    public string ProfileUrl { get; set; }
    public int? SalaryExpectation { get; set; }
    [ValidateNotEmpty]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string About { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditModify)]
public class UpdateContact : IUpdateDb<Contact>, IReturn<Contact>
{
    public int Id { get; set; }
    [ValidateNotEmpty]
    public string Name { get; set; }
    public string ProfileUrl { get; set; }
    public int? SalaryExpectation { get; set; }
    [ValidateNotEmpty]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string About { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditSoftDelete)]
public class DeleteContact : IDeleteDb<Contact>, IReturnVoid
{
    public int Id { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditQuery)]
public class QueryJob : QueryDb<Job>
{
    public int? Id { get; set; }
    public List<int>? Ids { get; set; }
}


[Tag("Talent")]
[AutoApply(Behavior.AuditCreate)]
public class CreateJob : ICreateDb<Job>, IReturn<Job>
{
    public string Title { get; set; }
    public string Description { get; set; }

    public int SalaryRangeLower { get; set; }
    public int SalaryRangeUpper { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditModify)]
public class UpdateJob : IUpdateDb<Job>, IReturn<Job>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public int SalaryRangeLower { get; set; }
    public int SalaryRangeUpper { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditSoftDelete)]
public class DeleteJob : IDeleteDb<Job>, IReturn<Job>
{
    public int Id { get; set; }
}


[Tag("Talent")]
[AutoApply(Behavior.AuditQuery)]
public class QueryJobApplication : QueryDb<JobApplication>
{
    public int? Id { get; set; }
    public List<int>? Ids { get; set; }

    public int? JobId { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditCreate)]
public class CreateJobApplication : ICreateDb<JobApplication>, IReturn<JobApplication>
{
    public int JobId { get; set; }
    public int ContactId { get; set; }
    public DateTime AppliedDate { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditModify)]
public class UpdateJobApplication : IUpdateDb<JobApplication>, IReturn<JobApplication>
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int ContactId { get; set; }
    public DateTime AppliedDate { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditSoftDelete)]
public class DeleteJobApplication : IDeleteDb<JobApplication>, IReturnVoid
{
    public int Id { get; set; }
}


[Tag("Talent")]
[AutoApply(Behavior.AuditQuery)]
public class QueryPhoneScreen : QueryDb<PhoneScreen>
{

}

[Tag("Talent")]
[AutoApply(Behavior.AuditCreate)]
public class CreatePhoneScreen : ICreateDb<PhoneScreen>, IReturn<PhoneScreen>
{
    [ValidateNotEmpty]
    public int JobApplicationId { get; set; }
    public int ApiAppUserId { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditModify)]
public class UpdatePhoneScreen : IUpdateDb<PhoneScreen>, IReturn<PhoneScreen>
{
    [ValidateNotEmpty]
    public int JobApplicationId { get; set; }
    public int ApiAppUserId { get; set; }

    public string Notes { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditQuery)]
public class QueryInterview : QueryDb<Interview>
{
    public int? Id { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditCreate)]
public class CreateInterview : ICreateDb<Interview>, IReturn<Interview>
{
    [ValidateNotNull]
    public DateTime? BookingTime { get; set; }
    [ValidateNotEmpty]
    public int JobApplicationId { get; set; }
    public int ApiAppUserId { get; set; }

    public string Notes { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditModify)]
public class UpdateInterview : IUpdateDb<Interview>, IReturn<Interview>
{
    [ValidateNotNull]
    public DateTime? BookingTime { get; set; }
    [ValidateNotEmpty]
    public int JobApplicationId { get; set; }
    public int ApiAppUserId { get; set; }

    public string Notes { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditQuery)]
public class QueryJobAppEvents : QueryDb<JobApplicationEvent>
{
    public int? JobApplicationId { get; set; }
}

[Tag("Talent")]
[AutoApply(Behavior.AuditCreate)]
public class CreateJobApplicationEvent : ICreateDb<JobApplicationEvent>, 
    IReturn<JobApplicationEvent>
{

}

[Tag("Talent")]
[AutoApply(Behavior.AuditModify)]
public class UpdateJobApplicationEvent : IUpdateDb<JobApplicationEvent>,
    IReturn<JobApplicationEvent>
{

}

[Tag("Talent")]
[AutoApply(Behavior.AuditSoftDelete)]
public class DeleteJobApplicationEvent : IDeleteDb<JobApplicationEvent>,
    IReturn<JobApplicationEvent>, IReturnVoid
{

}

[Tag("Talent")]
public class QueryAppUser : QueryDb<ApiAppUser>
{
    public string? EmailContains { get; set; }
    public string? FirstNameContains { get; set; }
    public string? LastNameContains { get; set; }
}
