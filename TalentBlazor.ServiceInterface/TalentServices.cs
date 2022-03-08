﻿using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentBlazor.ServiceModel;

namespace TalentBlazor.ServiceInterface
{
    public class TalentServices : Service
    {
        public IAutoQueryDb AutoQuery { get; set; }

        JobApplicationEvent CreateEvent(JobApplicationStatus status)
        {
            var userId = this.GetSession().UserAuthId;
            return new JobApplicationEvent
            {
                AppUserId = userId.ToInt(),
                Status = status,
                Description = status.ToDescription(),
                EventDate = DateTime.UtcNow,
            }.WithAudit(userId);
        }

        public object Post(CreatePhoneScreen request)
        {
            var jobApp = Db.SingleById<JobApplication>(request.JobApplicationId);
            jobApp.PhoneScreen = request.ConvertTo<PhoneScreen>().WithAudit(Request);
            jobApp.Events ??= new();
            if(jobApp.ApplicationStatus != request.ApplicationStatus)
                jobApp.Events.Add(CreateEvent(request.ApplicationStatus));
            jobApp.ApplicationStatus = request.ApplicationStatus;
            Db.Save(jobApp, references:true);
            return jobApp.PhoneScreen;
        }

        public object Patch(UpdatePhoneScreen request)
        {
            var jobApp = Db.LoadSingleById<JobApplication>(request.JobApplicationId);
            var jobAppStatus = request.ApplicationStatus ?? JobApplicationStatus.PhoneScreeningCompleted;
            jobApp.PhoneScreen.PopulateWithNonDefaultValues(request).WithAudit(Request);
            if (jobApp.ApplicationStatus != request.ApplicationStatus)
                jobApp.Events.Add(CreateEvent(jobAppStatus));
            jobApp.ApplicationStatus = jobAppStatus;
            Db.Save(jobApp, references:true);
            return jobApp.PhoneScreen;
        }

        public object Post(CreateInterview request)
        {
            var jobApp = Db.SingleById<JobApplication>(request.JobApplicationId);
            jobApp.Interview = request.ConvertTo<Interview>().WithAudit(Request);
            jobApp.Events ??= new();
            if (jobApp.ApplicationStatus != request.ApplicationStatus)
                jobApp.Events.Add(CreateEvent(request.ApplicationStatus));
            jobApp.ApplicationStatus = request.ApplicationStatus;
            Db.Save(jobApp, references: true);
            return jobApp.Interview;
        }

        public object Patch(UpdateInterview request)
        {
            var jobApp = Db.LoadSingleById<JobApplication>(request.JobApplicationId);
            var jobAppStatus = request.ApplicationStatus ?? JobApplicationStatus.InterviewCompleted;
            jobApp.Interview.PopulateWithNonDefaultValues(request).WithAudit(Request);
            if (jobApp.ApplicationStatus != request.ApplicationStatus)
                jobApp.Events.Add(CreateEvent(jobAppStatus));
            jobApp.ApplicationStatus = jobAppStatus;
            Db.Save(jobApp, references: true);
            return jobApp.Interview;
        }
    }
}
