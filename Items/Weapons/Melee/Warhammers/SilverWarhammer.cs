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

namespace Polarities.Items.Weapons.Melee.Warhammers
{
	public class SilverWarhammer : WarhammerBase
	{
		public override int HammerLength => 53;
		public override int HammerHeadSize => 13;
		public override int DefenseLoss => 6;
		public override int DebuffTime => 540;

		public override void SetDefaults()
		{
			Item.SetWeaponValues(18, 11, 0);
			Item.DamageType = DamageClass.Melee;

			Item.width = 66;
			Item.height = 66;

			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = WarhammerUseStyle;

			Item.value = Item.sellPrice(silver: 15);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SilverBar, 12)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}