using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace KatanaZERO.Systems
{
    public class Settings : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.KatanaZERO.SwordSettingsHeader")]
        [LabelKey("$Mods.KatanaZERO.LungeLabel")]
        [TooltipKey("$Mods.KatanaZERO.LungeTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool Lunge = true;

        
        [LabelKey("$Mods.KatanaZERO.TimeShiftLabel")]
        [TooltipKey("$Mods.KatanaZERO.TimeShiftTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool TimeShift = true;

        [LabelKey("$Mods.KatanaZERO.VectorKnockback")]
        [TooltipKey("$Mods.KatanaZERO.VectorKnockbackTooltip")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool VectorKnockback = false;
    }
}
