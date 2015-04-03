using System.Collections.Generic;

using TAPI;
using Terraria;

namespace ToRT_Hunger
{
    [GlobalMod]
    public class hungerTile : ModTileType
    {

        int PreKilling = -1;

/*        public override void PostKill(int x, int y, bool fail, bool effectsOnly, bool noItem)
        {

            if ((float)Main.rand.NextDouble() < 0.13f)
            {
                if ((PreKilling == 5 && !fail && (int)Main.tile[x, y + 1].type != 5) && Main.player[Main.myPlayer] != null)
                {
                    // 2grass 23corrupdet 60jung 70mush 109hallow 147snow 199crimson  200Crimson Ice 201Crimson Grass                   

                }
            }
        }
*/
        public override bool PreKill(int x, int y, bool fail, bool effectsOnly, bool noItem)
        {
            PreKilling = (int)Main.tile[x, y].type;
            return base.PreKill(x, y, fail, effectsOnly, noItem);
        }
    }
}