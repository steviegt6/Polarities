﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.NPCs;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Polarities.Items.Weapons.Melee.Warhammers
{
	public class HaliteWarhammer : WarhammerBase
	{
		public override int HammerLength => 55;
		public override int HammerHeadSize => 13;
		public override int DefenseLoss => 8;
		public override int DebuffTime => 600;

        public override void SetDefaults()
		{
			Item.SetWeaponValues(18, 12, 0);
			Item.DamageType = DamageClass.Melee;

			Item.width = 68;
			Item.height = 68;

			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = WarhammerUseStyle;

			Item.value = Item.sellPrice(silver: 20);
			Item.rare = ItemRarityID.Blue;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffType<Buffs.Desiccating>(), 300);

			base.OnHitNPC(player, target, damage, knockBack, crit);
        }

        public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemType<Items.Placeable.SaltCrystals>(), 16)
				.AddIngredient(ItemType<Items.Placeable.Blocks.Salt>(), 8)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}

