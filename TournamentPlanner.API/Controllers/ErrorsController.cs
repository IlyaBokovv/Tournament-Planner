using Microsoft.AspNetCore.Mvc;

namespace TournamentPlanner.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
