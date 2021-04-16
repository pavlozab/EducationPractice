using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/roles")]
    public class RoleController : ControllerBase
    {
        
    }
}