using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreApp.Application.Interfaces;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace NetCoreApp.Authorization
{
    public class BaseResourceAuthorizationHandler:AuthorizationHandler<OperationAuthorizationRequirement,string>
    {
        private readonly IRoleService _roleService;

        public BaseResourceAuthorizationHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement and resource.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <param name="resource">The resource to evaluate.</param>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            string resource)
        {
            var roles = ((ClaimsIdentity) context.User.Identity).Claims.FirstOrDefault(x =>
                x.Type == CommonConstants.UserClaims.Roles);
            if (roles != null)
            {
                var listRole = roles.Value.Split(";");
                var hasPermission = await _roleService.CheckPermission(resource, requirement.Name, listRole);
                if (hasPermission || listRole.Contains(CommonConstants.AppRole.AdminRole))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
