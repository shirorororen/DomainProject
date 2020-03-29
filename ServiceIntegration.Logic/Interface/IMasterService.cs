using ServiceIntegration.Logic.Contract;
using ServiceIntegration.Logic.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceIntegration.Logic.Interface
{
    public interface IMasterService
    {
        public Task<Result> Execute(string argument);
        public string CommandString { get; }
        public ServiceType ServiceType { get; }
        public string FileName { get; }
    }
}
