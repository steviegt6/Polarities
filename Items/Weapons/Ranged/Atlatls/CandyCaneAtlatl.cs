﻿using Microsoft.Xna.Framework;
using Polarities.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Items.Weapons.Ranged.Atlatls
{
    public class CandyCaneAtlatl : AtlatlBase
    {
        public override Vector2[] ShotDistances => new Vector2[] { new Vector2(28) };

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(71, 3, 0);
            Item.DamageType = DamageClass.Ranged;

            Item.width = 40;
            Item.height = 36;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.shoot = 10;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Dart;

            Item.value = Item.sellPrice(gold: 9);
            Item.rare = ItemRarityID.Yellow;
        }

        public override float UseSpeedMultiplier(Player player)
        {
            return 30f / (30f - player.GetModPlayer<PolaritiesPlayer>().candyCaneAtlatlBoost / 40f);
        }

        public override bool RealShoot(Player player, EntitySource_ItemUse_WithAmmo source, int index, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            float offsetAmount = Main.rand.NextFloat(-1f, 1f) * player.GetModPlayer<PolaritiesPlayer>().candyCaneAtlatlBoost / 3000f;
            Main.projectile[Projectile.NewProjectile(source, position, velocity.RotatedBy(offsetAmount), type, damage, knockback, player.whoAmI)].GetGlobalProjectile<PolaritiesProjectile>().candyCaneAtlatl = true;

            return false;
        }
    }
}