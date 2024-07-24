using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using System;
using KatanaZERO.Items.ZerosKatana;
using static Terraria.ModLoader.ModLoader;


namespace KatanaZERO.Items.PrismSword
{
    public class PrismSword: ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/PrismSword/prism_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/PrismSword/prism_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/PrismSword/prism_slash3");

        private float attackCooldown = 0f;
        public bool hasAttacked = false;

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(gold: 5, silver: 10);
            Item.UseSound = Slash1;
            Item.crit = 15;


            Item.useTime = 20;
            if (HasMod("FargowiltasSouls"))
            {
                Item.useAnimation = 20;
            }
            else
            {
                Item.useAnimation = 1;
            }

            Item.useStyle = ItemUseStyleID.Swing;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;
            Item.holdStyle = 0;
            Item.knockBack = 3;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<PrismSwordSlash>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<ZeroKatana>())
                .AddIngredient(ItemID.RedDye, 1)
                .AddIngredient(ItemID.GreenDye, 1)
                .AddIngredient(ItemID.BlueDye, 1)
                .AddTile(TileID.Anvils)
                .AddCustomShimmerResult(ItemType<ZeroKatana>())
                .AddCustomShimmerResult(ItemID.RedDye, 1)
                .AddCustomShimmerResult(ItemID.GreenDye, 1)
                .AddCustomShimmerResult(ItemID.BlueDye, 1)
                .Register();
        }

        public override bool? UseItem(Player player)
        {

            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();

            player.direction = direction.X > 0 ? 1 : -1;

            if (hasAttacked)
            {
                player.velocity = direction * 4f;
            }
            else
            {
                player.velocity = direction * 8f;
                hasAttacked = true;
            }


            attackCooldown = 20f; //artificial cooldown

            System.Random random = new System.Random();
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

