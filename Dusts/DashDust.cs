using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace KatanaZERO.Dusts
{
    public class DashDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = 1f;
            dust.alpha = 100;
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 10, 10);
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, 1, 1, 1);

            dust.scale -= 0.0001f;
            if (dust.scale  < 0.9990f)
            {
                dust.alpha += 18;
            }

            if (dust.alpha > 240)
            {
                dust.active = false;
            }
            return false;
        }
    }
}