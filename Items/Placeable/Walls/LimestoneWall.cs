﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items.Placeable.Walls
{
    public class LimestoneWall : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall((ushort)WallType<LimestoneWallPlaced>());

            Item.ResearchUnlockCount = (400);
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemType<Blocks.Limestone>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class LimestoneWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ItemType<LimestoneWall>();
            AddMapEntry(new Color(34, 50, 30));

            DustType = DustType<Dusts.LimestoneDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }

    public class LimestoneWallNatural : ModWall
    {
        public override string Texture => "Polarities/Items/Placeable/Walls/LimestoneWallPlaced";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(34, 50, 30));

            DustType = DustType<Dusts.LimestoneDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}