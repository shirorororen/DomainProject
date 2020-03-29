using ServiceIntegration.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Medallion;
using System.Threading.Tasks;
using Medallion.Shell;
using ServiceIntegration.Logic.Enum;

namespace ServiceIntegration.Logic.Contract
{
    public abstract class MasterService
        : IMasterService
    {
        public abstract string CommandString { get; }
        public abstract ServiceType ServiceType { get; }
        public abstract string FileName { get; }
        public MasterService()
        {
        }

        protected abstract string ResolveCommand(string argument);

        public virtual async Task<Result> Execute(string domain)
        {
            var resolvedCommand = ResolveCommand(domain);
            try
            {
       
                var command = Command.Run(FileName, resolvedCommand);

                var result = await command.Task;

                return new Result
                {
                    Message = result.StandardOutput,
                    ErrorMessage = $"{result.StandardError} - {result.ExitCode}",
                    Parameter = $"{domain} - Service : {this.GetType().Name}",
                    Success = result.Success
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    ErrorMessage = ex.Message,
                    Parameter = $"{domain} - Service : {this.GetType().Name}",
                };
            }

        }

        public virtual Result Execute(string domain, int test = 1)
        {
            var task = new TaskCompletionSource<Result>();

            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = ResolveCommand(domain);


            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.RedirectStandardOutput = false;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                var result = new Result
                {
                    ErrorMessage = $"Error while executing command for {domain} : {e.Message}",
                    Parameter = domain
                };
            }

            if (process.ExitCode == 0)
            {
                var result = new Result
                {
                    Message = stdOutput.ToString(),
                    Parameter = domain
                };
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }
                var result = new Result
                {
                    ErrorMessage = $"Error while executing command for {domain} : Finished with exit code = {process.ExitCode} : {message}",
                    Parameter = domain
                };

                return result;
            }

            return new Result();
        }
    }
}
