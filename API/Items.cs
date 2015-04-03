using System.Collections.Generic;

using TAPI;
using Terraria;

namespace ToRT_Hunger
{
    [GlobalMod]
    public class hungerItem : ModItem
    {
        public static bool Food = false;

        public static bool isFoodItem(Item itm, bool upHunger, ModBase obj)
        {
            Food = false;
            if (itm.type > Main.maxItemTypes) //1866 max itemID for 1.2.4.1
            {
                int foodValue = 0; 
                if (itm.def.json.Has("foodValue") && itm.def.json["foodValue"].IsInt) foodValue = (int)itm.def.json["foodValue"]; // written by zetaPRIME
                if (foodValue > 0)
                {
                    Food = true;
                    if (upHunger) HungerS.EatFood(foodValue, obj); //Main.NewText("f " + foodValue);
                    return true;
                }
            }
            return false;
        }

        public static bool Drinkable = false;

        public static bool isDrinkableItem(Item itm, bool upThirst, ModBase obj)
        {
            Drinkable = false;
            if (itm.type > Main.maxItemTypes) //1866 max itemID for 1.2.4.1
            {
                int thirstValue = 0;
                if (itm.def.json.Has("thirstValue") && itm.def.json["thirstValue"].IsInt) thirstValue = (int)itm.def.json["thirstValue"]; // written by zetaPRIME
                if (thirstValue > 0)
                {
                    Food = true;
                    if (upThirst) HungerS.Drink(thirstValue, obj); //Main.NewText("f " + foodValue);
                    return true;
                }
            }
            return false;
        }

        public override bool? UseItem(Player P)
        {   
            if (P.whoAmI != Main.myPlayer) return false;

            HungerS.updateHunger(item, modBase, P); // update hunger according to item used
            HungerS.updateThirst(item, modBase, P);

            if (isFoodItem(item, true, modBase)) return true;           
            if (item.type == ItemDef.byName["Vanilla:Mushroom"].type) ToRT_Hunger.HungerS.EatFood(ToRT_Hunger.HungerS.food.MushroomValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Glowing Mushroom"].type) ToRT_Hunger.HungerS.EatFood(ToRT_Hunger.HungerS.food.GlowingMushroomValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Bowl of Soup"].type) ToRT_Hunger.HungerS.EatFood(ToRT_Hunger.HungerS.food.SoupValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Vile Mushroom"].type) ToRT_Hunger.HungerS.EatFood(ToRT_Hunger.HungerS.food.VileMushroomValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Goldfish"].type) ToRT_Hunger.HungerS.EatFood(ToRT_Hunger.HungerS.food.GoldfishValue, modBase);
            return null;

            if (isDrinkableItem(item, true, modBase)) return true;
            if (item.type == ItemDef.byName["Vanilla:Bottled Water"].type) ToRT_Hunger.HungerS.Drink(ToRT_Hunger.HungerS.drinks.BottledWaterValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Bowl of Soup"].type) ToRT_Hunger.HungerS.Drink(ToRT_Hunger.HungerS.drinks.SoupValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Bottled Honey"].type) ToRT_Hunger.HungerS.Drink(ToRT_Hunger.HungerS.drinks.BottledHoneyValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Ale"].type) ToRT_Hunger.HungerS.Drink(ToRT_Hunger.HungerS.drinks.AleValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Sake"].type) ToRT_Hunger.HungerS.Drink(ToRT_Hunger.HungerS.drinks.SakeValue, modBase);
            if (item.type == ItemDef.byName["Vanilla:Egg Nog"].type) ToRT_Hunger.HungerS.Drink(ToRT_Hunger.HungerS.drinks.EggNogValue, modBase);
            return null;
        }
    }
}