using Terraria.ModLoader;
using Terraria;
using static Terraria.ModLoader.ModContent;
using KatanaZERO.Items.FifteensBlade;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using KatanaZERO.Systems;
using System;
using Microsoft.Xna.Framework;

namespace KatanaZERO
{
    public class KatanaZERO : Mod
    {
        // Global variable
        public static bool enableLunge = true; // Default to true
        public static bool enableTimeShift = true;
        public static bool enableVectorKnockback = false;
        public override void Load()
        {
            Settings config = GetInstance<Settings>();

            enableLunge = config.Lunge;
            enableTimeShift = config.TimeShift;
            enableVectorKnockback = config.VectorKnockback;
        }
    }

    public class MyGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!npc.friendly)
            {
                npcLoot.Add(ItemDropRule.Common(ItemType<FifteensBlade>(), 40000, 1, 1));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ItemType<FifteensBlade>(), 9, 1, 1));
            }
        }
    }

    public static class ColorHelper
    {
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("#", "");

            byte r = (byte)(Convert.ToInt32(hex.Substring(0, 2), 16)); // Red
            byte g = (byte)(Convert.ToInt32(hex.Substring(2, 2), 16)); // Green
            byte b = (byte)(Convert.ToInt32(hex.Substring(4, 2), 16)); // Blue

            return new Color(r, g, b);
        }
    }

}