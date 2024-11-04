using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Player = Terraria.Player;

namespace KatanaZERO.Items.PhoenixEdge
{
    public class PhoenixFlames : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 76; //default 52
            Projectile.height = 65; //default 32
            Projectile.knockBack = 3;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 50;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.ownerHitCheck = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Main.projFrames[Projectile.type] = 10;
        }

        public override void AI()
        {
            //animate projectile
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            float angleToCursor = (float)Math.Atan2(Main.MouseWorld.Y - player.Center.Y, Main.MouseWorld.X - player.Center.X);

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
            target.AddBuff(BuffID.Daybreak, 10 * 60);
            SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact);

            float DistanceFromCore = 20f;
            Player player = Main.player[Projectile.owner];
            float angleToCursor = (float)Math.Atan2(Main.MouseWorld.Y - player.Center.Y, Main.MouseWorld.X - player.Center.X);
            Vector2 offset = new Vector2(DistanceFromCore, 0f).RotatedBy(angleToCursor);

            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(Projectile.position + offset, Projectile.width, Projectile.height, DustID.SolarFlare, 0f, 0f, 150, default(Color), 1.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Set the projectile's velocity to zero
            Projectile.velocity = Vector2.Zero;
            return false;
        }

    }
}
