using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceIntegration.Logic.Contract
{
    public class Result
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string Parameter { get; set; }
        public bool Success { get; set; }
    }
}
