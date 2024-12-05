using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        public UserController(ILogger logger) : base(logger)
        {
        }
    }
}
