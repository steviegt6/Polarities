using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items.Placeable.Walls
{
    public class SaltBrickWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (400);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall((ushort)WallType<SaltBrickWallPlaced>());
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemType<Blocks.SaltBrick>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class SaltBrickWallPlaced : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ItemType<SaltWall>();
            AddMapEntry(new Color(204, 165, 205));

            DustType = DustType<Dusts.SaltDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}