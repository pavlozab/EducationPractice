using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MyApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            var userId = ControllerContext.HttpContext.User.Claims.Where(obj => 
                    obj.Type == "UserId")
                .Select(obj => obj.Value).SingleOrDefault();
            if (userId is null)
                throw new SecurityTokenValidationException("Invalid token");
            return new Guid(userId);
        }
    }
}