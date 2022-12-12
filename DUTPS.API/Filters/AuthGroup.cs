using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DUTPS.API.Filters
{
    public class AuthGroup : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int[] _groups;
        public AuthGroup()
        {
            _groups = new int[] { DUTPS.Commons.CodeMaster.Role.Admin.CODE};
        }
        public AuthGroup(int group)
        {
            _groups = new int[] { group };
        }
        public AuthGroup(int[] groups)
        {
            _groups = groups;
        }

        public bool hasAccess(int[] groups) {
            foreach (var group in _groups){
                if (groups.Contains(group))
                {
                    return true;
                }
            }
            return false;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var user = context.HttpContext.User;

                if (!user.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                else
                {
                    var role = user.FindFirst("Role").Value;
                    if (!_groups.Contains(Convert.ToInt32(role)))
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                context.Result = new BadRequestObjectResult(e.Message);
                return;
            }
        }
    }
}
