using KatanaZERO.Items.FifteensBlade;
using KatanaZERO.Systems;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace KatanaZERO
{
    public class KatanaZERO : Mod
    {
        // Global variable
        public static bool enableLunge = true; // Default to true
        public static bool enableTimeShift = true;
        public static bool enableVectorKnockback = false;
        public string zeroKatanaSound;
        public string prismSwordSound;
        public string masterSwordSound;
        public string savantKnifeSound;
        public string claymoreSound;
        public string phoenixEdgeSound;
        public string fifteensKatanaSound;
        public string dashTrailStyle;

        public override void Load()
        {
            Settings config = GetInstance<Settings>();

            enableLunge = config.Lunge;
            enableTimeShift = config.TimeShift;
            enableVectorKnockback = config.VectorKnockback;

            zeroKatanaSound = config.ZeroKatanaSound;
            prismSwordSound = config.PrismSwordSound;
            masterSwordSound = config.MasterSwordSound;
            savantKnifeSound = config.SavantKnifeSound;
            claymoreSound = config.ClaymoreSound;
            phoenixEdgeSound = config.PhoenixEdgeSound;
            fifteensKatanaSound = config.FifteensBladeSound;
            dashTrailStyle = config.DashTrailStyle;
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