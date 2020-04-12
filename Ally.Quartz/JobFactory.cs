using System;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace Ally.Quartz
{
    public class JobFactory: PropertySettingJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = _serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            var jobDataMap = new JobDataMap();
            jobDataMap.PutAll(scheduler.Context);
            jobDataMap.PutAll(bundle.JobDetail.JobDataMap);
            jobDataMap.PutAll(bundle.Trigger.JobDataMap);
            SetObjectProperties(job, jobDataMap);
            return job;
        }

        public override void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}