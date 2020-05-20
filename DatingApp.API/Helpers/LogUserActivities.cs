using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    ///This class used as Action filter to set the user last activity.summary
    public class LogUserActivities : IAsyncActionFilter
    {
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var unique = Guid.Parse(userId);
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            var user = await repo.GetUser(unique);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}