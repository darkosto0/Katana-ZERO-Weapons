using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using System;
using KatanaZERO.Items.ZerosKatana;
using static Terraria.ModLoader.ModLoader;
using KatanaZERO.Systems;


namespace KatanaZERO.Items.SavantKnife
{
    public class SavantKnife : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/SavantKnife/savant_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/SavantKnife/savant_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/SavantKnife/savant_slash3");

        private float attackCooldown = 0f;
        public bool hasAttacked = false;

        public override void SetDefaults()
        {
            Item.damage = 76;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10, silver: 24);
            Item.UseSound = Slash1;
            Item.crit = 30;


            Item.useTime = 10;
            if (HasMod("FargowiltasSouls"))
            {
                Item.useAnimation = 10;
            }
            else
            {
                Item.useAnimation = 1;
            }

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;

            Item.knockBack = 3;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SavantSlash>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<ZeroKatana>())
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ItemID.SoulofMight, 3)
                .AddIngredient(ItemID.SoulofSight, 3)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddTile(TileID.SharpeningStation)
                .AddCustomShimmerResult(ItemType<ZeroKatana>())
                .AddCustomShimmerResult(ItemID.ChlorophyteBar, 10)
                .AddCustomShimmerResult(ItemID.SoulofMight, 3)
                .AddCustomShimmerResult(ItemID.SoulofSight, 3)
                .AddCustomShimmerResult(ItemID.SoulofFright, 3)
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
                    player.velocity = direction * 2.2f;
                }
                else
                {
                    player.velocity = direction * 8f;
                    hasAttacked = true;
                }
            }

            attackCooldown = 10f; //artificial cooldown

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
            return attackCooldown <= 0f;
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

