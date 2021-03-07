using Hangfire.RecurringJobAdmin;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.TimeZoneInfo;

namespace Hangfire.Sample.Library
{
    public class AppointmentSmsNotificationJob
    {
        [RecurringJob("*/2 * * * *", "UTC", "default", RecurringJobId = "Run")]
        public void Run()
        {
            Console.WriteLine("Check File Exists");
        }

        [RecurringJob("*/1 * * * *", "default", TimeZone = "China Standard Time" /* (UTC+08:00) 北京，重庆，香港特别行政区，乌鲁木齐 */, RecurringJobId = "RunDelayJob")]
        [DisableConcurrentlyJobExecution(nameof(RunDelayJob), jobState: JobState.EnqueuedState)]
        public async Task RunDelayJob()
        {
            var id = Guid.NewGuid();
            Console.WriteLine(id);
            CancellationTokenSource source = new CancellationTokenSource();
            await Task.Delay(180000, source.Token);
        }

        [RecurringJob("*/1 * * * *", "default", RecurringJobId = "DoThis")]
        [DisableConcurrentlyJobExecution(nameof(DoThis))]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task DoThis()
        {
            await Task.Run(() => throw new Exception());
        }
    }
}
