// Importing the necessary namespace for working with MVC and Controllers in ASP.NET Core
using Microsoft.AspNetCore.Mvc;

namespace ST10251759_PROG6212_POE.Controllers
{
    // Defining the DefaultUserController class, which is a controller for managing default user-related actions.
    // The controller inherits from the Controller class, which provides methods for handling HTTP requests and rendering views.
    public class DefaultUserController : Controller
    {
        // The Index method responds to the default HTTP GET request to the "Index" route of this controller.
        // This method will be called when the user navigates to the /DefaultUser/Index URL.
        // The method returns the default view associated with this action.
        public IActionResult Index()
        {
            // Returning the "Index" view. This will render a view with the same name as the action (Index.cshtml) by default.
            return View();
        }
    }
}
