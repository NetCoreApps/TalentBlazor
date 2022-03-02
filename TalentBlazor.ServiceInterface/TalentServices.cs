using ServiceStack;
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

        public object Post(CreatePhoneScreen request)
        {
            long phoneScreenId = 0;
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var phoneScreen = request.ConvertTo<PhoneScreen>();
                phoneScreen.CreatedBy = this.GetSession().UserAuthId;
                phoneScreen.CreatedDate = now;
                phoneScreen.ModifiedBy = this.GetSession().UserAuthId;
                phoneScreen.ModifiedDate = now;
                phoneScreenId = Db.Insert(phoneScreen, selectIdentity: true);
                var jobAppId = request.JobApplicationId;
                var userId = request.ApiAppUserId;
                var jobAppEvent = new JobApplicationEvent
                {
                    ApiAppUserId = userId,
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.PhoneScreening,
                    CreatedBy = this.GetSession().UserAuthId,
                    ModifiedBy = this.GetSession().UserAuthId,
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

        public object Put(UpdatePhoneScreen request)
        {
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var phoneScreen = request.ConvertTo<PhoneScreen>();
                phoneScreen.ModifiedBy = this.GetSession().UserAuthId;
                phoneScreen.ModifiedDate = now;
                var jobAppId = request.JobApplicationId;
                var userId = request.ApiAppUserId;
                var jobAppEvent = new JobApplicationEvent
                {
                    ApiAppUserId = userId,
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.PhoneScreeningCompleted,
                    CreatedBy = this.GetSession().UserAuthId,
                    ModifiedBy = this.GetSession().UserAuthId,
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

        public object Post(CreateInterview request)
        {
            long interviewId = 0;
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var interview = request.ConvertTo<Interview>();
                interview.CreatedBy = this.GetSession().UserAuthId;
                interview.CreatedDate = now;
                interview.ModifiedBy = this.GetSession().UserAuthId;
                interview.ModifiedDate = now;
                interviewId = Db.Insert(interview, selectIdentity: true);
                var jobAppId = request.JobApplicationId;
                var userId = request.ApiAppUserId;
                var jobAppEvent = new JobApplicationEvent
                {
                    ApiAppUserId = userId,
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.Interview,
                    CreatedBy = this.GetSession().UserAuthId,
                    ModifiedBy = this.GetSession().UserAuthId,
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

        public object Put(UpdateInterview request)
        {
            using (var transaction = Db.OpenTransaction())
            {
                var now = DateTime.UtcNow;
                var interview = request.ConvertTo<Interview>();
                interview.ModifiedBy = this.GetSession().UserAuthId;
                interview.ModifiedDate = now;
                var jobAppId = request.JobApplicationId;
                var userId = request.ApiAppUserId;
                var jobAppEvent = new JobApplicationEvent
                {
                    ApiAppUserId = userId,
                    JobApplicationId = jobAppId,
                    Status = JobApplicationStatus.InterviewCompleted,
                    CreatedBy = this.GetSession().UserAuthId,
                    ModifiedBy = this.GetSession().UserAuthId,
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
