﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items.Placeable.Furniture
{
	public abstract class PianoBase : ModItem
	{
		public abstract int PlaceTile { get; }

		public override void SetStaticDefaults()
		{
			SacrificeTotal = (1);
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(PlaceTile);
			Item.value = Item.sellPrice(copper: 60);
		}
	}

	public abstract class PianoTileBase : ModTile
	{
		public abstract int MyDustType { get; }
		public abstract int DropItem { get; }
		public virtual bool DieInLava => true;
		public virtual Color MapColor => new Color(191, 142, 111);

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = DieInLava;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			AddMapEntry(MapColor, Lang.GetItemName(ItemID.Piano));

			DustType = MyDustType;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Pianos };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, DropItem);
		}
	}
}