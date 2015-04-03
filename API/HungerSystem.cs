using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TAPI;
using Terraria;

namespace ToRT_Hunger
{
    public class HungerS : TAPI.ModPlayer
    {
        int starveTimer;
        int dehydrateTimer;
        public static double lastMessage = 0;
        public static double Hunger = 100;
        public static double Thirst = 101;
        public static float PlayerLife = 0;

        public class food
        {
            public static int MushroomValue = 5;
            public static int VileMushroomValue = 5;
            public static int GlowingMushroomValue = 4;
            public static int SoupValue = 20;
            public static int GoldfishValue = 8;
        }

        public class drinks
        {
            public static int BottledWaterValue = 15;
            public static int BottledHoneyValue = 11;
            public static int AleValue = 9;
            public static int SoupValue = 8;
            public static int SakeValue = 9;
            public static int EggNogValue = 9;
        }


        public static void doMessage(int Hardness)
        {
            string text = "";

            switch (Hardness)
            {
                case 8: text = "I'm Feeling Thirsty"; break;
                case 7: text = "I'm Parched"; break;
                case 6: text = "I'm Dry As A Bone!"; break;
                case 5: text = "I'm Dehydrating..."; break;

                case 4: text = "I'm Peckish"; break;
                case 3: text = "I'm Hungry"; break;
                case 2: text = "I'm Very Hungry!"; break;
                case 1: text = "I'm Starving..."; break;
            }
            Player npc = Main.player[Main.myPlayer];
            CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(255, (int)(23 * 2.6f * Hardness), 0, 200), text);
        }

        public override void Save(BinBuffer bb)
        {
            bb.Write(Thirst);
            bb.Write(Hunger);
        }
        public override void Load(BinBuffer bb)
        {
            Thirst = bb.ReadDouble();
            Hunger = bb.ReadDouble();
        }

        public static void EatFood(float value, ModBase obj)
        {
            Hunger += value * ToRT_Hunger.HungerBase.hungerRate; //decrease hunger + rate
        }

        public static void Drink(float value, ModBase obj)
        {
            Thirst += value * ToRT_Hunger.HungerBase.thirstRate; //decrease thirst + rate
        }

        public static void updateHunger(Item itm, ModBase obj, Player P)
        {
            if ((itm.damage > 0 || itm.axe > 0 || itm.pick > 0 || itm.hammer > 0) &&
            Main.player[Main.myPlayer] != null && ToRT_Hunger.HungerS.Hunger > 0)
            {
                //Main.NewText("B" + ToRT.MPlayer.Hunger);
                float consume = 0;
                if (itm.magic) consume = 0.01f; //on magic cast
                else
                    if (itm.ConsumeAmmo(P)) consume = 0.007f; //on shoot
                    else
                        if (itm.ConsumeItem(P)) consume = 0.007f; //on item use
                        else
                            consume = (itm.useTime / 800); // if axe/hammer/item with damage 17/800 = 0.02125
                ToRT_Hunger.HungerS.EatFood(-consume, obj);
            }
        }

        public static void updateThirst(Item itm, ModBase mb, Player P)
        {
            if ((itm.damage > 0 || itm.axe > 0 || itm.pick > 0 || itm.hammer > 0) &&
            Main.player[Main.myPlayer] != null && ToRT_Hunger.HungerS.Thirst > 0)
            {
                //Main.NewText("B" + ToRT.MPlayer.Hunger);
                float consume = 0;
                if (itm.magic) consume = 0.01f; //on magic cast
                else
                    if (itm.ConsumeAmmo(P)) consume = 0.007f; //on shoot
                    else
                        if (itm.ConsumeItem(P)) consume = 0.007f; //on item use
                        else
                            consume = (itm.useTime / 800); // if axe/hammer/item with damage 17/800 = 0.02125
                ToRT_Hunger.HungerS.Drink(-consume, mb);
            }
        }

        public override void PostUpdate()
        {
            if (player.whoAmI != Main.myPlayer) return;

            if (KState.Special.Alt.Down() && KState.Special.Ctrl.Down() && KState.Special.Shift.Down())
                HungerS.Hunger -= 0.1f; // debug thing
                HungerS.Thirst -= 0.1f; // debug thing

            if ((int)(Main.time % 60) == 0) 
            { 
                EatFood(-0.006944f, modBase);
                Drink(-0.006944f, modBase);
            } // one second -0.04f food || over time

            if (player.position.X != player.oldPosition.X && (player.controlLeft || player.controlRight))
            {
                Drink(-1 * Math.Abs(player.position.X - player.oldPosition.X) / (1200 + player.moveSpeedMod), modBase);
                if ((int)Thirst <= 0 && (int)(Main.time % 6) == 0)
                player.statLife -= (int)(Math.Abs(player.position.X - player.oldPosition.X) / 2);

                EatFood(-1 * Math.Abs(player.position.X - player.oldPosition.X) / (1200 + player.moveSpeedMod), modBase);
                if ((int)Hunger <= 0 && (int)(Main.time % 6) == 0)
                player.statLife -= (int)(Math.Abs(player.position.X - player.oldPosition.X) / 2);
            }

            if (Hunger < 0) Hunger = 0;
            if (player.dead || Hunger > 100) Hunger = 100;
            int startPeckish = 70;
            int startHungry = 50;
            int startVeryHungry = 25;
            int startStarving = ToRT_Hunger.Buffs.BUFF_Starving.StarvingAt;

            if ((int)Hunger < startPeckish && (Main.time - lastMessage) > 1.3 * 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * (3 + Main.rand.Next(1)));
                doMessage(4);
            }
            if ((int)Hunger < startHungry && (Main.time - lastMessage) > 1.1 * 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * (2 + Main.rand.Next(1)));
                doMessage(3);
            }
            if ((int)Hunger < startVeryHungry && (Main.time - lastMessage) > 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * (1 + Main.rand.Next(1)));
                doMessage(2);
            }
            if ((int)Hunger < startStarving && (Main.time - lastMessage) > 0.9 * 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * Main.rand.Next(1));
                doMessage(1);
            }

            #region Buffs
            if ((int)Hunger >= ToRT_Hunger.Buffs.BUFF_Saturated.SaturatedAt)
                player.AddBuff(BuffDef.byName["ToRT_Hunger:BUFF_Saturated"], 600);
            else
                player.ClearBuff(BuffDef.byName["ToRT_Hunger:BUFF_Saturated"]);

            if ((int)Hunger < ToRT_Hunger.Buffs.BUFF_Starving.StarvingAt && (int)Hunger > 0)
                player.AddBuff(BuffDef.byName["ToRT_Hunger:BUFF_Starving"], 600);
            else
                player.ClearBuff(BuffDef.byName["ToRT_Hunger:BUFF_Starving"]);

            if ((int)Thirst < ToRT_Hunger.Buffs.BUFF_Dehydrating.DehydratingAt && (int)Thirst > 0)
                player.AddBuff(BuffDef.byName["ToRT_Hunger:BUFF_Dehydrating"], 600);
            else
                player.ClearBuff(BuffDef.byName["ToRT_Hunger:BUFF_Dehydrating"]);
            #endregion

            #region <= 0
            if ((int)Hunger <= 0) // kill
            {
                player.lifeRegenCount = 0;
                player.lifeRegenTime = 0;
                player.lifeRegen = 0;
                player.moveSpeedMax /= 14;

                starveTimer++;
                if (starveTimer >= 15)
                {
                    if (player.statLife < 2 || Main.rand.Next(11) > 8)
                        switch (Main.rand.Next(2))
                        {
                            case 0: player.Hurt(1, 0, false, false, " Didn't find food in time..."); break;
                            case 1: player.Hurt(1, 0, false, false, " Starved to death..."); break;
                        }
                    else
                        player.statLife -= 1;
                    starveTimer = 0;
                }
            }
            else
                starveTimer = 0;
            #endregion

            for (int i = 0; i < 49; i++)
            {
                if (player.inventory[i].type == 5) player.inventory[i].tooltip2 = "Restores " + food.MushroomValue + " food points."; // Mushroom
                if (player.inventory[i].type == 60) player.inventory[i].tooltip2 = "Restores " + food.VileMushroomValue + " food points."; // Vile Mushroom
                if (player.inventory[i].type == 183) player.inventory[i].tooltip2 = "Restores " + food.GlowingMushroomValue + " food points."; // Glowing Mushroom
                if (player.inventory[i].type == 357) player.inventory[i].tooltip2 = "Restores " + food.SoupValue + " food points."; //Bowl of Soup
                if (player.inventory[i].type == 126) player.inventory[i].tooltip2 = "Quenches " + drinks.BottledWaterValue + " thirst.";
                if (player.inventory[i].type == 1134) player.inventory[i].tooltip2 = "Quenches " + drinks.BottledHoneyValue + " thirst.";
                if (player.inventory[i].type == 353) player.inventory[i].tooltip2 = "Quenches " + drinks.AleValue + " thirst.";
                if (player.inventory[i].type == 357) player.inventory[i].tooltip2 = "Quenches " + drinks.SoupValue + " thirst.";
                if (player.inventory[i].type == 1912) player.inventory[i].tooltip2 = "Quenches " + drinks.EggNogValue + " thirst.";
                if (player.inventory[i].type == 2266) player.inventory[i].tooltip2 = "Quenches " + drinks.SakeValue + " thirst.";
            }

            if (Thirst < 0) Thirst = 0;
            if (player.dead || Thirst > 100) Thirst = 100;

            int startFeelingThirsty = 70;
            int startParched = 50;
            int startBoneDry = 25;
            int startDehydrating = ToRT_Hunger.Buffs.BUFF_Dehydrating.DehydratingAt;

            if ((int)Thirst < startFeelingThirsty && (Main.time - lastMessage) > 1.3 * 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * (3 + Main.rand.Next(1)));
                doMessage(8);
            }
            if ((int)Thirst < startParched && (Main.time - lastMessage) > 1.1 * 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * (2 + Main.rand.Next(1)));
                doMessage(7);
            }
            if ((int)Thirst < startBoneDry && (Main.time - lastMessage) > 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * (1 + Main.rand.Next(1)));
                doMessage(6);
            }
            if ((int)Thirst < startDehydrating && (Main.time - lastMessage) > 0.9 * 60 * 60) //60*Seconds (60*60*Minutes) After last starve message
            {
                lastMessage = Main.time + (60 * 60 * Main.rand.Next(1));
                doMessage(5);
            }

            #region Thirst <= 0
            if ((int)Thirst <= 0) // kill
            {
                player.lifeRegenCount = 0;
                player.lifeRegenTime = 0;
                player.lifeRegen = 0;
                player.moveSpeedMax /= 14;

                dehydrateTimer++;
                if (dehydrateTimer >= 15)
                {
                    if (player.statLife < 2 || Main.rand.Next(11) > 8)
                        switch (Main.rand.Next(2))
                        {
                            case 0: player.Hurt(1, 0, false, false, " Forgot How To Drink..."); break;
                            case 1: player.Hurt(1, 0, false, false, " Dehydrated..."); break;
                        }
                    else
                        player.statLife -= 1;
                    dehydrateTimer = 0;
                }
            }
            else
                dehydrateTimer = 0;
            #endregion
        }
    }

    public class hungerHUD : TAPI.ModInterface
    {
        public override bool PreDrawInterface(SpriteBatch sb)
        {
            if (Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].pvpDeath)

            HungerS.Hunger = 100;
            HungerS.Thirst = 100;

            string Sprite = "ToRT_Hunger:HungerIcon";
            string TSprite = "ToRT_Hunger:ThirstIcon";

            int x = Main.screenWidth - 66;
            int y = Main.screenHeight - 565;
            int x2 = Main.screenWidth - 92;

            int iH = Main.goreTexture[GoreDef.gores[Sprite]].Height;
            int iW = Main.goreTexture[GoreDef.gores[Sprite]].Width;
            int iH2 = Main.goreTexture[GoreDef.gores[TSprite]].Height;
            int iW2 = Main.goreTexture[GoreDef.gores[TSprite]].Width;

            /*int xx = (int)((24 - iW) / 2) + Main.screenWidth - sX;
            int yy = (int)((24 - iH) / 2) + sY + (int)(Main.player[Main.myPlayer].statManaMax2 * 1.4) + (24 - iW) * (-1) + 20;
            int xx2 = (int)((24 - iW2) / 2) + Main.screenWidth - sX;
            int yy2 = (int)((24 - iH2) / 2) + 175 + (int)(Main.player[Main.myPlayer].statManaMax2 * 1.4) + (24 - iW2) * (-1) + 20;

            sb.DrawString(Main.fontMouseText, "Food ", new Vector2(xx - 12, yy - 18 + (24 - iW) * (-1)), Color.White);
            sb.DrawString(Main.fontMouseText, "Water ", new Vector2(xx - 15, yy + 128 + (24 - iW2) * (-1)), Color.White);*/

            float hD = (float)HungerS.Hunger - ((int)(HungerS.Hunger / 20) * 20);
            float hD2 = (float)HungerS.Thirst - ((int)(HungerS.Thirst / 20) * 20);

            if ((int)HungerS.Hunger == 100)
            for (int i = 0; i < (int)(HungerS.Hunger / 20); i++)
            {
                sb.Draw(Main.goreTexture[GoreDef.gores[Sprite]],
                                new Vector2(x, y),
                                new Rectangle?(new Rectangle(0, 0, iW, iH)),
                                Color.White,
                                0.0f,
                                new Vector2(),//Center point, for an NPC it should be 0,0 //200
                                1, //Scale  /2.1f * Main.inventoryScale
                                SpriteEffects.None,
                                1);
            }
            if ((int)HungerS.Thirst == 100)
            for (int i = 0; i < (int)(HungerS.Thirst / 20); i++)
            {
                sb.Draw(Main.goreTexture[GoreDef.gores[TSprite]],
                                new Vector2(x2, y),
                                new Rectangle?(new Rectangle(0, 0, iW2, iH2)),
                                Color.White,
                                0.0f,
                                new Vector2(),//Center point, for an NPC it should be 0,0 //200
                                1, //Scale  /2.1f * Main.inventoryScale
                                SpriteEffects.None,
                                1);
            }

            if ((int)HungerS.Thirst != 100)
            {
                float alpha = 0.2f + hD2 * 0.04f; //0.5f + hD * 0.025f;
                float scale = 0.8f + hD2 * 0.01f;
                float sW = (24 - 24 * scale) / 2;
                float sH = (24 - 24 * scale) / 2;

                sb.Draw(Main.goreTexture[GoreDef.gores[TSprite]],
                new Vector2(x2 + sW, y + sH + ((int)(HungerS.Thirst / 20)) * (iH2 + 2)),
                new Rectangle?(new Rectangle(0, 0, iW2, iH2)),
                Color.White * alpha,
                0.0f,
                new Vector2(),//Center point, for an NPC it should be 0,0 //200
                scale, //Scale  /2.1f * Main.inventoryScale
                SpriteEffects.None,
                1);

                scale = 0.85f;
                sW = (24 - 24 * scale) / 2;
                sH = (24 - 24 * scale) / 2;
                if ((int)HungerS.Thirst < 30)
                    for (int i = (int)(HungerS.Thirst / 20) + 1; i < 1; i++)
                    {
                        sb.Draw(Main.goreTexture[GoreDef.gores[TSprite]],
                        new Vector2(x2 + sW, y + sH + i * (iH2 + 2)),
                        new Rectangle?(new Rectangle(0, 0, iW, iH)),
                        Color.White * 0.2f,
                        0.0f,
                        new Vector2(),//Center point, for an NPC it should be 0,0 //200
                        scale, //Scale  /2.1f * Main.inventoryScale
                        SpriteEffects.None,
                        1); //*0.625f
                    }
            }

            if ((int)HungerS.Hunger != 100)
            {
                float alpha = 0.2f + hD * 0.04f; //0.5f + hD * 0.025f;
                float scale = 0.8f + hD * 0.01f;
                float sW = (24 - 24 * scale) / 2;
                float sH = (24 - 24 * scale) / 2;

                sb.Draw(Main.goreTexture[GoreDef.gores[Sprite]],
                new Vector2(x, y),
                new Rectangle?(new Rectangle(0, 0, iW, iH)),
                Color.White * alpha,
                0.0f,
                new Vector2(),//Center point, for an NPC it should be 0,0 //200
                scale, //Scale  /2.1f * Main.inventoryScale
                SpriteEffects.None,
                1);

                scale = 0.85f;
                sW = (24 - 24 * scale) / 2;
                sH = (24 - 24 * scale) / 2;
                if ((int)HungerS.Hunger < 30)
                    for (int i = (int)(HungerS.Hunger / 20) + 1; i < 1; i++)
                    {
                        sb.Draw(Main.goreTexture[GoreDef.gores[Sprite]],
                        new Vector2(x, y),
                        new Rectangle?(new Rectangle(0, 0, iW, iH)),
                        Color.White * 0.2f,
                        0.0f,
                        new Vector2(),//Center point, for an NPC it should be 0,0 //200
                        scale, //Scale  /2.1f * Main.inventoryScale
                        SpriteEffects.None,
                        1); //*0.625f
                    }
            }
            if (Main.mouseX < x + iW && Main.mouseX > x && Main.mouseY > y && Main.mouseY < y + (iH + 2))
                if (KState.Special.Ctrl.Down() && KState.Special.Shift.Down())
                    API.main.MouseText((HungerS.Hunger) + "");
                else
                    API.main.MouseText("Hunger   " + System.Environment.NewLine + ((int)HungerS.Hunger) + "/100");
            else if (Main.mouseX < x2 + iW2 && Main.mouseX > x2 && Main.mouseY > y && Main.mouseY < y + (iH2 + 2))
                if (KState.Special.Ctrl.Down() && KState.Special.Shift.Down())
                    API.main.MouseText((HungerS.Thirst) + "");
                else
                    API.main.MouseText("Thirst  " + System.Environment.NewLine + ((int)HungerS.Thirst) + "/100");
            return true;
        }
    }
}