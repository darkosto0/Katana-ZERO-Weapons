using Terraria;
using Terraria.ModLoader;

namespace KatanaZERO.Dusts
{
    class FifteenDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = 1f;
        }

        public override bool Update(Dust dust)
        {
            dust.scale -= 0.001f;
            if (dust.scale <0.999f)
            {
                dust.active = false;
            }
            else
            {
                Lighting.AddLight(dust.position, 0.25f, 1.02f, 1.02f);
            }

            dust.noGravity = true;
            return false;
        }
    }
}