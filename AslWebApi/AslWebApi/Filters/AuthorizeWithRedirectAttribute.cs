using Microsoft.AspNetCore.Mvc.Filters;

namespace AslWebApi.Filters
{
    /// <summary>
    /// Check if token exists in cookie. if not exists then redirect to login page.
    /// </summary>
    public class AuthorizeWithRedirectAttribute : ActionFilterAttribute
    {


        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //context.HttpContext.Response.Headers.Add();

            var token = context!.HttpContext!.Request.Cookies["AslWebApiCookie"];
            if (string.IsNullOrEmpty(token))
            {
                context.HttpContext.Response.Redirect("/home/login");
                return;
            }

            base.OnResultExecuting(context);
        }
    }
}
