﻿using Microsoft.Xna.Framework;
using Polarities.Dusts;
using Polarities.Items.Placeable.Blocks.Fractal;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Polarities.Items.Placeable.Furniture.Fractal
{
    public class FractalLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BrassLantern);
            Item.createTile = ModContent.TileType<FractalLanternTile>();
            Item.placeStyle = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FractalBrick>(), 6)
                .AddIngredient(ItemID.Torch)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class FractalLanternTile : LanternTileBase
    {
        public override int MyDustType => ModContent.DustType<FractalMatterDust>();
        public override int DropItem => ModContent.ItemType<FractalLantern>();
        public override Color LightColor => new Color(0.75f, 0.75f, 1f);
        public override bool DieInWater => false;
    }
}
