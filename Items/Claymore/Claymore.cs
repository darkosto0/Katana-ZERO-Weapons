using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            Item.crit = 60;

            Item.useTime = 20;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = ItemHoldStyleID.HoldRadio;
            Item.knockBack = 6;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ClaymoreSlash>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BreakerBlade, 1);
            recipe.AddIngredient(ItemID.SpectreBar, 10);
            recipe.AddIngredient(ItemID.BeetleShell, 3);
            recipe.AddIngredient(ItemID.EyeoftheGolem, 1);
            recipe.AddTile(TileID.MythrilAnvil);
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
    } 
}

