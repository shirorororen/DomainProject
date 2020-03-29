using ServiceIntegration.Logic.Enum;

namespace ServiceIntegration.Logic.Contract
{
    public class NSLookup : MasterService
    {
        public override string CommandString => @"/C nslookup";

        public override ServiceType ServiceType => ServiceType.Domain;

        public override string FileName => "cmd.exe";

        protected override string ResolveCommand(string argument)
        {
            return $"{CommandString} {argument}";
        }
    }
}
