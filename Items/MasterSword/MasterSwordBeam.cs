using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Player = Terraria.Player;

namespace KatanaZERO.Items.MasterSword
{
    public class MasterSwordBeam : ModProjectile
    {
        public static readonly SoundStyle Shoot = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/sound_player_mastersword_shot_01");

        private const float DistanceFromCore = 132f;
        private const float projectileSpeed = 10f;
        
        public override void SetDefaults()
        {
            Projectile.width = 52; //default 52
            Projectile.height = 32; //default 32
            Projectile.knockBack = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 300;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.ownerHitCheck = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Main.projFrames[Projectile.type] = 3;
        }

        public override void AI()
        {
            //animate projectile
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            Dust dust = Dust.NewDustPerfect(Position: Projectile.Center, DustID.MagicMirror, Velocity: Vector2.Zero, Alpha: 100, Scale: 0.9f); 
            dust.noGravity = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            float angleToCursor = (float)Math.Atan2(Main.MouseWorld.Y - player.Center.Y, Main.MouseWorld.X - player.Center.X);
            Vector2 offset = new Vector2(DistanceFromCore, 0f).RotatedBy(angleToCursor);
            Projectile.position = player.Center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2);

            float speed = 10f;
            Projectile.velocity = new Vector2((float)Math.Cos(angleToCursor), (float)Math.Sin(angleToCursor)) * speed;

            if (Main.MouseWorld.X < player.Center.X) //fix sprite orientation
            {
                Projectile.rotation = angleToCursor + MathHelper.Pi; //rotate by 180 deg
                Projectile.spriteDirection = -1; //mirror the sprite on its Y-axis
            }
            else
            {
                Projectile.rotation = angleToCursor; //normal rotation
                Projectile.spriteDirection = 1; //normal sprite direction
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle Impact = new SoundStyle("KatanaZERO/Sounds/Items/MasterSword/impact_01") { Volume = 0.5f };
            SoundEngine.PlaySound(Impact, Entity.Center);
            target.AddBuff(BuffID.ShadowFlame, 10 * 60);
        }

        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 30; i++)
            {
                //Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 150, default(Color), 1.5f);
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 0, default, 1);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);

        }
    }
}
