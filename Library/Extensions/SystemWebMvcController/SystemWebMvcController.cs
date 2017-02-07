using System;
using System.Linq;

namespace Library.Extensions.SystemWebMvcController
{
    public static class Extensions
    {
        public static string SoleErrorMessage(this System.Web.Mvc.Controller controller, string modelKey)
        {
            Console.Write("Keys:");
            foreach (var key in controller.ModelState.Keys)
                Console.Write(" " + key);
            var errors = controller.ModelState[modelKey]?.Errors;
            if (errors != null && errors.Any())
                return errors.First().ErrorMessage;
            return "*** No errors ***";
        }
    }
}