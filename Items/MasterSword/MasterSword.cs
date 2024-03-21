using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using rail;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace KatanaZERO.Items.MasterSword
{
    public class MasterSword : ModItem
    {
        //public static readonly SoundStyle MasterSwordCharge = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_charged_01");
        //public static readonly SoundStyle MasterSwordImpact = new SoundStyle("");
        //public static readonly SoundStyle MasterSwordShot = new SoundStyle("");
        public static readonly SoundStyle Slash1 = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_slash_01");
        public static readonly SoundStyle Slash2 = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_slash_02");
        public static readonly SoundStyle Slash3 = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_slash_03");

        private float attackCooldown = 0f;
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
            Item.crit = 40;

            Item.useTime = 20;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 6;

            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MasterSwordSlash>();
          //  Item.shoot = ModContent.ProjectileType<MasterSwordBeam>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddIngredient(ItemID.BluePhasesaber, 1);
            recipe.AddIngredient(ItemID.Sapphire, 10);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
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
                player.velocity = direction * 4f;
            }
            else
            {
                player.velocity = direction * 10f;
                hasAttacked = true;
            }


            attackCooldown = 30f; //artificial cooldown

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

