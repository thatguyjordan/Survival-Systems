using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using TAPI;
using Terraria;

namespace ToRT_Hunger.Buffs
{
    public class BUFF_Dehydrating : TAPI.ModBuff
    {
        public static int DehydratingAt = 10;
        //public override void Start(Player player, int index){} //called when the buff begins.
        //public override void End(Player player, int index){} //called when the buff ends.
        public override void Effects(Player player, int index)
		{
            if (ToRT_Hunger.HungerS.Thirst > DehydratingAt) return;
            float thirstLeft = (DehydratingAt - (float)ToRT_Hunger.HungerS.Thirst) / 2;
            float coif = DehydratingAt / 5;                                          // from 0 to StarvingAt
            player.pickSpeed += player.pickSpeed * thirstLeft * 0.06f / coif;     // from -1.3x to 1x mining speed 
            player.jumpSpeedBoost -= thirstLeft / 3.333333333333333f * coif;      // from -1.5 to 0                             
            player.arrowDamage -= player.arrowDamage * thirstLeft * 0.08f / coif; // from -1.4x to 1x damage
            player.meleeDamage -= player.meleeDamage * thirstLeft * 0.12f / coif; // from -1.6x to 1x damage
            player.moveSpeed -= player.moveSpeed * thirstLeft * 0.2f / coif;      // from -2x to 1x speed
		}

		// public override Color ModifyDrawColor(Player player, Color color) { return color; }
    }
}