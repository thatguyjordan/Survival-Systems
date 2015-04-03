using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TAPI;
using Terraria;

namespace ToRT_Hunger
{
    public class HungerBase: ModBase
    {
        public static float hungerRate = 1;

        public static float thirstRate = 2;

        public static bool doSpecificSpawn = true;

        public static void loadOptions(ModBase obj)
        {
            float multiply = 1.6f;
            float multiply2 = 1.6f;
            switch ((string)obj.options["hungerRate"].Value)
            {
                case "Slower - 1": hungerRate = 1f * multiply; break;
                case "Normal - 1.5": hungerRate = 1.5f * multiply; break;
                default: hungerRate = 1.5f; break;
            }
            switch ((string)obj.options["thirstRate"].Value)
            {
                case "Slower - 1": thirstRate = 1f * multiply2; break;
                case "Normal - 1.5": thirstRate = 1.5f * multiply2; break;
                case "DEBUG": thirstRate = 9.5f * multiply2; break;
                default: thirstRate = 1.5f; break;
            }
        }

        public override void OnLoad()
        {   
            /*       
            #region Recipe for Hay!
            Recipe.newRecipe.createItem.SetDefaults(1727); // Hey
            Recipe.newRecipe.requiredItem.Add(new Item());
            Recipe.newRecipe.requiredItem[0].SetDefaults(ItemDef.byName["ToRT_Hunger:Grass"].type);
            Recipe.newRecipe.requiredItem[0].SetStack(6);

            Recipe.newRecipe.requiredTile.Add(17);
            Recipe.AddRecipe();
            #endregion
            */
        }
    }
}