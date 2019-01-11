using NLog;
using System.Web.Http.Filters;

namespace BoardGameLibrary.Api
{
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ErrorLogService.Log(context.Exception, LogLevel.Error);
        }
    }
}