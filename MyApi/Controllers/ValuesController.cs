using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MyApi.Controllers
{
    [Authorize]
    public class ValuesController : Controller
    {
        [HttpGet]
        [Route("test")]
        public IActionResult Get()
        {
            var claims = from c in User.Claims
                         select new { c.Type, c.Value };

            return new JsonResult(claims);
        }

        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

    }
}
