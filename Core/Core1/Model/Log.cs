using System;

namespace Core1.Model
{
    public class Log
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Exception { get; set; }
        public int Id { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string RequestBody { get; set; }
        public DateTime? Timestamp { get; set; }
        public string User { get; set; }
    }
}