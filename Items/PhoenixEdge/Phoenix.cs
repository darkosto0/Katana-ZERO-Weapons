using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.Audio;
using KatanaZERO.Items.ZerosKatana;
using static Terraria.ModLoader.ModLoader;

namespace KatanaZERO.Items.PhoenixEdge
{
    public class Phoenix : ModItem
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

        private float attackCooldown = 0f;
        public bool hasAttacked = false;
        private const float DistanceFromCore = 66f;
        private readonly KatanaZERO mod = ModContent.GetInstance<KatanaZERO>();

        public override void SetDefaults()
        {
            string selectedWeaponSound = KatanaZERO.phoenixEdgeSound;

            if (KatanaZERO.progressionDamage)
            {
                PowerLevelManager power = new();
                power.Apply(Item);
            }
            else
            {
                Item.damage = 224;
                Item.crit = 45;
            }

            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.Quest;
            Item.value = Item.sellPrice(gold: 29, silver: 79);
            Item.UseSound = GetRandomWeaponSound(selectedWeaponSound);

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
            Item.knockBack = 3;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PhoenixSlash>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<ZeroKatana>())
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddIngredient(ItemID.LivingFireBlock, 5)
                .AddIngredient(ItemID.CursedFlame, 3)
                .AddTile(TileID.LunarCraftingStation)
                .AddCustomShimmerResult(ItemType<ZeroKatana>())
                .AddCustomShimmerResult(ItemID.FragmentSolar, 10)
                .AddCustomShimmerResult(ItemID.LivingFireBlock, 5)
                .AddCustomShimmerResult(ItemID.CursedFlame, 3)
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
                
            string selectedWeaponSound = KatanaZERO.phoenixEdgeSound;

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

                type = ModContent.ProjectileType<PhoenixFlames>();

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

