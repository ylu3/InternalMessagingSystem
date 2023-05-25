using Microsoft.AspNetCore.Mvc;

namespace InternalMessagingSystem.Controllers
{
    /// <summary>
    /// Home controller for handling errors in the internal messaging system.
    /// </summary>
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Action method to handle errors and return a problem response.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the problem response.</returns>
        [Route("/Error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() =>
            Problem();
    }
}
