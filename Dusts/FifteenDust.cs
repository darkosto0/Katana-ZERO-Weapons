using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;


namespace KatanaZERO.Dusts
{
    class FifteenDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = 0.8f;
            dust.noGravity = true;
	        dust.frame = new Rectangle(0, 0, 8, 8);
        }

        public override bool Update(Dust dust)
        {
            Player player = Main.player[Player.FindClosest(dust.position, 0, 0)];

            float manaRatio = Math.Clamp((player.statMana - 2f) / (float)player.statManaMax2, 0f, 1f);

            dust.scale -= 0.001f;
            if (dust.scale < 0.799f)
            {
                dust.active = false;
            }
            else
            {
                Lighting.AddLight(dust.position, 0.25f * manaRatio, 1.02f * manaRatio, 1.02f * manaRatio);
            }

            return false;
        }
    }
}