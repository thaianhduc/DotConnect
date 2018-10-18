using System;
using System.Collections.Generic;

namespace DotConnect.Loggy
{
    public class PipelineResult
    {
        public string TransactionType { get; set; }
        public int TotalExecutionTime { get; set; }
        public int NumberOfExecution { get; set; }
        public IList<Guid> CorrelationIds { get; set; }
    }
}