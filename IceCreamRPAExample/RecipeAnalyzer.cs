using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamRPAExample
{
    public class RecipeAnalyzer
    {
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
            else if (recipe.IsMintChocolateChip)
            {
                return "Mint Chocolate Chip";
            }
            else if (recipe.IsNeopolitan)
            {
                return "Neopolitan";
            }

            return null;
        }


        private RecipeModel _input;

        private RecipeAnalyzer(RecipeModel input)
        {
            this._input = input;
        }

        private int FlavorCount
        {
            get
            {
                int count = 0;
                count = !string.IsNullOrEmpty(_input.IceCream1) ? count + 1 : count;
                count = !string.IsNullOrEmpty(_input.IceCream2) ? count + 1 : count;
                count = !string.IsNullOrEmpty(_input.IceCream3) ? count + 1 : count;
                return count;
            }
        }

        private int MixInCount
        {
            get
            {
                int count = 0;
                count = !string.IsNullOrEmpty(_input.MixIn1) ? count + 1 : count;
                count = !string.IsNullOrEmpty(_input.MixIn2) ? count + 1 : count;
                count = !string.IsNullOrEmpty(_input.MixIn3) ? count + 1 : count;
                return count;
            }
        }

        bool IsMintChocolateChip
        {
            get => this.FlavorCount == 1
                && _input.IceCream1.Equals("mint", StringComparison.OrdinalIgnoreCase)
                && this.MixInCount == 1
                && _input.MixIn1.Equals("chocolate chips", StringComparison.OrdinalIgnoreCase);
        }

        bool IsNeopolitan
        {
            get => this.FlavorCount == 3
                && ContainsFlavor("chocolate")
                && ContainsFlavor("vanilla")
                && ContainsFlavor("strawberry");
        }

        bool ContainsFlavor(string flavor)
        {
            return _input.IceCream1.Equals(flavor, StringComparison.OrdinalIgnoreCase)
                || _input.IceCream2.Equals(flavor, StringComparison.OrdinalIgnoreCase)
                || _input.IceCream3.Equals(flavor, StringComparison.OrdinalIgnoreCase);
        }


    }
}
