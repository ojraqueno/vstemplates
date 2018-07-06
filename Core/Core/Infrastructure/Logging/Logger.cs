using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Logging
{
    public static class Logger
    {
        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        static Logger()
        {
            _perfLogger = new LoggerConfiguration()
                .WriteTo
                .File(path: "C:\\temp\\logs\\perf.txt")
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                .WriteTo
                .File(path: "C:\\temp\\logs\\usage.txt")
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                .WriteTo
                .File(path: "C:\\temp\\logs\\errors.txt")
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                .WriteTo
                .File(path: "C:\\temp\\logs\\diagnostic.txt")
                .CreateLogger();
        }

        public static void WritePerf(LogDetail logDetail)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@LogDetail}", logDetail);
        }

        public static void WriteUsage(LogDetail logDetail)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@LogDetail}", logDetail);
        }

        public static void WriteError(LogDetail logDetail)
        {
            _errorLogger.Write(LogEventLevel.Information, "{@LogDetail}", logDetail);
        }

        public static void WriteDiagnostic(LogDetail logDetail)
        {
            _diagnosticLogger.Write(LogEventLevel.Information, "{@LogDetail}", logDetail);
        }
    }
}
