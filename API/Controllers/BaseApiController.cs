using Microsoft.AspNetCore.Mvc;
using API.Helpers;
using API.Sevices;

namespace API.Controllers;


[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]

public class BaseApiController : ControllerBase
{

}