using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace KatanaZERO.Systems
{
    public class Settings : ModConfig
    {
        public override void OnChanged()
        {
            KatanaZERO.SyncConfig(this);
        }

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("$Mods.KatanaZERO.SwordSettingsHeader")]
        [LabelKey("$Mods.KatanaZERO.LungeLabel")]
        [TooltipKey("$Mods.KatanaZERO.LungeTooltip")]
        [DefaultValue(true)]
        public bool Lunge;

        [LabelKey("$Mods.KatanaZERO.VectorKnockback")]
        [TooltipKey("$Mods.KatanaZERO.VectorKnockbackTooltip")]
        [DefaultValue(false)]
        public bool VectorKnockback;

        [LabelKey("$Mods.KatanaZERO.ProgressionDamage")]
        [TooltipKey("$Mods.KatanaZERO.ProgressionDamageTooltip")]
        [DefaultValue(false)]
        public bool ProgressionDamage;
        ///
        /// seperators
        ///
        [Header("$Mods.KatanaZERO.FifteensBladeHeader")]
        [LabelKey("$Mods.KatanaZERO.TimeShiftLabel")]
        [TooltipKey("$Mods.KatanaZERO.TimeShiftTooltip")]
        [DefaultValue(true)]
        public bool TimeShift;

        [LabelKey("$Mods.KatanaZERO.DashTrailStyle")]
        [TooltipKey("$Mods.KatanaZERO.DashTrailStyleTooltip")]
        [OptionStrings(["Default", "Fireflies", "Disabled"])]
        [DefaultValue("Default")]
        public string DashTrailStyle;

        [LabelKey("$Mods.KatanaZERO.ToggleDropLabel")]
        [TooltipKey("$Mods.KatanaZERO.ToggleDropTooltip")]
        [DefaultValue(true)]
        public bool ToggleDrop;
        /// 
        /// 
        /// 
        [Header("$Mods.KatanaZERO.SwordSoundEffects")]
        [LabelKey("$Mods.KatanaZERO.ZerosKatanaSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Zero's Katana")]
        public string ZeroKatanaSound;

        [LabelKey("$Mods.KatanaZERO.PrismSwordSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Prism Sword")]
        public string PrismSwordSound;

        [LabelKey("$Mods.KatanaZERO.MasterSwordSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Master Sword")]
        public string MasterSwordSound;

        [LabelKey("$Mods.KatanaZERO.SavantKnifeSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Savant Knife")]
        public string SavantKnifeSound;

        [LabelKey("$Mods.KatanaZERO.ClaymoreSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Claymore Prototype")]
        public string ClaymoreSound;

        [LabelKey("$Mods.KatanaZERO.PhoenixEdgeSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Phoenix Edge")]
        public string PhoenixEdgeSound;

        [LabelKey("$Mods.KatanaZERO.FifteensKatanaSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Dragon's Whisper")]
        public string FifteensBladeSound;
    }
}
