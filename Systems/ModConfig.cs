using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace KatanaZERO.Systems
{
    public class Settings : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.KatanaZERO.SwordSettingsHeader")]
        [DefaultValue(true)]
        [LabelKey("$Mods.KatanaZERO.LungeLabel")]
        [TooltipKey("$Mods.KatanaZERO.LungeTooltip")]
        [ReloadRequired]
        public bool Lunge = true;
    }
}
