using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamRPAExample
{
    class LoadedRecipe
    {
        public string Name { get; set; }
        public List<string> IceCreamFlavors { get; set; }
        public List<string> MixIns { get; set; }
    }

    public class RecipeAnalysis
    {
        public bool IsValidRecipe { get; set; }
        public string RecipeName { get; set; }
        public List<string> InputIngredients { get; set; } = new List<string>();

        public RecipeAnalysis(List<string> iceCreamFlavors, List<string> mixins)
        {
            foreach(string flavor in iceCreamFlavors)
            {
                this.InputIngredients.Add($"{flavor} ice cream");
            }
            foreach(string mixin in mixins)
            {
                this.InputIngredients.Add(mixin);
            }
        }

    }


    public class RecipeAnalyzer
    {
        List<string> _icecreamFlavors;
        List<string> _mixIns;
        static List<LoadedRecipe> _knownRecipes;

        static RecipeAnalyzer()
        {
            string recipeJson;
            using (var fs = new StreamReader("recipes.json"))
            {
                recipeJson = fs.ReadToEnd();
                _knownRecipes = JsonConvert.DeserializeObject<List<LoadedRecipe>>(recipeJson);
            }
        }

        public RecipeAnalyzer(RecipeModel input)
        {
            _icecreamFlavors = input.GetIceCreamFlavors();
            _mixIns = input.GetMixins();
        }

        bool ContainsFlavor(string flavor)
        {
            foreach (string inputFlavor in _icecreamFlavors)
            {
                if (!string.IsNullOrEmpty(inputFlavor) && inputFlavor.Equals(flavor, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        bool ContainsMixIn(string mixin)
        {
            foreach (string inputMixin in _mixIns)
            {
                if (!string.IsNullOrEmpty(inputMixin) && inputMixin.Equals(mixin, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private int FlavorCount
        {
            get => _icecreamFlavors.Count;
        }

        public RecipeAnalysis GetAnalysis()
        {
            RecipeAnalysis analysis = new RecipeAnalysis(_icecreamFlavors, _mixIns);
            string recipeName = null;
            bool isValidRecipe = true;

            if (this.FlavorCount == 0)
            {
                recipeName = "Must provide at least one ice cream flavor";
                isValidRecipe = false;
            }
            else if (this.FlavorCount == 1 && this.MixInCount == 0)
            {
                recipeName = _icecreamFlavors[0];
            }
            else if (this.IsKnownRecipe(out string knownName))
            {
                recipeName = knownName;
            }
            else
            {
                recipeName = "You created a new flavor";
            }

            analysis.RecipeName = recipeName;
            analysis.IsValidRecipe = isValidRecipe;

            return analysis;
        }


        internal bool IsKnownRecipe(out string recipeName)
        {
            recipeName = null;

            foreach (var recipe in _knownRecipes)
            {
                bool containsAllFlavors = recipe.IceCreamFlavors.Count == this.FlavorCount;
                if (containsAllFlavors && this.FlavorCount > 0)
                {
                    foreach (string flavor in recipe.IceCreamFlavors)
                    {
                        if (!ContainsFlavor(flavor))
                        {
                            containsAllFlavors = false;
                            break;
                        }
                    }
                }

                int recipeMixinCount = recipe.MixIns != null ? recipe.MixIns.Count : 0;
                bool containsAllMixIns = recipeMixinCount == this.MixInCount;

                if (containsAllMixIns && recipeMixinCount > 0)
                {
                    foreach (string mixIn in recipe.MixIns)
                    {
                        if (!ContainsMixIn(mixIn))
                        {
                            containsAllMixIns = false;
                            break;
                        }
                    }
                }

                if (containsAllFlavors && containsAllMixIns)
                {
                    recipeName = recipe.Name;
                    return true;
                }
            }

            return false;
        }

        private int MixInCount
        {
            get => _mixIns.Count;
        }

    }
}
