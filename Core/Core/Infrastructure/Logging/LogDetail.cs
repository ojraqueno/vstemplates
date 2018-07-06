using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Logging
{
    public class LogDetail
    {

        public DateTime AddedOn { get; }
        public string Message { get; set; }

        public LogDetail()
        {
            AddedOn = DateTime.UtcNow;
        }

        //public string Product { get; set; }
        //public string Layer { get; set; }
        public string Location { get; set; }
        //public string Hostname { get; set; }

        public string LoggedInUserId { get; set; }

        public long? ElapsedMilliseconds { get; set; }
        public Exception Exception { get; set; }
        public string CorrelationId { get; set; }
        public Dictionary<string, object> AdditionalInfo { get; set; }
    }
}
