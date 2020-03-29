using Microsoft.Extensions.Options;
using ServiceIntegration.Logic.Contract;
using ServiceIntegration.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceIntegration.Logic.Service
{
    public class DomainService : IDomainService
    {
        private readonly Context<MasterService> context;
        private readonly PoolConfiguration config;

        public DomainService(IOptions<PoolConfiguration> config)
        {
            this.config = config.Value;

            // init service classes
            context = new Context<MasterService>();
            var assem = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var dataType in assem)
                if (dataType.BaseType == typeof(MasterService))
                    context.Entities.Add(Activator.CreateInstance(dataType) as MasterService);
        }

        public async Task<List<List<Result>>> ProcessServices(string[] domains)
        {
            var results = new List<List<Result>>();

            foreach (string domain in domains) {
                results.Add(await ProcessServices(domain));
            }

            return results;
        }

        public async Task<List<Result>> ProcessServices(string domain)
        {
            var listResult = new List<Result>();
            var listTask = new List<Task<Result>>();

            var throttler = new SemaphoreSlim(config.MaxPool);

            foreach (var entity in context.Entities) {

                await throttler.WaitAsync();
                listTask.Add(Task.Run(async () =>
                {
                    return await entity.Execute(domain);
                }));
            }

            await Task.WhenAll(listTask);

            listTask.ForEach(t =>
            {
                listResult.Add(t.Result);
            });

            return listResult;
        }

    }
}
