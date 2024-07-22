using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModLoader;

namespace KatanaZERO.Items.MasterSword
{
    public class MasterSword : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_slash_01");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_slash_02");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_slash_03");

        private float attackCooldown;
        public bool hasAttacked = false;

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 15, silver: 57);
            Item.UseSound = Slash1;
            Item.crit = 20;

            Item.useTime = 30;
            if (HasMod("FargowiltasSouls"))
            {
                Item.useAnimation = 30;
            }
            else
            {
                Item.useAnimation = 1;
            }

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 6;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MasterSwordSlash>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword, 1)
                .AddIngredient(ItemID.BluePhasesaber, 1)
                .AddIngredient(ItemID.Sapphire, 10)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddTile(TileID.MythrilAnvil)
                .AddCustomShimmerResult(ItemID.EnchantedSword, 1)
                .AddCustomShimmerResult(ItemID.BluePhasesaber, 1)
                .AddCustomShimmerResult(ItemID.Sapphire, 10)
                .AddCustomShimmerResult(ItemID.SoulofMight, 5)
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
                player.velocity = direction * 10f;
                hasAttacked = true;
            }


            attackCooldown = 30f; //artificial cooldown

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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ModContent.ProjectileType<MasterSwordBeam>();
            Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position, velocity, type, damage, knockback, player.whoAmI);
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

