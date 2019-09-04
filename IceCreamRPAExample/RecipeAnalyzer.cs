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

        private RecipeAnalyzer(RecipeModel input)
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

        public static string GetRecipeName(RecipeModel inputRecipe)
        {
            RecipeAnalyzer recipe = new RecipeAnalyzer(inputRecipe);

            if (recipe.FlavorCount == 0)
            {
                return "Must provide at least one ice cream flavor";
            }
            else if (recipe.FlavorCount == 1 && recipe.MixInCount == 0)
            {
                return inputRecipe.IceCream1;
            }
            else if (recipe.IsKnownRecipe(out string recipeName))
            {
                return recipeName;
            }

            return null;
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
