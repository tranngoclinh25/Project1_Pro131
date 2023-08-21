using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult ClearSession()
        {
            // Clear the session if it exists.
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("YourSessionKeyName");

            return Ok();
        }
    }
}
