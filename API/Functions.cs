using System.Linq;

using Microsoft.Xna.Framework;

using TAPI;
using Terraria;

namespace ToRT_Hunger
{
    public static class Functions
    {
        public static int HaveB(int f, int w, int h, int x, int y, params ushort[] values)
        {
            int BlockFound = 0;
            int H = 0;
            int W = 0;
            try
            {
                for (H = 0; H <= (h * 2); H++)
                {
                    for (W = 0; W <= (w * 2); W++)
                    {
                        if (values.Contains(Main.tile[x + (W - w), y - h + (H - h) - f].type))
                        {
                            BlockFound++;
                        }
                    }
                }
            }
            catch { Main.NewText("!r: HaveB"); }
            return BlockFound;
        }

        public static void DropItemS(int x, int y, string itemN, int amount = 1, bool NPCsCoords = false) //drop where tile breaks
        {
            int index = -1;
            if (NPCsCoords)
                index = Item.NewItem(x, y, 16, 16, itemN);
            else
                index = Item.NewItem(x * 16, y * 16, 16, 18, itemN);
            if (Main.netMode == 1)
            {
                for (int i = 0; i < amount; i++)
                {
                    NetMessage.SendData(21, -1, -1, "", index, 0f, 0f, 0f, 0);
                }
            }
        }

        public static void DropNPCs(Vector2 pos, string NPCn, int amount = 1) //drop where tile breaks
        {
            for (int i = 0; i < amount; i++)
            {
                int pet = NPC.NewNPC((int)pos.X, (int)pos.Y, NPCDef.byName[NPCn].type, 0);
                Main.npc[pet].ai[0] = Main.player[Main.myPlayer].whoAmI;
                Main.npc[pet].netUpdate = true;
                if (Main.netMode == 2 && pet < 200)
                {
                    NetMessage.SendData(23, -1, -1, "", pet, 0f, 0f, 0f, 0);
                }
            }
        }
        public static NPC NPCatPOS(int x, int y, params string[] names)
        {
            //if (!Main.mouseOverNpc) return null;
            for (int azs0 = 0; azs0 < 200; azs0++)
            {
                if (Main.npc[azs0].active &&
                    Main.npc[azs0].position.X <= x &&
                    Main.npc[azs0].position.X + Main.npc[azs0].height >= x &&
                    Main.npc[azs0].position.Y <= y &&
                    Main.npc[azs0].position.Y + Main.npc[azs0].width >= y
                    )
                {
                    if (names.Length > 0)
                    {
                        foreach (string str in names)
                        {
                            if (Main.npc[azs0].type == NPCDef.byName[str].type)
                                return Main.npc[azs0];
                        }
                    }
                    else
                        return Main.npc[azs0];
                }
            }
            return null;
        }

        public static bool isNPCatPOS(int x, int y, string name = "")
        {
            return NPCatPOS(x, y, name) != null;
        }

        public static int SpawnedNPCs(string name)
        {
            int cou = 0;
            for (int azs0 = 0; azs0 < 200; azs0++)
            {// && Main.npc[azs0].ai[0] == Main.player[Main.myPlayer].whoAmI)
                if (Main.npc[azs0].active && Main.npc[azs0].type == NPCDef.byName[name].type)
                {
                    cou++;
                }
            }
            return cou;
        }

        public static Vector2 SpawnedPOSof(string name, int num)
        {
            for (int azs0 = 0; azs0 < 200; azs0++)
            {// && Main.npc[azs0].ai[0] == Main.player[Main.myPlayer].whoAmI)
                if (Main.npc[azs0].active && Main.npc[azs0].type == NPCDef.byName[name].type)
                {
                    num--;
                    if (num == 0) return Main.npc[azs0].position;
                }
            }
            return new Vector2();
        }

        public static bool MouseInNPCRange(Vector2 pos, int height, int width)
        {
            height *= 16;
            width *= 16;
            float mx = (Main.mouseX + Main.screenPosition.X);
            float my = (Main.mouseY + Main.screenPosition.Y);
            return (pos.X - width <= mx && pos.X + width >= mx &&
                    pos.Y - height <= my && pos.Y + height >= my);
        }

        public static bool NotSolid(Vector2 pos, int height, int width, bool NPCsCoords = false)
        {
            if (NPCsCoords)
            {
                pos.X /= 16;
                pos.Y /= 16;
            }
            for (int H = 0; H <= height; H++)
            {
                for (int W = 0; W <= width; W++)
                {
                    if (Main.tile[(int)pos.X + W, (int)pos.Y + W].collisionType > 0)
                      return false;
                }
            }
            return true;
        }
    }
}