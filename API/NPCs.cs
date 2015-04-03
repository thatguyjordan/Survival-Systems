using System.Collections.Generic;

using TAPI;
using Terraria;

namespace ToRT_Hunger
{
    [GlobalMod]
    public class hungerNPC : ModNPC
    {
/*        public override void PostNPCLoot()
        {
            if (!(bool)modBase.options["HungerOn"].Value) return;
            if ((float)Main.rand.NextDouble() < 0.38f) //30%
            {
                string itm = "";
                switch (npc.displayName)
                {
                    case "Chicken": itm = "ToRT_Hunger:ChickenMeat"; break; // Chicken
                    case "Cow": itm = "ToRT_Hunger:Beef"; break; // Cow
                    case "Bunny": itm = "ToRT_Hunger:RabbitMeat"; break; // Bunny
                    //case 47: // Corrupt Bunny
                    case "Corrupt Goldfish": itm = "ToRT_Hunger:CorruptedGoldfish"; break;// Corrupt Goldfish
                    case "Piranha": itm = "ToRT_Hunger:Piranha"; break; // Piranha
                    //case 61:// Vulture
                    case "Squid": itm = "ToRT_Hunger:SquidMeat"; break; // Squid 221
                    case "Shark": itm = "ToRT_Hunger:SharkMeat"; break; // Shark
                    case "Crab": itm = "ToRT_Hunger:CrabMeat"; break; // Crab
                    case "Blue Jay": itm = "ToRT_Hunger:BirdMeat"; break; // Blue Jay 297
                    case "Cardinal": itm = "ToRT_Hunger:BirdMeat"; break; // Cardinal 298
                    case "Bird": itm = "ToRT_Hunger:BirdMeat"; break; // Bird
                    //case 299:// Squirrel
                    //case 86:// Unicorn
                    //case 93:// Giant Bat
                    case "Angler Fish": itm = "ToRT_Hunger:AnglerFish"; break; // Angler Fish
                    //case 104:// Werewolf
                    //case 220:// Sea Snail

                    //case 300:// Mouse
                    //case 301:// Raven
                    //Duck
                    //Worm
                    //a lot of fish
                }
                if (itm != "")
                    Functions.DropItemS((int)npc.position.X, (int)npc.position.Y, itm, 1, true);
            }
        }*/
    }
}