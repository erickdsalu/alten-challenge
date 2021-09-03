using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]

    public class BaseController : ControllerBase
    {
    }
}
