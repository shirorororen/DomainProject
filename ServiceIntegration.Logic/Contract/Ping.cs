using ServiceIntegration.Logic.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceIntegration.Logic.Contract
{
    public class Ping : MasterService
    {
        public override string CommandString => @"/C ping";

        public override ServiceType ServiceType => ServiceType.Domain;

        public override string FileName => "cmd.exe";

        protected override string ResolveCommand(string argument)
        {
            return $"{CommandString} {argument}";
        }
    }
}
