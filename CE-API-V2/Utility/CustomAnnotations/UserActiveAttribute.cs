using CE_API_V2.Models.Records;
using CE_API_V2.Services.Interfaces;
using CE_API_V2.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CE_API_V2.Utility.CustomAnnotations
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserActiveAttribute : Attribute, IActionFilter
    {
        IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!AnyUserAllowed(context.ActionDescriptor))
            {
                context.Result = DetermineResult(context);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private IActionResult? DetermineResult(ActionExecutingContext context)
        {
            _serviceProvider = context.HttpContext.RequestServices;

            var userUow = GetService<IUserUOW>();
            var userInformation = GetUserId(context);

            if (userInformation is null)
            {
                return new ObjectResult("Could not retrieve user information")
                {
                    StatusCode = 404
                };
            }

            var user = userUow.GetUser(userInformation.UserId, userInformation);

            if (user is null)
            {
                return new ObjectResult("User was not found")
                {
                    StatusCode = 404
                };

            }

            return user.IsActive ? null : new ForbidResult();
        }

        private UserIdsRecord GetUserId(ActionExecutingContext context)
        {
            var userInformationExtractor = GetService<IUserInformationExtractor>();

            return userInformationExtractor.GetUserIdInformation(context.HttpContext.User);
        }

        private T GetService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

        private static bool AnyUserAllowed(ActionDescriptor actionDescriptor)
            => actionDescriptor.EndpointMetadata.Any(e => e is AllowInActiveUserAttribute);
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowInActiveUserAttribute : Attribute
    {
    }
}