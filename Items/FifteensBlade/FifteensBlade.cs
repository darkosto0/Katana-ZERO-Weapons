using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using KatanaZERO.Dusts;

namespace KatanaZERO.Items.FifteensBlade
{
    public class FifteensBlade : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_slash3");
        public static readonly SoundStyle Special1 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special1");
        public static readonly SoundStyle Special2 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special2");

        private float attackCooldown = 0f;
        private float dragonCooldown = 0f;
        public bool hasAttacked = false;
        public bool hasRightClicked = false;
        public bool hasReleasedRightClick = false;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 620;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(gold: 99, silver: 99);
            Item.UseSound = Slash1;
            Item.crit = 50;

            Item.useTime = 20;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;
            Item.knockBack = 6;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<FifteensSlash>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod, "ZerosKatana");
            recipe.AddIngredient(ItemID.FragmentSolar, 20);
            recipe.AddIngredient(ItemID.FragmentStardust, 20);
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddIngredient(ItemID.FragmentNebula, 20);
            recipe.AddIngredient(ItemID.LunarOre, 50);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.AddCustomShimmerResult(ItemID.Katana, 1);
            recipe.AddCustomShimmerResult(ItemID.SilverBar, 5);
            recipe.AddCustomShimmerResult(ItemID.FragmentSolar, 20);
            recipe.AddCustomShimmerResult(ItemID.FragmentStardust, 20);
            recipe.AddCustomShimmerResult(ItemID.FragmentVortex, 20);
            recipe.AddCustomShimmerResult(ItemID.FragmentNebula, 20);
            recipe.AddCustomShimmerResult(ItemID.LunarOre, 50);
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

            attackCooldown = 20f; //artificial cooldown

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
            if (dragonCooldown > 0f)
            {
                dragonCooldown -= 1f;
            }

            if (dragonCooldown <= 0f)
            {
                if (Main.mouseRight)
                {
                    hasRightClicked = true;
                }
                if (Main.mouseRight == false && hasRightClicked == true)
                {
                    DragonDash(player);
                    hasRightClicked = false;
                    dragonCooldown = 180;
                }
                CreateAllDust(player); //create the big circle and 
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (player.velocity.Y == 0f)
            {
                hasAttacked = false;
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.mouseRight == false && attackCooldown <= 0f && dragonCooldown <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public bool CreateAllDust(Player player) //i used AI for all the math since thats my weakpoint, im just a skid i guess
        {
            if (Main.mouseRight)
            {
                int dustAmount = 800; // Total number of dust particles in the circle
                float radius = 464f; // Radius for the circle
                int dustType = ModContent.DustType<FifteenDust>(); // Dust type

                for (int i = 0; i < dustAmount; i++) //create circle
                {
                    float angle = MathHelper.TwoPi / dustAmount * i;
                    Vector2 position = player.Center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Dust dustCircle = Dust.NewDustPerfect(position, dustType);
                    dustCircle.noGravity = true;
                    dustCircle.noLight = true;
                }

                float distanceToCursor = Vector2.Distance(player.Center, Main.MouseWorld);
                float maxLerpAmount = Math.Min(1f, radius / distanceToCursor);

                for (float lerpAmount = 0; lerpAmount <= maxLerpAmount; lerpAmount += 0.004f) // Use smaller increments for smoother trajectory
                {
                    Vector2 barPosition = Vector2.Lerp(player.Center, Main.MouseWorld, lerpAmount);
                    if (Collision.SolidCollision(barPosition, 10, 10))
                    {
                        break; // Stop drawing when collision occurs
                    }

                    Dust dust = Dust.NewDustPerfect(barPosition, dustType);
                    dust.noGravity = true;
                }

                Vector2 endPosition = Vector2.Lerp(player.Center, Main.MouseWorld, maxLerpAmount);
                int dustAmount1 = 10;
                // Create a larger circle centered at the end position
                float largerRadius = 3.3f; // Adjust the size as needed
                for (int i = 0; i < dustAmount1; i++)
                {
                    float angle = MathHelper.TwoPi / dustAmount1 * i;
                    Vector2 position = endPosition + largerRadius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    if (Collision.SolidCollision(position, 1, 1))
                    {
                        break; // Stop drawing when collision occurs
                    }
                    Dust dustLargerCircle = Dust.NewDustPerfect(position, dustType);
                    dustLargerCircle.noGravity = true;
                    dustLargerCircle.noLight = true;
                }
                player.velocity = Vector2.Zero;
            }
            return true; //return true to make the dust spawn 
        }

        public bool DragonDash(Player player) //same thing here, i couldnt do all the math myself even if i tried
        {
            Vector2 cursorPosition = Main.MouseWorld;
            Vector2 playerPosition = player.Center;

            float maxRadius = 464f; // 29 blocks

            float distanceToCursor = Vector2.Distance(playerPosition, cursorPosition);

            if (distanceToCursor > maxRadius)  // If the distance to the cursor is greater than the max radius, adjust the cursor position
            {
                Vector2 direction = Vector2.Normalize(cursorPosition - playerPosition);
                cursorPosition = playerPosition + direction * maxRadius;
                distanceToCursor = maxRadius; // Update the distance to the new cursor position
            }

            Vector2 midPoint = (playerPosition + cursorPosition) / 2; // Calculate the mid-point between player and cursor

            int hitboxWidth = player.width * 2; // Define hitbox dimension
            int hitboxHeight = player.height * 2;

            // Create hitboxes
            Rectangle playerHitbox = new Rectangle((int)playerPosition.X - hitboxWidth / 2, (int)playerPosition.Y - hitboxHeight / 2, hitboxWidth, hitboxHeight);
            Rectangle cursorHitbox = new Rectangle((int)cursorPosition.X - hitboxWidth / 2, (int)cursorPosition.Y - hitboxHeight / 2, hitboxWidth, hitboxHeight);
            Rectangle midHitbox = new Rectangle((int)midPoint.X - hitboxWidth / 2, (int)midPoint.Y - hitboxHeight / 2, hitboxWidth, hitboxHeight);

            // Calculate distances for additional hitboxes
            float distanceToMid = Vector2.Distance(playerPosition, midPoint);
            float distanceFromMidToCursor = Vector2.Distance(midPoint, cursorPosition);

            // Create additional hitboxes
            Rectangle playerToMidHitbox = new Rectangle((int)playerPosition.X - hitboxWidth / 2, (int)(playerPosition.Y - distanceToMid / 2) - hitboxHeight / 2, hitboxWidth, (int)distanceToMid);
            Rectangle midToCursorHitbox = new Rectangle((int)midPoint.X - hitboxWidth / 2, (int)(midPoint.Y - distanceFromMidToCursor / 2) - hitboxHeight / 2, hitboxWidth, (int)distanceFromMidToCursor);

            player.Teleport(cursorPosition - new Vector2(player.width / 2, player.height / 2));

            Random random = new Random();
            int randomNumber = random.Next(1, 3);
            switch (randomNumber)
            {
                case 1:
                    SoundEngine.PlaySound(Special1);
                    break;
                case 2:
                    SoundEngine.PlaySound(Special2);
                    break;
            }

            foreach (NPC enemy in Main.npc) // i am sorry for your eyes for what youre about to see, im desperate to get his working.
            {
                if (!enemy.friendly && !enemy.boss && enemy.Hitbox.Intersects(cursorHitbox))
                {
                    enemy.SimpleStrikeNPC(Item.damage * 3, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                else if (!enemy.friendly && enemy.boss && enemy.Hitbox.Intersects(cursorHitbox))
                {
                    enemy.SimpleStrikeNPC(enemy.lifeMax / 5, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                /////// This can definitely be simplified some way, ill try to find a way. (you cant use the || operator to check for any hitbox mind you)
                if (!enemy.friendly && !enemy.boss && enemy.Hitbox.Intersects(playerHitbox))
                {
                    enemy.SimpleStrikeNPC(Item.damage * 3, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                else if (!enemy.friendly && enemy.boss && enemy.Hitbox.Intersects(playerHitbox))
                {
                    enemy.SimpleStrikeNPC(enemy.lifeMax / 5, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                ///////
                if (!enemy.friendly && !enemy.boss && enemy.Hitbox.Intersects(midHitbox))
                {
                    enemy.SimpleStrikeNPC(Item.damage * 3, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                else if (!enemy.friendly && enemy.boss && enemy.Hitbox.Intersects(midHitbox))
                {
                    enemy.SimpleStrikeNPC(enemy.lifeMax / 5, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                ///////
                if (!enemy.friendly && !enemy.boss && enemy.Hitbox.Intersects(playerToMidHitbox))
                {
                    enemy.SimpleStrikeNPC(Item.damage * 2, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                else if (!enemy.friendly && enemy.boss && enemy.Hitbox.Intersects(playerToMidHitbox))
                {
                    enemy.SimpleStrikeNPC(enemy.lifeMax / 5, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                ///////
                if (!enemy.friendly && !enemy.boss && enemy.Hitbox.Intersects(midToCursorHitbox))
                {
                    enemy.SimpleStrikeNPC(Item.damage * 2, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
                else if (!enemy.friendly && enemy.boss && enemy.Hitbox.Intersects(midToCursorHitbox))
                {
                    enemy.SimpleStrikeNPC(enemy.lifeMax / 5, 0, true, player.direction * 2f, DamageClass.Melee, true, 0, false);
                    player.immune = true;
                    player.immuneTime = 60;
                }
            }
            return true;
        }
    }
}