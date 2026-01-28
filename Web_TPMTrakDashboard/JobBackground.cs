using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web_TPMTrakDashboard
{
    public class JobBackground : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            await Task.Factory.StartNew(() => CachedData.RefreshData());
        }
    }
}