using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IceCreamRPAExample.Pages
{

    public class IndexModel : PageModel
    {
        [BindProperty]
        public RecipeModel Recipe { get; set; }

        [BindProperty]
        public string FlavorName { get; set; }
        [BindProperty]
        public bool ShowFlavorCreation { get; set; } = false;

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            await Task.Delay(500);
            this.ShowFlavorCreation = true;
            this.FlavorName = RecipeAnalyzer.GetRecipeName(this.Recipe);
        }
    }
}
