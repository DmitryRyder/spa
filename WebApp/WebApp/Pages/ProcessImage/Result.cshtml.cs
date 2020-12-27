using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.ProcessImage
{
    public class ResultModel : PageModel
    {
        public IActionResult OnGet(string name, int age)
        {
            return Content($"Запрошенные данные: name {name} age {age}");
        }
    }
}
