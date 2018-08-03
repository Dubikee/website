using Quartz;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Server.Jobs
{
    public class TimeJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await File.AppendAllTextAsync("D:/1.txt", DateTime.Now.ToLongDateString());
        }
    }
}
