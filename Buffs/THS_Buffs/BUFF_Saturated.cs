using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using TAPI;
using Terraria;

namespace ToRT_Hunger.Buffs
{
    public class BUFF_Saturated : TAPI.ModBuff
    {
        public static int SaturatedAt = 95;
        //public override void Start(Player player, int index){} //called when the buff begins.
        //public override void End(Player player, int index){} //called when the buff ends.
        public override void Effects(Player player, int index)
		{
            if (ToRT_Hunger.HungerS.Hunger < SaturatedAt) return;
            float hungerLeft = (float)(ToRT_Hunger.HungerS.Hunger - SaturatedAt);
            float coif = (100 - SaturatedAt) / 5;                                 // from 100 to SaturatedAt
            player.pickSpeed -= player.pickSpeed * hungerLeft * 0.06f / coif;     // from +1.3x to 1x mining speed 
            player.jumpSpeedBoost += hungerLeft / 3.333333333333333f * coif;      // from +1.5 to 0                             
            player.arrowDamage += player.arrowDamage * hungerLeft * 0.08f / coif; // from +1.4x to 1x damage
            player.meleeDamage += player.meleeDamage * hungerLeft * 0.12f / coif; // from +1.6x to 1x damage
            player.moveSpeed   +=   player.moveSpeed * hungerLeft * 0.06f / coif; // from +1.3x to 1x speed
		}

		// public override Color ModifyDrawColor(Player player, Color color) { return color; }
    }
}