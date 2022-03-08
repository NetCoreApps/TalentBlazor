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
            }.WithAudit(userId);
        }

        public object Post(CreatePhoneScreen request)
        {
            var jobApp = Db.SingleById<JobApplication>(request.JobApplicationId);
            jobApp.ApplicationStatus = JobApplicationStatus.PhoneScreening;
            jobApp.PhoneScreen = request.ConvertTo<PhoneScreen>().WithAudit(Request);
            jobApp.Events ??= new();
            jobApp.Events.Add(CreateEvent(JobApplicationStatus.PhoneScreening));
            Db.Save(jobApp, references:true);
            return jobApp.PhoneScreen;
        }

        public object Patch(UpdatePhoneScreen request)
        {
            var jobApp = Db.LoadSingleById<JobApplication>(request.JobApplicationId);
            jobApp.ApplicationStatus = JobApplicationStatus.PhoneScreening;
            jobApp.PhoneScreen.PopulateWithNonDefaultValues(request).WithAudit(Request);
            jobApp.Events ??= new();
            jobApp.Events.Add(CreateEvent(JobApplicationStatus.PhoneScreeningCompleted));
            Db.Save(jobApp, references:true);
            return jobApp.PhoneScreen;
        }

        /*
        public object Post(CreatePhoneScreen request)
        {
            long phoneScreenId = 0;
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var phoneScreen = request.ConvertTo<PhoneScreen>();
                var userId = this.GetSession().UserAuthId;
                phoneScreen.CreatedBy = userId;
                phoneScreen.CreatedDate = now;
                phoneScreen.ModifiedBy = userId;
                phoneScreen.ModifiedDate = now;
                phoneScreenId = Db.Insert(phoneScreen, selectIdentity: true);
                var jobAppId = request.JobApplicationId;
                var jobAppEvent = new JobApplicationEvent
                {
                    AppUserId = userId.ToInt(),
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.PhoneScreening,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Description = "Advanced to phone screening",
                    EventDate = now,
                    CreatedDate = now,
                    ModifiedDate = now
                };
                Db.Insert(jobAppEvent, selectIdentity: true);
                var jobApp = Db.SingleById<JobApplication>(jobAppId);
                jobApp.ApplicationStatus = JobApplicationStatus.PhoneScreening;
                Db.Save(jobApp);
                transaction.Commit();
            }
            
            var phoneScreenResult = Db.SingleById<PhoneScreen>(phoneScreenId);
            return phoneScreenResult;
        }

        public object Patch(UpdatePhoneScreen request)
        {
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var phoneScreen = Db.SingleById<PhoneScreen>(request.Id);
                var userId = this.GetSession().UserAuthId;
                phoneScreen.ModifiedBy = userId;
                phoneScreen.ModifiedDate = now;
                phoneScreen.PopulateWithNonDefaultValues(request);
                Db.Save(phoneScreen);
                var jobAppId = request.JobApplicationId.Value;
                var jobAppEvent = new JobApplicationEvent
                {
                    AppUserId = userId.ToInt(),
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.PhoneScreeningCompleted,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Description = "Completed phone screening",
                    EventDate = now,
                    CreatedDate = now,
                    ModifiedDate = now
                };
                Db.Insert(jobAppEvent, selectIdentity: true);
                var jobApp = Db.SingleById<JobApplication>(jobAppId);
                jobApp.ApplicationStatus = JobApplicationStatus.PhoneScreeningCompleted;
                Db.Save(jobApp);
                transaction.Commit();
            }

            var phoneScreenResult = Db.SingleById<PhoneScreen>(request.Id);
            return phoneScreenResult;
        }
        */

        public object Post(CreateInterview request)
        {
            long interviewId = 0;
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var interview = request.ConvertTo<Interview>();
                var userId = this.GetSession().UserAuthId;
                interview.CreatedBy = userId;
                interview.CreatedDate = now;
                interview.ModifiedBy = userId;
                interview.ModifiedDate = now;
                interviewId = Db.Insert(interview, selectIdentity: true);
                var jobAppId = request.JobApplicationId;
                var jobAppEvent = new JobApplicationEvent
                {
                    AppUserId = userId.ToInt(),
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.Interview,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Description = "Advanced to interview",
                    EventDate = now,
                    CreatedDate = now,
                    ModifiedDate = now
                };
                Db.Insert(jobAppEvent, selectIdentity: true);
                var jobApp = Db.SingleById<JobApplication>(jobAppId);
                jobApp.ApplicationStatus = JobApplicationStatus.Interview;
                Db.Save(jobApp);
                transaction.Commit();
            }

            var interviewResult = Db.SingleById<Interview>(interviewId);
            return interviewResult;
        }

        public object Patch(UpdateInterview request)
        {
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var interview = Db.SingleById<Interview>(request.Id);
                var userId = this.GetSession().UserAuthId;
                interview.ModifiedBy = userId;
                interview.ModifiedDate = now;
                interview.PopulateWithNonDefaultValues(request);
                Db.Save(interview);
                var jobAppId = request.JobApplicationId.Value;
                var jobAppEvent = new JobApplicationEvent
                {
                    AppUserId = userId.ToInt(),
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.InterviewCompleted,
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Description = "Completed interview",
                    EventDate = now,
                    CreatedDate = now,
                    ModifiedDate = now
                };
                Db.Insert(jobAppEvent, selectIdentity: true);
                var jobApp = Db.SingleById<JobApplication>(jobAppId);
                jobApp.ApplicationStatus = JobApplicationStatus.InterviewCompleted;
                Db.Save(jobApp);
                transaction.Commit();
            }

            var interviewResult = Db.SingleById<Interview>(request.Id);
            return interviewResult;
        }
    }
}
