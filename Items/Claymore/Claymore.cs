using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using KatanaZERO.Items.ZerosKatana;
using KatanaZERO.Systems;

namespace KatanaZERO.Items.Claymore
{
    public class Claymore : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/Claymore/claymore_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/Claymore/claymore_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/Claymore/claymore_slash3");

        private float attackCooldown = 0f;
        public bool hasAttacked = false;

        public override void SetDefaults()
        {
            Item.damage = 101;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 17, silver: 79);
            Item.UseSound = Slash1;
            Item.crit = 40;

            Item.useTime = 35;
            if (HasMod("FargowiltasSouls"))
            {
                Item.useAnimation = 35;
            }
            else
            {
                Item.useAnimation = 1;
            }

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;
            Item.knockBack = 4;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ClaymoreSlash>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BreakerBlade, 1)
                .AddIngredient(ItemID.SpectreBar, 10)
                .AddIngredient(ItemID.BeetleShell, 3)
                .AddIngredient(ItemID.EyeoftheGolem, 1)
                .AddTile(TileID.MythrilAnvil)
                .AddCustomShimmerResult(ItemID.BreakerBlade, 1)
                .AddCustomShimmerResult(ItemID.SpectreBar, 10)
                .AddCustomShimmerResult(ItemID.BeetleShell, 3)
                .AddCustomShimmerResult(ItemID.EyeoftheGolem, 1)
                .Register();
        }

        public override bool? UseItem(Player player)
        {

            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();

            player.direction = direction.X > 0 ? 1 : -1;

            if (KatanaZERO.enableLunge)
            {
                if (hasAttacked)
                {
                    player.velocity = direction * 5f;
                }
                else
                {
                    player.velocity = direction * 12f;
                    hasAttacked = true;
                }
            }

            attackCooldown = 35f; //artificial cooldown

            Random random = new Random();
            int randomNumber = random.Next(1, 4);
            switch (randomNumber)
            {
                case 1:
                    Item.UseSound = Slash1;
                    break;
                case 2:
                    Item.UseSound = Slash2;
                    break;
                case 3:
                    Item.UseSound = Slash3;
                    break;
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (attackCooldown > 0f)
            {
                attackCooldown -= 1f;
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (attackCooldown <= 0f) return true;
            else return false;
        }
        public override void UpdateInventory(Player player)
        {
            if (player.velocity.Y == 0f)
            {
                hasAttacked = false;
            }
        }
    } 
}

