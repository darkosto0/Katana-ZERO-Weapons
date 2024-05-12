using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace KatanaZERO.Items.PhoenixEdge
{
    public class Phoenix : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/PhoenixEdge/phoenix_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/PhoenixEdge/phoenix_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/PhoenixEdge/phoenix_slash3");

        private float attackCooldown = 0f;
        public bool hasAttacked = false;
        private const float DistanceFromCore = 66f;

        public override void SetDefaults()
        {
            Item.damage = 179;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Quest;
            Item.value = Item.sellPrice(gold: 29, silver: 79);
            Item.UseSound = Slash1;
            Item.crit = 70;

            Item.useTime = 20;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;
            Item.knockBack = 6;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PhoenixSlash>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod, "ZerosKatana");
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddIngredient(ItemID.LivingFireBlock, 5);
            recipe.AddIngredient(ItemID.CursedFlame, 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool? UseItem(Player player)
        {

            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();

            player.direction = direction.X > 0 ? 1 : -1;

            if (hasAttacked)
            {
                player.velocity = direction * 5f;
            }
            else
            {
                player.velocity = direction * 12f;
                hasAttacked = true;
            }


            attackCooldown = 35f; //artificial cooldown

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

        /* public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
         {
             int numProjectiles = 4; // This is the number of projectiles you want to shoot
             float spread = MathHelper.ToRadians(45); // 45 degrees in radians for the spread
             float baseSpeed = velocity.Length();
             float baseAngle = velocity.ToRotation();


             for (int i = 0; i < numProjectiles; i++)
             {
                 type = ModContent.ProjectileType<PhoenixFlames>();
                 float variation = Main.rand.NextFloat(-spread, spread);
                 Vector2 perturbedSpeed = velocity.RotatedBy(variation);

                 Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position, perturbedSpeed, type, damage, knockback, player.whoAmI);
             }

             return true;
         } */

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int NumProjectiles = Main.rand.Next(3, 6); // Number of projectiles to shoot
            float angleToCursor = (float)Math.Atan2(Main.MouseWorld.Y - player.Center.Y, Main.MouseWorld.X - player.Center.X);
            Vector2 offset = new Vector2(DistanceFromCore, 0f).RotatedBy(angleToCursor);
            position = player.Center + offset;

            for (int i = 0; i < NumProjectiles; i++)
            {
                float speed = Main.rand.NextFloat(9f, 13f); // Randomize speed within a range
                float spread = MathHelper.ToRadians(7); // 10 degrees of spread
                float baseAngle = angleToCursor + Main.rand.NextFloat(-spread, spread); // Randomize angle within the spread
                velocity = new Vector2((float)Math.Cos(baseAngle), (float)Math.Sin(baseAngle)) * speed;

                type = ModContent.ProjectileType<PhoenixFlames>(); // Uncomment and use your own projectile type if needed
                //type = ProjectileID.DD2FlameBurstTowerT1Shot; // Example projectile type

                Projectile.NewProjectileDirect(
                    source,
                    position,
                    velocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );
            }

            return true;
        }

    }
}

