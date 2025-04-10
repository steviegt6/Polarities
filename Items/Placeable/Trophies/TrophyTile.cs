﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items.Placeable.Trophies
{
    public class TrophyTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.FramesOnKillWall[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddMapEntry(new Color(120, 85, 60), CreateMapEntryName());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 54;
            int itemType = TrophyBase.trophyIndexToItemType[style];
            if (itemType != 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, itemType);
            }
        }
    }

    public abstract class TrophyBase : ModItem
    {
        public abstract int TrophyIndex { get; }

        public static Dictionary<int, int> trophyIndexToItemType = new Dictionary<int, int>();

        public override void Unload()
        {
            trophyIndexToItemType = null;
        }

        public override void SetStaticDefaults()
        {
            trophyIndexToItemType.Add(TrophyIndex, Type);

            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType<TrophyTile>(), TrophyIndex);

            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.value = 50000;
            Item.rare = ItemRarityID.Blue;
        }
    }

    public class StormCloudfishTrophy : TrophyBase { public override int TrophyIndex => 1; }
    public class SunPixieTrophy : TrophyBase { public override int TrophyIndex => 2; }
    public class GigabatTrophy : TrophyBase { public override int TrophyIndex => 4; }
    public class StarConstructTrophy : TrophyBase { public override int TrophyIndex => 5; }
    public class EsophageTrophy : TrophyBase { public override int TrophyIndex => 6; }
    public class ConvectiveWandererTrophy : TrophyBase { public override int TrophyIndex => 10; }
}

