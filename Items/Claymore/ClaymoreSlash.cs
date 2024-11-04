using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Player = Terraria.Player;

namespace KatanaZERO.Items.Claymore
{
    public class ClaymoreSlash : ModProjectile
    {
        private const float DistanceFromCore = 66f; //offset from the center of the player to the sprite
                                                    //like the empty space between a hydrogen atom core and its electron

        public override void SetDefaults()
        {
            Projectile.width = 178; //default 77
            Projectile.height = 184; //default 82
            Projectile.knockBack = 4;
            Projectile.aiStyle = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 35;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            

            Main.projFrames[Projectile.type] = 8;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (++Projectile.frameCounter >= 5) //animate slash
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            foreach (NPC target in Main.npc) //apply knockback to hit targets and give immunity
            {
                if (target.active && !target.friendly && Projectile.Hitbox.Intersects(target.Hitbox))
                {
                    player.immune = true;
                    player.immuneTime = 4;

                    if (!target.boss && KatanaZERO.enableVectorKnockback)
                    {
                        float angleToTarget = (float)Math.Atan2(target.Center.Y - player.Center.Y, target.Center.X - player.Center.X);
                        target.velocity = new Vector2((float)Math.Cos(angleToTarget), (float)Math.Sin(angleToTarget)) * Projectile.knockBack;
                    }
                }
            }

            foreach (Projectile p in Main.projectile) //deflect projectiles in contact with the hitbox
            {
                if (p.active && p.hostile && p.getRect().Intersects(Projectile.getRect()))
                {
                    p.hostile = false;
                    p.friendly = true;
                    p.velocity *= -2;
                    p.damage = Projectile.damage * 8;
                    SoundEngine.PlaySound(new SoundStyle("KatanaZERO/Sounds/bullet_deflect")
                    {
                        Volume = 0.5f,
                    });
                }
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = Vector2.Zero; //set projectile velocity to zero

            float angleToCursor = (float)Math.Atan2(Main.MouseWorld.Y - player.Center.Y, Main.MouseWorld.X - player.Center.X); //find angle between player and cursor

            Vector2 offset = new Vector2(DistanceFromCore, 0f).RotatedBy(angleToCursor); //make offset between player center and begining of sprite
            Projectile.position = player.Center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2); //position sprite with said offset

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
            target.AddBuff(BuffID.BrokenArmor, 20 * 60);
        }
    }
}
