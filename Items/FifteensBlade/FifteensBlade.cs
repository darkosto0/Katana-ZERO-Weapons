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

namespace KatanaZERO.Items.FifteensBlade
{
    public class FifteensBlade : ModItem
    {
        //Please forgive me
        public static SoundStyle GetSlashSound(string swordName, int slashNumber)
        {
            return new SoundStyle($"KatanaZERO/Sounds/Items/{swordName}/slash{slashNumber}");
        }

        public static readonly SoundStyle Slash1 = GetSlashSound("Katana", 1);
        public static readonly SoundStyle Slash2 = GetSlashSound("Katana", 2);
        public static readonly SoundStyle Slash3 = GetSlashSound("Katana", 3);

        public static readonly SoundStyle PrismSlash1 = GetSlashSound("PrismSword", 1);
        public static readonly SoundStyle PrismSlash2 = GetSlashSound("PrismSword", 2);
        public static readonly SoundStyle PrismSlash3 = GetSlashSound("PrismSword", 3);

        public static readonly SoundStyle MasterSlash1 = GetSlashSound("MasterSword", 1);
        public static readonly SoundStyle MasterSlash2 = GetSlashSound("MasterSword", 2);
        public static readonly SoundStyle MasterSlash3 = GetSlashSound("MasterSword", 3);

        public static readonly SoundStyle SavantSlash1 = GetSlashSound("SavantKnife", 1);
        public static readonly SoundStyle SavantSlash2 = GetSlashSound("SavantKnife", 2);
        public static readonly SoundStyle SavantSlash3 = GetSlashSound("SavantKnife", 3);

        public static readonly SoundStyle ClaymoreSlash1 = GetSlashSound("Claymore", 1);
        public static readonly SoundStyle ClaymoreSlash2 = GetSlashSound("Claymore", 2);
        public static readonly SoundStyle ClaymoreSlash3 = GetSlashSound("Claymore", 3);

        public static readonly SoundStyle PhoenixSlash1 = GetSlashSound("PhoenixEdge", 1);
        public static readonly SoundStyle PhoenixSlash2 = GetSlashSound("PhoenixEdge", 2);
        public static readonly SoundStyle PhoenixSlash3 = GetSlashSound("PhoenixEdge", 3);

        public static readonly SoundStyle FifteenSlash1 = GetSlashSound("FifteensBlade", 1);
        public static readonly SoundStyle FifteenSlash2 = GetSlashSound("FifteensBlade", 2);
        public static readonly SoundStyle FifteenSlash3 = GetSlashSound("FifteensBlade", 3);

        public static readonly SoundStyle Special1 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special1");
        public static readonly SoundStyle Special2 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special2");
        public static readonly SoundStyle Special3 = new SoundStyle("KatanaZERO/Sounds/Items/FifteensBlade/fifteen_special3");

        public static readonly SoundStyle SlowEngage = new SoundStyle("KatanaZERO/Sounds/slomo_engage");
        public static readonly SoundStyle SlowDisengage = new SoundStyle("KatanaZERO/Sounds/slomo_disengage");

        private Dictionary<int, float> originalNPCSpeeds = new Dictionary<int, float>();
        private Dictionary<int, float> originalProjectileSpeeds = new Dictionary<int, float>();

        const float slowdownFactor = 0.25f;
        const float maxDistance = 2000f;

        public float attackCooldown = 0f;
        public float dragonCooldown = 0f;
        public int SlowMoCounter = 0;
        public int PowerLevel;
        public Color glitchColor = ColorHelper.HexToColor("#00ccff");

        public bool hasAttacked = false;
        public bool playedSlomoEngage = false;
        public bool hasRightClicked = false;
        public bool hasReleasedRightClick = false;
        public bool rightClickActivated = false;

        private readonly KatanaZERO mod = ModContent.GetInstance<KatanaZERO>();

        public override void SetDefaults()
        {
            string selectedWeaponSound = mod.fifteensKatanaSound;

            Item.damage = 6;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(gold: 99, silver: 99);
            Item.UseSound = GetRandomWeaponSound(selectedWeaponSound);
            Item.crit = 0;
            Item.knockBack = 4;

            Item.useTime = 20;
            if (HasMod("FargowiltasSouls"))
            {
                Item.useAnimation = 20;
            }
            else
            {
                Item.useAnimation = 1;
            }

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<FifteensSlash>();
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

            attackCooldown = 20f; //artificial cooldown

            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            string selectedWeaponSound = mod.fifteensKatanaSound;

            SoundStyle weaponSound = GetRandomWeaponSound(selectedWeaponSound);

            switch (randomNumber)
            {
                case 1:
                    Item.UseSound = weaponSound;
                    break;
                case 2:
                    Item.UseSound = weaponSound;
                    break;
                case 3:
                    Item.UseSound = weaponSound;
                    break;
            }
            return true;
        }

        public override void HoldItem(Player player)
        {
            if (dragonCooldown <= 0)
            {
                if (!Main.mouseItem.IsAir) //i tried adding an easter egg here if you try triggering the glitch, which is why it took me so long to publish the bugfix, i couldnt make it work properly
                {
                    hasRightClicked = false;
                    return;
                }
                if (Main.mouseRight && !hasRightClicked)
                {
                    hasRightClicked = true;
                }
                if (!Main.mouseRight && hasRightClicked)
                {
                    DragonDash(player);
                    hasRightClicked = false;
                    dragonCooldown = 180;
                }
                CreateAllDust(player);
                if (KatanaZERO.enableTimeShift)
                {
                    SlowDown(player);
                    SlomoSoundEffect();
                }
                else
                {
                    SlomoSoundEffect();
                }
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
                int dustAmount = 800;
                float radius = 464f;
                int dustType = ModContent.DustType<FifteenDust>();

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

                for (float lerpAmount = 0; lerpAmount <= maxLerpAmount; lerpAmount += 0.004f)
                {
                    Vector2 barPosition = Vector2.Lerp(player.Center, Main.MouseWorld, lerpAmount);
                    if (Collision.SolidCollision(barPosition, 10, 10))
                    {
                        break; //stop drawing when collision occurs
                    }

                    Dust dust = Dust.NewDustPerfect(barPosition, dustType);
                    dust.noGravity = true;
                }

                Vector2 endPosition = Vector2.Lerp(player.Center, Main.MouseWorld, maxLerpAmount);
                int dustAmount1 = 10;

                float largerRadius = 3.3f;
                for (int i = 0; i < dustAmount1; i++)
                {
                    float angle = MathHelper.TwoPi / dustAmount1 * i;
                    Vector2 position = endPosition + largerRadius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    if (Collision.SolidCollision(position, 1, 1))
                    {
                        break;
                    }
                    Dust dustLargerCircle = Dust.NewDustPerfect(position, dustType);
                    dustLargerCircle.noGravity = true;
                    dustLargerCircle.noLight = true;
                }
                player.velocity = Vector2.Zero;
            }
        }

        public void CreateDashDust(Vector2 position, Player player, float Xoffsetter, float Yoffsetter, bool topDust, bool middleDust, bool bottomDust)
        {
            position += new Vector2((player.width / 2) - Xoffsetter, (player.height / 2 - 20) + Yoffsetter);

            if (topDust) //yandere simulator final boss
            {
                if (mod.dashTrailStyle != "Fireflies") //"i shall, set the seas ablaze!"
                {
                    Color dustColor = ColorHelper.HexToColor("03FFB0");
                    Dust dust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<DashDust>(), 0f, 0f, 0, dustColor, 1f);
                }
                else
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.Firefly, null, 0, default, 1);
                }
            }
            else if (Yoffsetter == 10)
            {
                if (mod.dashTrailStyle != "Fireflies")
                {
                    Color dustColor = ColorHelper.HexToColor("FC9DCE");
                    Dust dust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<DashDust>(), 0f, 0f, 0, dustColor, 1f);
                }
                else
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.Firefly, null, 0, default, 1);
                }
            }
            else if (middleDust)
            {
                if (mod.dashTrailStyle != "Fireflies")
                {
                    Color dustColor = ColorHelper.HexToColor("AC408E");
                    Dust dust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<DashDust>(), 0f, 0f, 0, dustColor, 1f);
                }
                else
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.Firefly, null, 0, default, 1);
                }
            }
            else if (Yoffsetter == 32)
            {
                if (mod.dashTrailStyle != "Fireflies")
                {
                    Color dustColor = ColorHelper.HexToColor("2F92BC");
                    Dust dust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<DashDust>(), 0f, 0f, 0, dustColor, 1f);
                }
                else
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.Firefly, null, 0, default, 1);
                }
            }
            else if (bottomDust)
            {
                if (mod.dashTrailStyle != "Fireflies")
                {
                    Color dustColor = ColorHelper.HexToColor("393073");
                    Dust dust = Dust.NewDustDirect(position, 0, 0, ModContent.DustType<DashDust>(), 0f, 0f, 0, dustColor, 1f);
                }
                else
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.Firefly, null, 0, default, 1);
                }
            }
        }

        public bool DragonDash(Player player)
        {
            if (!Main.myPlayer.Equals(player.whoAmI))
                return false;

            Vector2 cursorPosition = Main.MouseWorld;
            Vector2 playerPosition = player.Center;

            float x, y;
            bool isLookingLeft;

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
            player.immuneTime = ((int)dashTicks < 30) ? 30 : ((int)dashTicks * 2);

            int hitboxWidth = player.width * 3;
            int hitboxHeight = player.height * 3;

            Vector2 dashStep = (directionToCursor * (dashSpeed / 60f));

            Dictionary<int, bool> hitEnemies = new Dictionary<int, bool>();

            bool topDust = false;
            bool middleDust = false;
            bool bottomDust = false;

            for (float i = 0; i < dashTicks; i++)
            {
                Vector2 nextPosition = player.position + dashStep;

                if (mod.dashTrailStyle != "Disabled")
                {
                    isLookingLeft = Main.MouseWorld.X < player.Center.X;
                    x = isLookingLeft ? 6f : -6f; // Start at 9 for left, -9 for right

                    for (y = 0; y < 40; y += 2)
                    {
                        if (isLookingLeft)
                        {
                            if (x > 0 && y <= 8)
                            {
                                topDust = true;
                                CreateDashDust(nextPosition, player, x, y, topDust, middleDust, bottomDust);
                                x -= 1.5f;
                                topDust = false;
                            }
                            else if (x == 0 && y > 7 && y < 31)
                            {
                                middleDust = true;
                                CreateDashDust(nextPosition, player, x, y, topDust, middleDust, bottomDust);
                                middleDust = false;
                            }
                            else if (x >= 0)
                            {
                                bottomDust = true;
                                x += 1.5f;
                                CreateDashDust(nextPosition, player, x, y, topDust, middleDust, bottomDust);
                                bottomDust = false;
                            }
                        }
                        else
                        {
                            if (x < 0 && y <= 8)
                            {
                                topDust = true;
                                CreateDashDust(nextPosition, player, x, y, topDust, middleDust, bottomDust);
                                x += 1.5f;
                                topDust = false;
                            }
                            else if (x == 0 && y > 7 && y < 31)
                            {
                                middleDust = true;
                                CreateDashDust(nextPosition, player, x, y, topDust, middleDust, bottomDust);
                                middleDust = false;
                            }
                            else if (x <= 0)
                            {
                                bottomDust = true;
                                x -= 1.5f;
                                CreateDashDust(nextPosition, player, x, y, topDust, middleDust, bottomDust);
                                bottomDust = false;
                            }
                        }
                    }
                }

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

        public void SlowDown(Player player)
        {
            if (Main.mouseRight)
            {
                foreach (NPC mob in Main.npc)
                {
                    if (mob.active && !mob.friendly && mob.Distance(player.Center) < maxDistance)
                    {
                        if (!originalNPCSpeeds.ContainsKey(mob.whoAmI))
                        {
                            originalNPCSpeeds[mob.whoAmI] = mob.velocity.Length();
                        }
                        float originalSpeed = originalNPCSpeeds[mob.whoAmI];
                        float desiredSpeed = originalSpeed * slowdownFactor;

                        if (mob.velocity.Length() > desiredSpeed)
                        {
                            Vector2 currentDirection = mob.velocity.SafeNormalize(Vector2.Zero); // Avoid division by zero
                            mob.velocity = currentDirection * desiredSpeed; // Set new velocity
                        }
                    }
                    else
                    {
                        originalNPCSpeeds.Remove(mob.whoAmI);
                    }
                }

                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.type != ProjectileID.WaterGun)
                    {
                        if (proj.Distance(player.Center) < maxDistance)
                        {
                            if (!originalProjectileSpeeds.ContainsKey(proj.whoAmI))
                            {
                                originalProjectileSpeeds[proj.whoAmI] = proj.velocity.Length();
                            }
                            float originalSpeed = originalProjectileSpeeds[proj.whoAmI];
                            float desiredSpeed = originalSpeed * slowdownFactor;

                            if (proj.velocity.Length() > desiredSpeed)
                            {
                                Vector2 currentDirection = proj.velocity.SafeNormalize(Vector2.Zero);
                                proj.velocity = currentDirection * desiredSpeed;
                            }
                        }
                        else
                        {
                            originalProjectileSpeeds.Remove(proj.whoAmI);
                        }
                    }
                }
            }
            else if (Main.mouseRight == false && rightClickActivated == true) //right click released
            {
                RevertNormalSpeed();
            }
        }

        public void RevertNormalSpeed()
        {
            foreach (NPC mob in Main.npc)
            {
                if (mob.active && originalNPCSpeeds.ContainsKey(mob.whoAmI))
                {
                    float originalSpeed = originalNPCSpeeds[mob.whoAmI];
                    Vector2 currentDirection = mob.velocity.SafeNormalize(Vector2.Zero);
                    mob.velocity = currentDirection * originalSpeed;
                    originalNPCSpeeds.Remove(mob.whoAmI);
                }
            }

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && originalProjectileSpeeds.ContainsKey(proj.whoAmI))
                {
                    float originalSpeed = originalProjectileSpeeds[proj.whoAmI];
                    Vector2 currentDirection = proj.velocity.SafeNormalize(Vector2.Zero);
                    proj.velocity = currentDirection * originalSpeed;
                    originalProjectileSpeeds.Remove(proj.whoAmI);
                }
            }
        }

        public bool SlomoSoundEffect()
        {
            if (Main.mouseRight)
            {
                if (playedSlomoEngage == false)
                {
                    SoundEngine.PlaySound(SlowEngage);
                    playedSlomoEngage = true;
                }
                rightClickActivated = true;
            }
            else if (Main.mouseRight == false && rightClickActivated == true) //right click released
            {
                SoundEngine.PlaySound(SlowDisengage);

                playedSlomoEngage = false;
                rightClickActivated = false;
            }
            return true;
        }

        public void StatChange() // lord forgive me for i have pulled a yandere simulator - Darkosto // So gosh darn true.. - Sequile
        {

            TryGetMod("CalamityMod", out Mod Calamity);
            TryGetMod("ThoriumMod", out Mod thoriumMod);

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
        public static SoundStyle GetRandomWeaponSound(string weaponSoundOption)
        {
            Random random = new Random();
            int randomSlash = random.Next(1, 4);

            switch (weaponSoundOption)
            {
                case "Zero's Katana":
                    return randomSlash switch
                    {
                        1 => Slash1,
                        2 => Slash2,
                        3 => Slash3,
                        _ => Slash1
                    };
                case "Prism Sword":
                    return randomSlash switch
                    {
                        1 => PrismSlash1,
                        2 => PrismSlash2,
                        3 => PrismSlash3,
                        _ => PrismSlash1
                    };
                case "Master Sword":
                    return randomSlash switch
                    {
                        1 => MasterSlash1,
                        2 => MasterSlash2,
                        3 => MasterSlash3,
                        _ => MasterSlash1
                    };
                case "Savant Knife":
                    return randomSlash switch
                    {
                        1 => SavantSlash1,
                        2 => SavantSlash2,
                        3 => SavantSlash3,
                        _ => SavantSlash1
                    };
                case "Claymore Prototype":
                    return randomSlash switch
                    {
                        1 => ClaymoreSlash1,
                        2 => ClaymoreSlash2,
                        3 => ClaymoreSlash3,
                        _ => ClaymoreSlash1
                    };
                case "Phoenix Edge":
                    return randomSlash switch
                    {
                        1 => PhoenixSlash1,
                        2 => PhoenixSlash2,
                        3 => PhoenixSlash3,
                        _ => PhoenixSlash1
                    };
                case "Dragon's Whisper":
                    return randomSlash switch
                    {
                        1 => FifteenSlash1,
                        2 => FifteenSlash2,
                        3 => FifteenSlash3,
                        _ => FifteenSlash1
                    };
                default:
                    return Slash1;
            }
        }
    }
}