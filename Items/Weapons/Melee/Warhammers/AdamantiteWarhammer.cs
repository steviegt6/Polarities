﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items.Weapons.Melee.Warhammers
{
    public class AdamantiteWarhammer : WarhammerBase
    {
        public override int HammerLength => 99;
        public override int HammerHeadSize => 28;
        public override int DefenseLoss => 24;
        public override int DebuffTime => 1200;

        public override void SetDefaults()
        {
            Item.SetWeaponValues(70, 16f, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 128;
            Item.height = 128;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ProjectileType<AdamantiteWarhammerProjectile>()] < 1;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Thrust;
                Item.shoot = ProjectileType<AdamantiteWarhammerProjectile>();
                Item.shootSpeed = 2.5f;
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useStyle = WarhammerUseStyle;
                Item.shoot = 0;
                Item.shootSpeed = 0f;
                Item.noMelee = false;
                Item.noUseGraphic = false;
            }
            return null;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage /= 2;
            knockback /= 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.altFunctionUse == 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }


    public class AdamantiteWarhammerProjectile : ModProjectile
    {
        public override string Texture => "Polarities/Items/Weapons/Melee/Warhammers/AdamantiteWarhammer";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("{$Mods.Polarities.ItemName.AdamantiteWarhammer}");
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;
            DrawOffsetX = 29 * 2 - 64 * 2;
            DrawOriginOffsetY = 0;
            DrawOriginOffsetX = 64 - 29;

            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public float movementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.Center = ownerMountedCenter;
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }

            Projectile.position += Projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }
}