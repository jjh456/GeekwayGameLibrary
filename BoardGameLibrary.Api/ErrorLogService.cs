using NLog;
using System;

namespace BoardGameLibrary.Api
{
    public static class ErrorLogService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Log(Exception ex, LogLevel level, string message = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    logger.Log(level, ex);
                else
                    logger.Log(level, ex, message);
            }
            catch { }
        }
    }
}