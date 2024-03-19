using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace KatanaZERO.Items.ZerosKatana
{
    public class ZerosKatana : ModItem
    {
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/Katana/katana_slash1");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/Katana/katana_slash2");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/Katana/katana_slash3");

        private float attackCooldown = 0f;
        public bool hasAttacked = false;

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(gold: 1, silver: 10); //same sell price as katana and 5 silver bars
            Item.UseSound = Slash1;
            Item.crit = 15;

            Item.useTime = 20;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.holdStyle = 0;
            Item.knockBack = 6;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ZerosKatanaSlash>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Katana, 1);
            recipe.AddIngredient(ItemID.SilverBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
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

