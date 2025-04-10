﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Polarities.NPCs.Eclipxie
{
    public class SolarCorona : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Solar Corona");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.alpha = 255;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 3f;
            Projectile.hide = true;
        }

        public override void AI()
        {
            NPC pixie = Main.npc[(int)Projectile.ai[0]];
            Projectile.rotation += Projectile.ai[1] * 0.1f / Projectile.scale;

            Projectile.scale = 2f * (float)(1 / 14f + Math.Sqrt(1 - (Projectile.timeLeft - 45f) * (Projectile.timeLeft - 45f) / (45f * 45f)));
            Projectile.alpha = (int)(170 * (1.5f - Math.Sqrt(1 - (Projectile.timeLeft - 45f) * (Projectile.timeLeft - 45f) / (45f * 45f))));

            Projectile.width = (int)(168 * Projectile.scale);
            Projectile.height = (int)(168 * Projectile.scale);
            DrawOffsetX = (int)(0.5f * (Projectile.height - 168));
            DrawOriginOffsetY = (int)(0.5f * (Projectile.height - 168));
            DrawOriginOffsetX = 0;
            Projectile.position = pixie.Center - Projectile.Hitbox.Size() / 2;
            if (!pixie.active) { Projectile.Kill(); }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 180, true);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float nearestX = Math.Max(targetHitbox.X, Math.Min(Projectile.Center.X, targetHitbox.X + targetHitbox.Size().X));
            float nearestY = Math.Max(targetHitbox.Y, Math.Min(Projectile.Center.Y, targetHitbox.Y + targetHitbox.Size().Y));
            return new Vector2(Projectile.Center.X - nearestX, Projectile.Center.Y - nearestY).Length() < Projectile.width / 2;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
    }
}
