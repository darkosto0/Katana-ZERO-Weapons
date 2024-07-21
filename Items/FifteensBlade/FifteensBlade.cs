using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using KatanaZERO.Dusts;
using System;
using Player = Terraria.Player;
using System.IO;
using System.Collections.Generic;
using static Terraria.ModLoader.ModLoader;
using Microsoft.Build.Experimental.ProjectCache;

namespace KatanaZERO.Items.FifteensBlade
{
    public class FifteensBlade : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_slash3");

        public static readonly SoundStyle Special1 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special1");
        public static readonly SoundStyle Special2 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special2");
        public static readonly SoundStyle Special3 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special3");

        public static readonly SoundStyle SlowEngage = new SoundStyle("KatanaZERO/Sounds/slomo_engage");
        public static readonly SoundStyle SlowDisengage = new SoundStyle("KatanaZERO/Sounds/slomo_disengage");


        public float attackCooldown = 0f;
        public float dragonCooldown = 0f;
        public int SlowMoCounter = 0;
        public int PowerLevel;

        public bool hasAttacked = false;
        public bool hasRightClicked = false;
        public bool hasReleasedRightClick = false;
        public bool rightClickActivated = false; //i needed another right click bool (i think)
        public bool playedSlomoEngage = false;


        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(gold: 99, silver: 99);
            Item.UseSound = Slash1;
            Item.crit = 0;

            Item.useTime = 20;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<FifteensSlash>();
        }

        /*
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<ZeroKatana>())
                .AddIngredient(ItemID.FragmentSolar, 20)
                .AddIngredient(ItemID.FragmentStardust, 20)
                .AddIngredient(ItemID.FragmentVortex, 20)
                .AddIngredient(ItemID.FragmentNebula, 20)
                .AddIngredient(ItemID.LunarOre, 50)
                .AddTile(TileID.LunarCraftingStation)
                .AddCustomShimmerResult(ItemType<ZeroKatana>())
                .AddCustomShimmerResult(ItemID.FragmentSolar, 20)
                .AddCustomShimmerResult(ItemID.FragmentStardust, 20)
                .AddCustomShimmerResult(ItemID.FragmentVortex, 20)
                .AddCustomShimmerResult(ItemID.FragmentNebula, 20)
                .AddCustomShimmerResult(ItemID.LunarOre, 50)
                .Register();
        }
        */

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
                CreateAllDust(player); //create the big circle and the trajectory
                SlowDown(player);
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (player.velocity.Y == 0f)
            {
                hasAttacked = false;
            }

            if (attackCooldown > 0f)
            {
                attackCooldown -= 1f;
            }

            if (dragonCooldown > 0f)
            {
                dragonCooldown -= 1f;
            }
            StatChange();
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

        public static void CreateAllDust(Player player)
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
        }

        public bool DragonDash(Player player)
        {
            if (!Main.myPlayer.Equals(player.whoAmI))
                return false;

            Vector2 cursorPosition = Main.MouseWorld;
            Vector2 playerPosition = player.Center;

            float maxRadius = 464f;
            float distanceToCursor = Vector2.Distance(playerPosition, cursorPosition);

            if (distanceToCursor > maxRadius)
            {
                Vector2 direction = Vector2.Normalize(cursorPosition - playerPosition);
                cursorPosition = playerPosition + direction * maxRadius;
                distanceToCursor = maxRadius;
            }

            Vector2 directionToCursor = Vector2.Normalize(cursorPosition - playerPosition);
            float moveDistance = distanceToCursor;

            float dashSpeed = 520f;
            float dashTime = moveDistance / dashSpeed;
            float dashTicks = dashTime * 60;

            player.immune = true;
            player.immuneTime = (int)dashTicks;

            int hitboxWidth = player.width * 3;
            int hitboxHeight = player.height * 3;

            Vector2 dashStep = directionToCursor * (dashSpeed / 60f);

            Dictionary<int, bool> hitEnemies = new Dictionary<int, bool>();

            for (int i = 0; i < dashTicks; i++)
            {
                Vector2 nextPosition = player.position + dashStep;
                if (Collision.SolidCollision(nextPosition, player.width, player.height))
                {
                    break;
                }

                player.position = nextPosition;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                }

                Rectangle playerHitbox = new Rectangle((int)player.position.X - hitboxWidth / 2, (int)player.position.Y - hitboxHeight / 2, hitboxWidth, hitboxHeight);

                foreach (NPC enemy in Main.npc)
                {
                    if (!enemy.friendly && enemy.Hitbox.Intersects(playerHitbox))
                    {
                        if (!hitEnemies.ContainsKey(enemy.whoAmI))
                        {
                            hitEnemies[enemy.whoAmI] = true;

                            if (!enemy.boss)
                            {
                                enemy.SimpleStrikeNPC(Item.damage * 5, 0, false, 0, DamageClass.Melee, true, 0, false);
                            }
                            else
                            {
                                double FivePercentDamage = enemy.lifeMax * 0.025;
                                enemy.SimpleStrikeNPC((Item.damage * 5) + (int)FivePercentDamage, 0, true, 0, DamageClass.Melee, false, 0, false);
                            }
                        }
                    }
                }
            }

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

            player.velocity = Vector2.Zero;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
            }
            return true;
        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
            hasAttacked = reader.ReadBoolean();
            attackCooldown = reader.ReadSingle();
            dragonCooldown = reader.ReadSingle();
            hasRightClicked = reader.ReadBoolean();
            hasReleasedRightClick = reader.ReadBoolean();
        }

        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
            writer.Write(hasAttacked);
            writer.Write(attackCooldown);
            writer.Write(dragonCooldown);
            writer.Write(hasRightClicked);
            writer.Write(hasReleasedRightClick);
        }
    

    public bool SlowDown(Player player)
        {
            if (Main.mouseRight)
            {
                if (playedSlomoEngage == false)
                {
                    SoundEngine.PlaySound(SlowEngage);
                    playedSlomoEngage = true;
                }

                foreach (NPC mob in Main.npc)
                {
                    if (mob.Distance(player.Center) < 2000f && mob.active && !mob.friendly)
                    {
                        mob.velocity /= 1.05f;
                    }
                }

                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.Distance(player.Center) < 2000f && projectile.active && !projectile.friendly)
                    {
                        projectile.velocity /= 1.05f;
                    }
                }
                SlowMoCounter++;
                rightClickActivated = true;
            }
            else if (Main.mouseRight == false && rightClickActivated == true) //right click released
            {
                SoundEngine.PlaySound(SlowDisengage);

                // Slowed down mobs do NOT need speed changes, their AI does that for them.
                // Projectiles though, don't most of the time.
                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.Distance(player.Center) < 2000f && projectile.active && !projectile.friendly)
                    {
                        projectile.velocity *= (SlowMoCounter * 1.05f);
                    }
                }
                playedSlomoEngage = false;
                rightClickActivated = false;
                SlowMoCounter = 0;
            }
            return true;
        }

        public void StatChange() // lord forgive me for i have pulled a yandere simulator - Darkosto // So gosh darn true.. - Sequile
        {
            
            ModLoader.TryGetMod("CalamityMod", out Mod Calamity);
            ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod);

            // ONLY CALAMITY
            if (HasMod("CalamityMod") && !HasMod("ThoriumMod"))
            {
                if ((bool)Calamity.Call("Downed", "bossrush")) { PowerLevel = 55; } // 55
                else if ((bool)Calamity.Call("Downed", "calamitas")) { PowerLevel = 54; } // 54
                else if ((bool)Calamity.Call("Downed", "exomechs")) { PowerLevel = 53; } // 53
                else if ((bool)Calamity.Call("Downed", "yharon")) { PowerLevel = 52; } // 52
                else if ((bool)Calamity.Call("Downed", "devourerofgods")) { PowerLevel = 51; } // 51
                else if ((bool)Calamity.Call("Downed", "oldduke")) { PowerLevel = 50; } // 50
                else if ((bool)Calamity.Call("Downed", "polterghast")) { PowerLevel = 49; } // 49
                else if ((bool)Calamity.Call("Downed", "signus")) { PowerLevel = 48; } // 48
                else if ((bool)Calamity.Call("Downed", "stormweaver")) { PowerLevel = 47; } // 47
                else if ((bool)Calamity.Call("Downed", "ceaselessvoid")) { PowerLevel = 46; } // 46
                else if ((bool)Calamity.Call("Downed", "providence")) { PowerLevel = 44; } // 44
                else if ((bool)Calamity.Call("Downed", "dragonfolly")) { PowerLevel = 43; } // 43
                else if ((bool)Calamity.Call("Downed", "guardians")) { PowerLevel = 42; } // 42
                else if (NPC.downedMoonlord == true) { PowerLevel = 41; } // 41
                else if ((bool)Calamity.Call("Downed", "astrumdeus")) { PowerLevel = 40; } // 40
                else if (NPC.downedAncientCultist == true) { PowerLevel = 39; } // 39
                else if ((bool)Calamity.Call("Downed", "ravager")) { PowerLevel = 38; } // 38
                // Betsy skip (downedBetsy doesnt exist)
                else if (NPC.downedEmpressOfLight == true) { PowerLevel = 36; } // 36
                else if ((bool)Calamity.Call("Downed", "plaguebringergoliath")) { PowerLevel = 35; } // 35
                else if (NPC.downedFishron == true) { PowerLevel = 34; } // 34
                else if (NPC.downedGolemBoss == true) { PowerLevel = 32; } // 32
                else if ((bool)Calamity.Call("Downed", "astrumaureus")) { PowerLevel = 31; } // 31
                else if ((bool)Calamity.Call("Downed", "anahitaleviathan")) { PowerLevel = 30; } // 30
                else if (NPC.downedPlantBoss == true) { PowerLevel = 29; } // 29
                else if ((bool)Calamity.Call("Downed", "calamitasclone")) { PowerLevel = 28; } // 28
                else if (NPC.downedMechBoss3 == true) { PowerLevel = 26; } // 26
                else if ((bool)Calamity.Call("Downed", "brimstoneelemental")) { PowerLevel = 25; } // 25
                else if (NPC.downedMechBoss2 == true) { PowerLevel = 24; } // 24
                else if ((bool)Calamity.Call("Downed", "aquaticscourge")) { PowerLevel = 23; } // 23
                else if (NPC.downedMechBoss1 == true) { PowerLevel = 22; } // 22
                else if ((bool)Calamity.Call("Downed", "cryogen")) { PowerLevel = 21; } // 21
                else if (NPC.downedQueenSlime == true) { PowerLevel = 20; } // 20
                else if (Main.hardMode == true) { PowerLevel = 17; } // 17
                else if ((bool)Calamity.Call("Downed", "slimegod")) { PowerLevel = 15; } // 15
                else if (NPC.downedDeerclops == true) { PowerLevel = 12; } // 12
                else if (NPC.downedBoss3 == true) { PowerLevel = 11; } // 11
                else if (NPC.downedQueenBee == true) { PowerLevel = 10; } // 10
                else if ((bool)Calamity.Call("Downed", "hivemind") || ((bool)Calamity.Call("Downed", "perforator"))) { PowerLevel = 9; } // 9
                else if (NPC.downedBoss2 == true) { PowerLevel = 6; } // 6
                else if ((bool)Calamity.Call("Downed", "crabulon")) { PowerLevel = 5; } // 5
                else if (NPC.downedBoss1 == true) { PowerLevel = 4; } // 4
                else if ((bool)Calamity.Call("Downed", "desertscourge")) { PowerLevel = 3; } // 3
                else if (NPC.downedSlimeKing == true) { PowerLevel = 2; } // 2
            }

            // NEITHER
            if (!HasMod("CalamityMod") && !HasMod("ThoriumMod"))
            {
                if (NPC.downedMoonlord == true) { PowerLevel = 41; } // 41
                else if (NPC.downedAncientCultist == true) { PowerLevel = 39; } // 39
                // Betsy skip (downedBetsy doesnt exist)
                else if (NPC.downedEmpressOfLight == true) { PowerLevel = 36; } // 36
                else if (NPC.downedFishron == true) { PowerLevel = 34; } // 34
                else if (NPC.downedGolemBoss == true) { PowerLevel = 32; } // 32
                else if (NPC.downedPlantBoss == true) { PowerLevel = 29; } // 29
                else if (NPC.downedMechBoss3 == true) { PowerLevel = 26; } // 26
                else if (NPC.downedMechBoss2 == true) { PowerLevel = 24; } // 24
                else if (NPC.downedMechBoss1 == true) { PowerLevel = 22; } // 22
                else if (NPC.downedQueenSlime == true) { PowerLevel = 20; } // 20
                else if (Main.hardMode == true) { PowerLevel = 17; } // 17
                else if (NPC.downedDeerclops == true) { PowerLevel = 12; } // 12
                else if (NPC.downedBoss3 == true) { PowerLevel = 11; } // 11
                else if (NPC.downedQueenBee == true) { PowerLevel = 10; } // 10
                else if (NPC.downedBoss2 == true) { PowerLevel = 6; } // 6
                else if (NPC.downedBoss1 == true) { PowerLevel = 4; } // 4
                else if (NPC.downedSlimeKing == true) { PowerLevel = 2; } // 2
            }

            // ONLY THORIUM
            if (!HasMod("CalamityMod") && HasMod("ThoriumMod"))
            {
                     if ((bool)thoriumMod.Call("GetDownedBoss", "ThePrimordials")) { PowerLevel = 45; } // 45
                else if (NPC.downedMoonlord == true) { PowerLevel = 41; } // 41
                else if (NPC.downedAncientCultist == true) { PowerLevel = 39; } // 39
                // Betsy skip (downedBetsy doesnt exist)
                else if (NPC.downedEmpressOfLight == true) { PowerLevel = 36; } // 36
                else if (NPC.downedFishron == true) { PowerLevel = 34; } // 34
                else if ((bool)thoriumMod.Call("GetDownedBoss", "ForgottenOne")) { PowerLevel = 33; } // 33
                else if (NPC.downedGolemBoss == true) { PowerLevel = 32; } // 32
                else if (NPC.downedPlantBoss == true) { PowerLevel = 29; } // 29
                else if ((bool)thoriumMod.Call("GetDownedBoss", "Lich")) { PowerLevel = 27; } // 27
                else if (NPC.downedMechBoss3 == true) { PowerLevel = 26; } // 26
                else if (NPC.downedMechBoss2 == true) { PowerLevel = 24; } // 24
                else if (NPC.downedMechBoss1 == true) { PowerLevel = 22; } // 22
                else if (NPC.downedQueenSlime == true) { PowerLevel = 20; } // 20
                else if ((bool)thoriumMod.Call("GetDownedBoss", "FallenBeholder")) { PowerLevel = 19; } // 19
                else if ((bool)thoriumMod.Call("GetDownedBoss", "BoreanStrider")) { PowerLevel = 18; } // 18
                else if (Main.hardMode == true) { PowerLevel = 17; } // 17
                else if ((bool)thoriumMod.Call("GetDownedBoss", "StarScouter")) { PowerLevel = 16; } // 16
                else if ((bool)thoriumMod.Call("GetDownedBoss", "BuriedChampion")) { PowerLevel = 14; } // 14
                else if ((bool)thoriumMod.Call("GetDownedBoss", "GraniteEnergyStorm")) { PowerLevel = 13; } // 
                else if (NPC.downedDeerclops == true) { PowerLevel = 12; } // 12
                else if (NPC.downedBoss3 == true) { PowerLevel = 11; } // 11
                else if (NPC.downedQueenBee == true) { PowerLevel = 10; } // 10
                else if ((bool)thoriumMod.Call("GetDownedBoss", "Viscount")) { PowerLevel = 8; } // 8
                else if ((bool)thoriumMod.Call("GetDownedBoss", "QueenJellyfish")) { PowerLevel = 7; } // 7
                else if (NPC.downedBoss2 == true) { PowerLevel = 6; } // 6
                else if (NPC.downedBoss1 == true) { PowerLevel = 4; } // 4
                else if (NPC.downedSlimeKing == true) { PowerLevel = 2; } // 2
                else if ((bool)thoriumMod.Call("GetDownedBoss", "TheGrandThunderBird")) { PowerLevel = 1; } // 1

            }

            //  BOTH MODS ACTIVE
            if (HasMod("CalamityMod") && HasMod("ThoriumMod"))
            {
                if ((bool)Calamity.Call("Downed", "bossrush")) { PowerLevel = 55; } // 55
                else if ((bool)Calamity.Call("Downed", "calamitas")) { PowerLevel = 54; } // 54
                else if ((bool)Calamity.Call("Downed", "exomechs")) { PowerLevel = 53; } // 53
                else if ((bool)Calamity.Call("Downed", "yharon")) { PowerLevel = 52; } // 52
                else if ((bool)Calamity.Call("Downed", "devourerofgods")) { PowerLevel = 51; } // 51
                else if ((bool)Calamity.Call("Downed", "oldduke")) { PowerLevel = 50; } // 50
                else if ((bool)Calamity.Call("Downed", "polterghast")) { PowerLevel = 49; } // 49
                else if ((bool)Calamity.Call("Downed", "signus")) { PowerLevel = 48; } // 48
                else if ((bool)Calamity.Call("Downed", "stormweaver")) { PowerLevel = 47; } // 47
                else if ((bool)Calamity.Call("Downed", "ceaselessvoid")) { PowerLevel = 46; } // 46
                else if ((bool)Calamity.Call("Downed", "providence")) { PowerLevel = 44; } // 44
                else if ((bool)Calamity.Call("Downed", "dragonfolly")) { PowerLevel = 43; } // 43
                else if ((bool)Calamity.Call("Downed", "guardians")) { PowerLevel = 42; } // 42
                else if ((bool)thoriumMod.Call("GetDownedBoss", "ThePrimordials")) { PowerLevel = 45; } // 45
                else if (NPC.downedMoonlord == true) { PowerLevel = 41; } // 41
                else if ((bool)Calamity.Call("Downed", "astrumdeus")) { PowerLevel = 40; } // 40
                else if (NPC.downedAncientCultist == true) { PowerLevel = 39; } // 39
                else if ((bool)Calamity.Call("Downed", "ravager")) { PowerLevel = 38; } // 38
                // Betsy skip (downedBetsy doesnt exist)
                else if (NPC.downedEmpressOfLight == true) { PowerLevel = 36; } // 36
                else if ((bool)Calamity.Call("Downed", "plaguebringergoliath")) { PowerLevel = 35; } // 35
                else if (NPC.downedFishron == true) { PowerLevel = 34; } // 34
                else if ((bool)thoriumMod.Call("GetDownedBoss", "ForgottenOne")) { PowerLevel = 33; } // 33
                else if (NPC.downedGolemBoss == true) { PowerLevel = 32; } // 32
                else if ((bool)Calamity.Call("Downed", "astrumaureus")) { PowerLevel = 31; } // 31
                else if ((bool)Calamity.Call("Downed", "anahitaleviathan")) { PowerLevel = 30; } // 30
                else if (NPC.downedPlantBoss == true) { PowerLevel = 29; } // 29
                else if ((bool)Calamity.Call("Downed", "calamitasclone")) { PowerLevel = 28; } // 28
                else if ((bool)thoriumMod.Call("GetDownedBoss", "Lich")) { PowerLevel = 27; } // 27
                else if (NPC.downedMechBoss3 == true) { PowerLevel = 26; } // 26
                else if (NPC.downedMechBoss2 == true) { PowerLevel = 24; } // 24
                else if ((bool)Calamity.Call("Downed", "brimstoneelemental")) { PowerLevel = 25; } // 25
                else if (NPC.downedMechBoss1 == true) { PowerLevel = 22; } // 22
                else if ((bool)Calamity.Call("Downed", "cryogen")) { PowerLevel = 21; } // 21
                else if (NPC.downedQueenSlime == true) { PowerLevel = 20; } // 20
                else if ((bool)thoriumMod.Call("GetDownedBoss", "FallenBeholder")) { PowerLevel = 19; } // 19
                else if ((bool)thoriumMod.Call("GetDownedBoss", "BoreanStrider")) { PowerLevel = 18; } // 18
                else if (Main.hardMode == true) { PowerLevel = 17; } // 17
                else if ((bool)thoriumMod.Call("GetDownedBoss", "StarScouter")) { PowerLevel = 16; } // 16
                else if ((bool)Calamity.Call("Downed", "slimegod")) { PowerLevel = 15; } // 15
                else if ((bool)thoriumMod.Call("GetDownedBoss", "BuriedChampion")) { PowerLevel = 14; } // 14
                else if ((bool)thoriumMod.Call("GetDownedBoss", "GraniteEnergyStorm")) { PowerLevel = 13; } // 
                else if (NPC.downedDeerclops == true) { PowerLevel = 12; } // 12
                else if (NPC.downedBoss3 == true) { PowerLevel = 11; } // 11
                else if (NPC.downedQueenBee == true) { PowerLevel = 10; } // 10
                else if ((bool)Calamity.Call("Downed", "hivemind") || ((bool)Calamity.Call("Downed", "perforator"))) { PowerLevel = 9; } // 9
                else if ((bool)thoriumMod.Call("GetDownedBoss", "Viscount")) { PowerLevel = 8; } // 8
                else if ((bool)thoriumMod.Call("GetDownedBoss", "QueenJellyfish")) { PowerLevel = 7; } // 7
                else if (NPC.downedBoss2 == true) { PowerLevel = 6; } // 6
                else if ((bool)Calamity.Call("Downed", "crabulon")) { PowerLevel = 5; } // 5
                else if (NPC.downedBoss1 == true) { PowerLevel = 4; } // 4
                else if ((bool)Calamity.Call("Downed", "desertscourge")) { PowerLevel = 3; } // 3
                else if (NPC.downedSlimeKing == true) { PowerLevel = 2; } // 2
                else if ((bool)thoriumMod.Call("GetDownedBoss", "TheGrandThunderBird")) { PowerLevel = 1; } // 1
            }

            switch (PowerLevel)
            {
                case 1: Item.damage = 10; Item.crit = 1; break;
                case 2: Item.damage = 20; Item.crit = 2; break;
                case 3: Item.damage = 30; Item.crit = 4; break;
                case 5: Item.damage = 42; Item.crit = 7; break;
                case 6: Item.damage = 43; Item.crit = 9; break;
                case 7: Item.damage = 45; Item.crit = 11; break;
                case 8: Item.damage = 48; Item.crit = 13; break;
                case 9: Item.damage = 51; Item.crit = 15; break;
                case 10: Item.damage = 52; Item.crit = 17; break;
                case 11: Item.damage = 53; Item.crit = 19; break;
                case 12: Item.damage = 54; Item.crit = 21; break;
                case 13: Item.damage = 55; Item.crit = 23; break;
                case 14: Item.damage = 60; Item.crit = 25; break;
                case 15: Item.damage = 64; Item.crit = 27; break;
                case 16: Item.damage = 69; Item.crit = 29; break;
                case 17: Item.damage = 71; Item.crit = 31; break;
                case 18: Item.damage = 73; Item.crit = 33; break;
                case 19: Item.damage = 75; Item.crit = 35; break;
                case 20: Item.damage = 78; Item.crit = 36; break;
                case 21: Item.damage = 80; Item.crit = 38; break;
                case 22: Item.damage = 85; Item.crit = 40; break;
                case 23: Item.damage = 90; Item.crit = 42; break;
                case 24: Item.damage = 100; Item.crit = 44; break;
                case 25: Item.damage = 110; Item.crit = 45; break;
                case 26: Item.damage = 125; Item.crit = 47; break;
                case 27: Item.damage = 140; Item.crit = 49; break;
                case 28: Item.damage = 170; Item.crit = 51; break;
                case 29: Item.damage = 220; Item.crit = 53; break;
                case 30: Item.damage = 245; Item.crit = 55; break;
                case 31: Item.damage = 260; Item.crit = 56; break;
                case 32: Item.damage = 270; Item.crit = 58; break;
                case 33: Item.damage = 285; Item.crit = 60; break;
                case 34: Item.damage = 300; Item.crit = 62; break;
                case 35: Item.damage = 335; Item.crit = 64; break;
                case 36: Item.damage = 370; Item.crit = 65; break;
                //case 37: Item.damage = 10; Item.crit = 67; break; // Betsy (skipped)
                case 38: Item.damage = 440; Item.crit = 67; break;
                case 39: Item.damage = 495; Item.crit = 69; break;
                case 40: Item.damage = 560; Item.crit = 71; break;
                case 41: Item.damage = 630; Item.crit = 20; break;
                case 42: Item.damage = 650; Item.crit = 25; break;
                case 43: Item.damage = 750; Item.crit = 37; break;
                case 44: Item.damage = 770; Item.crit = 37; break;
                case 45: Item.damage = 820; Item.crit = 43; break;
                case 46: Item.damage = 1050; Item.crit = 45; break;
                case 47: Item.damage = 1230; Item.crit = 50; break;
                case 48: Item.damage = 1280; Item.crit = 55; break;
                case 49: Item.damage = 1315; Item.crit = 60; break;
                case 50: Item.damage = 1345; Item.crit = 63; break;
                case 51: Item.damage = 1515; Item.crit = 68; break;
                case 52: Item.damage = 2200; Item.crit = 75; break;
                case 53: Item.damage = 3550; Item.crit = 90; break;
                case 54: Item.damage = 4015; Item.crit = 100; break;
                case 55: Item.damage = 5115; Item.crit = 150; break;
            }
        }
    }
}