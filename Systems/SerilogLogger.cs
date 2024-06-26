﻿using Microsoft.Extensions.Configuration;
using Serilog;

namespace MudBucket.Systems
{
    public class SerilogLogger : Interfaces.ILogger
    {
        public SerilogLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public void Information(string message) => Log.Information(message);
        public void Debug(string message) => Log.Debug(message);
        public void Error(string message) => Log.Error(message);
    }
}
