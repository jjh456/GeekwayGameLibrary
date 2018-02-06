using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BoardGameLibrary.Api
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!actionExecutedContext.ActionContext.ModelState.IsValid)
            {
                var statusCode = actionExecutedContext.ActionContext.Response.StatusCode;
                //var content = actionExecutedContext.ActionContext.Response.Content;
                //var reason = actionExecutedContext.ActionContext.Response.ReasonPhrase;
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(statusCode, actionExecutedContext.ActionContext.ModelState);
            }
        }
    }
}