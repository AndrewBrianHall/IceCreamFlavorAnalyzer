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

        public RecipeAnalysis Analysis { get; set; }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            await Task.Delay(500);

            RecipeAnalyzer recipeAnalyzer = new RecipeAnalyzer(this.Recipe);
            this.Analysis = recipeAnalyzer.GetAnalysis();

            if (this.Analysis.IsValidRecipe)
            {
                this.Recipe.IceCream1 = null;
                this.Recipe.IceCream2 = null;
                this.Recipe.IceCream3 = null;
                this.Recipe.MixIn1 = null;

                const string recipeModelName = nameof(this.Recipe);
                string[] modelFieldsToClear = new string[]
                {
                    $"{recipeModelName}.{nameof(Recipe.IceCream1)}",
                    $"{recipeModelName}.{nameof(Recipe.IceCream2)}",
                    $"{recipeModelName}.{nameof(Recipe.IceCream3)}",
                    $"{recipeModelName}.{nameof(Recipe.MixIn1)}"
                };

                foreach (string modelKey in modelFieldsToClear)
                {
                    this.ModelState[modelKey].RawValue = null;
                }
            }
        }
    }
}
