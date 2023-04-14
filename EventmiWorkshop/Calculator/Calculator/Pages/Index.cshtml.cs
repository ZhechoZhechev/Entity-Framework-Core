using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Expression { get; set; }

        public string Result { get; private set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            try
            {
                // Изчисляване на резултата на израза
                Result = new DataTable().Compute(Expression, null).ToString();
            }
            catch
            {
                // Грешка при изчисляването на резултата
                Result = "Error";
            }
        }
    }
}
