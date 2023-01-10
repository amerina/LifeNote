using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Motto.API.Model;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Motto.API.Infrastructure
{
    public class MottoContextSeed
    {
        public async Task SeedAsync(MottoContext context, ILogger<MottoContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(MottoContextSeed));

            await policy.ExecuteAsync(async () =>
            {
               
                if (!context.MottoLanguages.Any())
                {
                    await context.MottoLanguages.AddRangeAsync(GetPreconfiguredMottoLanguages());

                    await context.SaveChangesAsync();
                }

                if (!context.MottoTypes.Any())
                {
                    await context.MottoTypes.AddRangeAsync(GetPreconfiguredMottoTypes());

                    await context.SaveChangesAsync();
                }

                if (!context.MottoItems.Any())
                {
                    await context.MottoItems.AddRangeAsync(GetPreconfiguredItems());

                    await context.SaveChangesAsync();
                }
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<MottoContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }

        static IEnumerable<MottoLanguage> GetPreconfiguredMottoLanguages()
        {
            return new List<MottoLanguage>()
            {
                new MottoLanguage("Chinese"),
                new MottoLanguage("English"),
            };
        }

        static IEnumerable<MottoType> GetPreconfiguredMottoTypes()
        {
            return new List<MottoType>()
            {
                new MottoType("Inspirational"),
                new MottoType("Dark Lessons"),
                new MottoType("Irony")
            };
        }

        static IEnumerable<MottoItem> GetPreconfiguredItems()
        {
            return new List<MottoItem>()
            {
                new MottoItem(1,1,"雪莱", "过去属于死神，未来属于你自己。"),
                new MottoItem(1,1,"无名氏", "永远都要记住你的人生掌握在你手中"),
                new MottoItem(1,1,"李苦禅", "鸟欲高飞先振翅，人求上进先读书"),
                new MottoItem(1,1,"罗曼·罗兰", "宿命论是那些缺乏意志力的弱者的借口。"),
                new MottoItem(1,1,"无名氏", "不努力，未来永远是个梦。"),
                new MottoItem(1,1,"屠格涅夫", "先相信你自己，然后别人才会相信你。"),
                new MottoItem(1,1,"颜真卿", "黑发不知勤学早，白首方悔读书迟"),
                new MottoItem(1,1,"柯罗连科", "生活就是战斗"),
            };
        }
    }
}
