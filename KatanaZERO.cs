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
        // Global variable to control the lunge mechanic
        public static bool enableLunge = true; // Default to true

        public override void Load()
        {
            // Access the config and update the global variable based on the config setting
            Settings config = GetInstance<Settings>();

            // Set the global enableLunge flag based on config
            enableLunge = config.Lunge;
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