using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_TPMTrakDashboard
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            IJobDetail job = JobBuilder.Create<JobBackground>().Build();

            ITrigger trigger = TriggerBuilder.Create().StartNow()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(1)
                    .OnEveryDay()
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
        }
    }
}