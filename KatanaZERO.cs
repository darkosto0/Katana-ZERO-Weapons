using Terraria.ModLoader;
using Terraria;
using static Terraria.ModLoader.ModContent;
using KatanaZERO.Items.FifteensBlade;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using KatanaZERO.Systems;

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
}