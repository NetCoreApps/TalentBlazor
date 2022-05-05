using Funq;
using ServiceStack;
using NUnit.Framework;
using TalentBlazor.ServiceInterface;
using TalentBlazor.ServiceModel;
using System.Threading.Tasks;
using System;
using ServiceStack.Text;
using System.IO;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;

namespace TalentBlazor.Tests;

[Category("Integration")]
public class FileUploadTests
{
    static string AppData = "../../../../TalentBlazor/App_Data";
    static string ProfileImageUrl = "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=4&w=256&h=256&q=60";
    JsonApiClient client;

    public FileUploadTests()
    {
        client = new JsonApiClient("https://localhost:5001");
        var api = client.Api(new Authenticate {
            provider = "credentials",
            UserName = "admin@email.com",
            Password = "p@55wOrd",
        });

        if (!api.Succeeded)
            throw new Exception(api.ErrorMessage);
    }

    [Test]
    public async Task Can_CreateContact_with_Profile_Image_and_CreateJobApplication_with_attachments()
    {
        var profileImg = await ProfileImageUrl.GetStreamFromUrlAsync();
        var contact = await client.PostFileWithRequestAsync<Contact>(profileImg, "cody-fisher.png", new CreateContact
        {
            FirstName = "Cody",
            LastName = "Fisher",
            Email = "cody.fisher@gmail.com",
            JobType = "Security",
            PreferredLocation = "Remote",
            PreferredWorkType = EmploymentType.FullTime,
            AvailabilityWeeks = 1,
            SalaryExpectation = 100_000,
            About = "Lead Security Associate",
        }, fieldName:nameof(CreateContact.ProfileUrl));

        Assert.That(contact.FirstName, Is.EqualTo("Cody"));
        Assert.That(contact.ProfileUrl, Is.EqualTo("/profiles/cody-fisher.png"));

        var uploadedImage = await client.BaseUri.CombineWith(contact.ProfileUrl).GetStreamFromUrlAsync();
        Assert.That(uploadedImage.Length, Is.GreaterThan(0));

        var coverLetter = new FileInfo($"{AppData}/sample_coverletter.pdf");
        var resume = new FileInfo($"{AppData}/sample_resume.pdf");

        var uploadAttachments = new UploadFile[] {
            new(coverLetter.Name, coverLetter.OpenRead(), nameof(CreateJobApplication.Attachments)),
            new(resume.Name, coverLetter.OpenRead(), nameof(CreateJobApplication.Attachments)),
            new(contact.ProfileUrl.LastRightPart('/'), uploadedImage, nameof(CreateJobApplication.Attachments)),
        };

        var jobApp = await client.PostFilesWithRequestAsync<JobApplication>(new CreateJobApplication {
                JobId = 1,
                AppliedDate = DateTime.UtcNow,
                ContactId = contact.Id,
            }, uploadAttachments);

        uploadAttachments.Each(x => x.Stream.Dispose());
    }

    [Test]
    public async Task Can_CreateContact_with_Profile_Image_and_CreateJobApplication_with_attachments_MultipartFormDataContent()
    {
        var profileImg = await ProfileImageUrl.GetStreamFromUrlAsync();
        using var createContact = new MultipartFormDataContent()
            .AddParams(new CreateContact
            {
                FirstName = "Cody",
                LastName = "Fisher",
                Email = "cody.fisher@gmail.com",
                JobType = "Security",
                PreferredLocation = "Remote",
                PreferredWorkType = EmploymentType.FullTime,
                AvailabilityWeeks = 1,
                SalaryExpectation = 100_000,
                About = "Lead Security Associate",
            })
            .AddFile(nameof(CreateContact.ProfileUrl), "cody-fisher.png", profileImg);

        var contactApi = await client.ApiFormAsync<Contact>(typeof(CreateContact).ToApiUrl(), createContact);
        Assert.That(contactApi.Succeeded, Is.True);
        
        var contact = contactApi.Response!;
        Assert.That(contact.FirstName, Is.EqualTo("Cody"));
        Assert.That(contact.ProfileUrl, Is.EqualTo("/profiles/cody-fisher.png"));

        using var uploadedImage = await client.BaseUri.CombineWith(contact.ProfileUrl).GetStreamFromUrlAsync();
        Assert.That(uploadedImage.Length, Is.GreaterThan(0));

        var coverLetter = new FileInfo($"{AppData}/sample_coverletter.pdf");
        var resume = new FileInfo($"{AppData}/sample_resume.pdf");

        var createJobApp = new MultipartFormDataContent()
            .AddParams(new CreateJobApplication {
                JobId = 1,
                AppliedDate = DateTime.UtcNow,
                ContactId = contact.Id,
            })
            .AddFile(nameof(CreateJobApplication.Attachments), coverLetter)
            .AddFile(nameof(CreateJobApplication.Attachments), resume)
            .AddFile(nameof(CreateJobApplication.Attachments), contact.ProfileUrl.LastRightPart('/'), uploadedImage);

        var jobAppApi = await client.ApiFormAsync<JobApplication>(typeof(CreateJobApplication).ToApiUrl(), createJobApp);
        jobAppApi.Error.PrintDump();
        Assert.That(jobAppApi.Succeeded, Is.True);

        var jobApp = jobAppApi.Response!;
    }
}
