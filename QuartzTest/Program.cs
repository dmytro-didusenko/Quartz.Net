using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace QuartzTest;
public class Program
{
    public static async  Task Main(string[] args)
    {
        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();

        var job1 = JobBuilder.Create<TimeJob>().Build();
        var job2 = JobBuilder.Create<MessageJob>().Build();

        var trigger1 = TriggerBuilder.Create()
            .WithCronSchedule("0,10,20,30,40,50 * * * * ? * *")
            .Build();

        var trigger2 = TriggerBuilder.Create()
            .WithCronSchedule("0,15,30,45 */2 * * * ? * *")
            .Build();

        await scheduler.Start();

        await scheduler.ScheduleJob(job1, trigger1);
        await scheduler.ScheduleJob(job2, trigger2);

        

        Console.ReadKey();
    }

    private class ConsoleLogProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= LogLevel.Info && func != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                }
                return true;
            };
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }
    }
}

public class TimeJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync(typeof(TimeJob).Name + " - " + DateTime.Now.ToString());
    }
}

public class MessageJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await Console.Out.WriteLineAsync(typeof(MessageJob).Name + " - " + DateTime.Now.ToString());
    }
}