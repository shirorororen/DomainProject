using ServiceIntegration.Logic.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceIntegration.Logic.Interface
{
    public interface IDomainService
    {
        public Task<List<List<Result>>> ProcessServices(string[] domains);
        public Task<List<Result>> ProcessServices(string domain);
    }
}
