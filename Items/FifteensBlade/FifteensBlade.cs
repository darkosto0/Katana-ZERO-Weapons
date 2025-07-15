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
        public float Y;
        public float X;


        public Color glitchColor = ColorHelper.HexToColor("#00ccff");
        private Vector2? frozenPosition = null;

        public bool hasAttacked = false;
        public bool playedSlomoEngage = false;
        public bool hasRightClicked = false;
        public bool hasReleasedRightClick = false;
        public bool rightClickActivated = false;
        public bool playerFreeze = false;




        public override void SetDefaults()
        {
            string selectedWeaponSound = KatanaZERO.fifteensKatanaSound;
            PowerLevelManager power = new();

            power.Apply(Item); //THIS. IS. POWER.

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(gold: 99, silver: 99);
            Item.UseSound = GetRandomWeaponSound(selectedWeaponSound);
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

            string selectedWeaponSound = KatanaZERO.fifteensKatanaSound;

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
            SetDefaults();
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
                }
                CreateAllDust(player);
                if (KatanaZERO.enableTimeShift)
                {
                    SlowDown(player);
                    SlomoSoundEffect(player);
                }
                else
                {
                    SlomoSoundEffect(player);
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

        public override bool AltFunctionUse(Player player) => false;

        public void CreateAllDust(Player player)
        {
            if (Main.myPlayer != player.whoAmI || Main.HoveringOverAnNPC || Main.playerInventory || Main.InGuideCraftMenu || Main.InReforgeMenu) // dont enter the dragon dash sequence at ALL
            {
                return;
            }

            if (Main.mouseRight && player.statMana >= 2)
            {
                int dustAmount = 800;
                float radius = 464f;
                int dustType = ModContent.DustType<FifteenDust>();
                float opacity = (1f - (Math.Max(0f, player.statMana - 2f) / (float)player.statManaMax)) * 255f;

                for (int i = 0; i < dustAmount; i++) //create circle
                {
                    float angle = MathHelper.TwoPi / dustAmount * i;
                    Vector2 position = player.Center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Dust dustCircle = Dust.NewDustPerfect(position, dustType);
                    dustCircle.alpha = (int)opacity;
                    
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
                    dust.alpha = (int)opacity;
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
                    dustLargerCircle.alpha = (int)opacity;
                    dustLargerCircle.noGravity = true;
                    dustLargerCircle.noLight = true;
                }


                if (KatanaZERO.enableTimeShift) 
                    player.CheckMana(2, true, true);
                FreezePlayerIfHolding(player);
            }
            else
            {
                frozenPosition = null;
            }
        }

        public void FreezePlayerIfHolding(Player player)
        {
            if (Main.myPlayer != player.whoAmI || Main.HoveringOverAnNPC || Main.playerInventory || Main.InGuideCraftMenu || Main.InReforgeMenu) // dont enter the dragon dash sequence at ALL
            {
                return;
            }

            if (frozenPosition == null)
            {
                frozenPosition = player.position;
            }
            player.position = frozenPosition.Value;
            player.velocity = Vector2.Zero;
            player.fallStart = (int)(player.position.Y / 16f); // prevent fall damage accumulation
        }

        public void CreateDashDust(Vector2 position, Player player, float Xoffsetter, float Yoffsetter, bool topDust, bool middleDust, bool bottomDust)
        {
            if (Main.myPlayer != player.whoAmI || Main.HoveringOverAnNPC || Main.playerInventory || Main.InGuideCraftMenu || Main.InReforgeMenu) // dont enter the dragon dash sequence at ALL
            {
                return;
            }

            position += new Vector2((player.width / 2) - Xoffsetter, (player.height / 2 - 20) + Yoffsetter);

            if (topDust) //yandere simulator final boss
            {
                if (KatanaZERO.dashTrailStyle != "Fireflies") //"i shall, set the seas ablaze!"
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
                if (KatanaZERO.dashTrailStyle != "Fireflies")
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
                if (KatanaZERO.dashTrailStyle != "Fireflies")
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
                if (KatanaZERO.dashTrailStyle != "Fireflies")
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
                if (KatanaZERO.dashTrailStyle != "Fireflies")
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

        public void DragonDash(Player player)
        {
            if (Main.myPlayer != player.whoAmI || Main.HoveringOverAnNPC || Main.playerInventory || Main.InGuideCraftMenu || Main.InReforgeMenu) // dont enter the dragon dash sequence at ALL
            {
                return;
            }
            SetDefaults();

            if (player.statMana <= 10)
            {
                dragonCooldown = 60;
                return;
            }

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

                if (KatanaZERO.dashTrailStyle != "Disabled")
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
                    if (!enemy.friendly && enemy.Hitbox.Intersects(playerHitbox) && enemy.netID != 549)
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

            dragonCooldown = 180;

        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
            hasAttacked = reader.ReadBoolean();
            attackCooldown = reader.ReadSingle();
            dragonCooldown = reader.ReadSingle();
            hasRightClicked = reader.ReadBoolean();
            hasReleasedRightClick = reader.ReadBoolean();

            byte packetType = reader.ReadByte();
            switch (packetType)
            {
                case 1: // NPC velocity change
                    int npcID = reader.ReadInt32();
                    Vector2 npcVelocity = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                    Main.npc[npcID].velocity = npcVelocity;
                    break;
                case 2: // Projectile velocity change
                    int projID = reader.ReadInt32();
                    Vector2 projVelocity = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                    Main.projectile[projID].velocity = projVelocity;
                    break;
                case 3: // Dust creation
                    Vector2 dustPosition = reader.ReadVector2();
                    int dustType = reader.ReadInt32();
                    Dust.NewDustPerfect(dustPosition, dustType);
                    break;
            }
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
            if (Main.mouseRight && player.statMana > 2)
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
                            Vector2 currentDirection = mob.velocity.SafeNormalize(Vector2.Zero);
                            Vector2 newVelocity = currentDirection * desiredSpeed;
                            mob.velocity = newVelocity;

                            if (Main.netMode == NetmodeID.Server)
                            {
                                SendNPCVelocityChange(mob.whoAmI, newVelocity);
                            }
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
                                Vector2 newVelocity = currentDirection * desiredSpeed;
                                proj.velocity = newVelocity;

                                if (Main.netMode == NetmodeID.Server)
                                {
                                    SendProjectileVelocityChange(proj.whoAmI, newVelocity);
                                }
                            }
                        }
                        else
                        {
                            originalProjectileSpeeds.Remove(proj.whoAmI);
                        }
                    }
                }
            }
            else if ((Main.mouseRight == false && rightClickActivated == true) || player.statMana <= 2) //right click released
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
                    Vector2 newVelocity = currentDirection * originalSpeed;
                    mob.velocity = newVelocity;

                    // Send the reverted velocity to all clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        SendNPCVelocityChange(mob.whoAmI, newVelocity);
                    }

                    originalNPCSpeeds.Remove(mob.whoAmI);
                }
            }

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && originalProjectileSpeeds.ContainsKey(proj.whoAmI))
                {
                    float originalSpeed = originalProjectileSpeeds[proj.whoAmI];
                    Vector2 currentDirection = proj.velocity.SafeNormalize(Vector2.Zero);
                    Vector2 newVelocity = currentDirection * originalSpeed;
                    proj.velocity = newVelocity;

                    // Send the reverted velocity to all clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        SendProjectileVelocityChange(proj.whoAmI, newVelocity);
                    }

                    originalProjectileSpeeds.Remove(proj.whoAmI);
                }
            }
        }

        private void SendNPCVelocityChange(int npcID, Vector2 newVelocity)
        {
            ModPacket packet = ModContent.GetInstance<KatanaZERO>().GetPacket();
            packet.Write((byte)1);
            packet.Write(npcID);
            packet.Write(newVelocity.X);
            packet.Write(newVelocity.Y);
            packet.Send();
        }

        private void SendProjectileVelocityChange(int projID, Vector2 newVelocity)
        {
            ModPacket packet = ModContent.GetInstance<KatanaZERO>().GetPacket();
            packet.Write((byte)2);
            packet.Write(projID);
            packet.Write(newVelocity.X);
            packet.Write(newVelocity.Y);
            packet.Send();
        }

        public void SlomoSoundEffect(Player player)
        {
            if (Main.myPlayer != player.whoAmI || Main.HoveringOverAnNPC || Main.playerInventory || Main.InGuideCraftMenu || Main.InReforgeMenu) // dont enter the dragon dash sequence at ALL
            {
                return;
            }

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