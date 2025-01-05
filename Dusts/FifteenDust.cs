using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


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
            dust.scale -= 0.001f;
            if (dust.scale < 0.799f)
            {
                dust.active = false;
            }
            else
            {
                Lighting.AddLight(dust.position, 0.25f, 1.02f, 1.02f);
            }
            return false;
        }
    }
}