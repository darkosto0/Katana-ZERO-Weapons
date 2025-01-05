using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
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
        public bool Lunge;

        [LabelKey("$Mods.KatanaZERO.VectorKnockback")]
        [TooltipKey("$Mods.KatanaZERO.VectorKnockbackTooltip")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool VectorKnockback;
        ///
        /// seperators
        ///
        [Header("$Mods.KatanaZERO.FifteensBladeHeader")]
        [LabelKey("$Mods.KatanaZERO.TimeShiftLabel")]
        [TooltipKey("$Mods.KatanaZERO.TimeShiftTooltip")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool TimeShift;

        [LabelKey("$Mods.KatanaZERO.DashTrailStyle")]
        [TooltipKey("$Mods.KatanaZERO.DashTrailStyleTooltip")]
        [OptionStrings(["Default", "Fireflies", "Disabled"])]
        [DefaultValue("Default")]
        [ReloadRequired]
        public string DashTrailStyle;
        /// 
        /// 
        /// 
        [Header("$Mods.KatanaZERO.SwordSoundEffects")]
        [LabelKey("$Mods.KatanaZERO.ZerosKatanaSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Zero's Katana")]
        [ReloadRequired]
        public string ZeroKatanaSound;

        [LabelKey("$Mods.KatanaZERO.PrismSwordSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Prism Sword")]
        [ReloadRequired]
        public string PrismSwordSound;

        [LabelKey("$Mods.KatanaZERO.MasterSwordSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Master Sword")]
        [ReloadRequired]
        public string MasterSwordSound;

        [LabelKey("$Mods.KatanaZERO.SavantKnifeSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Savant Knife")]
        [ReloadRequired]
        public string SavantKnifeSound;

        [LabelKey("$Mods.KatanaZERO.ClaymoreSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Claymore Prototype")]
        [ReloadRequired]
        public string ClaymoreSound;

        [LabelKey("$Mods.KatanaZERO.PhoenixEdgeSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Phoenix Edge")]
        [ReloadRequired]
        public string PhoenixEdgeSound;

        [LabelKey("$Mods.KatanaZERO.FifteensKatanaSound")]
        [TooltipKey("$Mods.KatanaZERO.SwordSoundTooltip")]
        [OptionStrings(["Zero's Katana", "Prism Sword", "Master Sword", "Savant Knife", "Claymore Prototype", "Phoenix Edge", "Dragon's Whisper"])]
        [DefaultValue("Dragon's Whisper")]
        [ReloadRequired]
        public string FifteensBladeSound;

        [Header("$Mods.KatanaZERO.DeveloperNoteHeader")]
        [LabelKey("$Mods.KatanaZERO.DeveloperNote")]
        [TooltipKey("$Mods.KatanaZERO.DeveloperNoteTooltip")]
        public bool placeholder;
    }
}
