﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.NPCs.Enemies;
using Polarities.NPCs.Enemies.Fractal;
using Polarities.NPCs.Enemies.Fractal.PostSentinel;
using Polarities.NPCs.Enemies.Granite;
using Polarities.NPCs.Enemies.HallowInvasion;
using Polarities.NPCs.Enemies.LavaOcean;
using Polarities.NPCs.Enemies.Limestone;
using Polarities.NPCs.Enemies.Marble;
using Polarities.NPCs.Enemies.Salt;
using Polarities.NPCs.Enemies.WorldEvilInvasion;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items.Placeable.Banners
{
    public class BannerTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddMapEntry(new Color(13, 88, 130), CreateMapEntryName());
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            //sway in wind
            bool intoRenderTargets = true;
            bool flag = intoRenderTargets || Main.LightingEveryFrame;
            if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 54 == 0 && flag)
            {
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, 5);
            }

            return false;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 18;
            int itemType = BannerBase.BannerIndexToItemType(style);
            if (itemType != 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, itemType);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                int style = Main.tile[i, j].TileFrameX / 18;
                int npcType = BannerBase.BannerIndexToNPCType(style);
                if (npcType != 0)
                {
                    int bannerItem = NPCLoader.GetNPC(npcType).BannerItem;
                    if (ItemID.Sets.BannerStrength.IndexInRange(bannerItem) && ItemID.Sets.BannerStrength[bannerItem].Enabled)
                    {
                        Main.SceneMetrics.NPCBannerBuff[npcType] = true;
                        Main.SceneMetrics.hasBanner = true;
                    }
                }
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }

    public abstract class BannerBase : ModItem
    {
        public abstract int BannerIndex { get; }
        public abstract int NPCType { get; }
        public virtual int BannerKills => 50;

        private static Dictionary<int, int> bannerIndexToNPCType = new Dictionary<int, int>();

        public override void Unload()
        {
            bannerIndexToNPCType = null;
        }

        public override void SetStaticDefaults()
        {
            bannerIndexToNPCType.Add(BannerIndex, NPCType);

            Item.ResearchUnlockCount = (1);

            string npcKey = "{$Mods.Polarities.NPCName." + NPCLoader.GetNPC(NPCType).Name + "}";
            // DisplayName.SetDefault(npcKey + "{$Mods.Polarities.ItemName.BannerBase}");
            // Tooltip.SetDefault("{$CommonItemTooltip.BannerBonus}" + npcKey);

            ItemID.Sets.KillsToBanner[Type] = BannerKills;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 28;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
            Item.createTile = TileType<BannerTile>();

            Item.placeStyle = BannerIndex;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine tooltip in tooltips)
            {
                if (tooltip.Name.Equals("Tooltip0"))
                {
                    tooltip.Text = tooltip.Text.Replace("{NPCName}", Language.GetTextValue("Mods.Polarities.NPCName." + NPCLoader.GetNPC(NPCType).Name));
                    break;
                }
            }
        }

        public static int BannerIndexToNPCType(int index)
        {
            if (bannerIndexToNPCType.ContainsKey(index))
            {
                return bannerIndexToNPCType[index];
            }
            else
            {
                return 0;
            }
        }

        public static int BannerIndexToItemType(int index)
        {
            return NPCLoader.GetNPC(BannerIndexToNPCType(index)).BannerItem;
        }
    }

    public class TurbulenceSparkBanner : BannerBase { public override int BannerIndex => 42; public override int NPCType => NPCType<TurbulenceSpark>(); }
    public class SparkCrawlerBanner : BannerBase { public override int BannerIndex => 15; public override int NPCType => NPCType<SparkCrawler>(); }
    public class ShockflakeBanner : BannerBase { public override int BannerIndex => 36; public override int NPCType => NPCType<Shockflake>(); }
    public class SeaAnomalyBanner : BannerBase { public override int BannerIndex => 39; public override int NPCType => NPCType<SeaAnomaly>(); }
    public class OrthoconicBanner : BannerBase { public override int BannerIndex => 32; public override int NPCType => NPCType<Orthoconic>(); }
    public class MegaMengerBanner : BannerBase { public override int BannerIndex => 21; public override int NPCType => NPCType<MegaMenger>(); }
    public class FractalSpiritBanner : BannerBase { public override int BannerIndex => 23; public override int NPCType => NPCType<FractalSpirit>(); }
    public class FractalSlimeBanner : BannerBase { public override int BannerIndex => 40; public override int NPCType => NPCType<FractalSlimeSmall>(); }
    public class FractalPointBanner : BannerBase { public override int BannerIndex => 25; public override int NPCType => NPCType<FractalPoint>(); }
    public class FractalFernBanner : BannerBase { public override int BannerIndex => 16; public override int NPCType => NPCType<FractalFern>(); }
    public class EuryopterBanner : BannerBase { public override int BannerIndex => 17; public override int NPCType => NPCType<Euryopter>(); }
    public class DustSpriteBanner : BannerBase { public override int BannerIndex => 41; public override int NPCType => NPCType<DustSprite>(); }
    public class ChaosCrawlerBanner : BannerBase { public override int BannerIndex => 38; public override int NPCType => NPCType<ChaosCrawler>(); }
    public class BisectorBanner : BannerBase { public override int BannerIndex => 37; public override int NPCType => NPCType<BisectorHead>(); }
    public class AmphisbaenaBanner : BannerBase { public override int BannerIndex => 24; public override int NPCType => NPCType<Amphisbaena>(); }
    public class SpitterBanner : BannerBase { public override int BannerIndex => 0; public override int NPCType => NPCType<Spitter>(); }
    public class RattlerBanner : BannerBase { public override int BannerIndex => 1; public override int NPCType => NPCType<Rattler>(); }
    public class BrineFlyBanner : BannerBase { public override int BannerIndex => 2; public override int NPCType => NPCType<BrineFly>(); public override int BannerKills => 200; }
    public class SalthopperBanner : BannerBase { public override int BannerIndex => 3; public override int NPCType => NPCType<Salthopper>(); }
    public class MusselBanner : BannerBase { public override int BannerIndex => 4; public override int NPCType => NPCType<Mussel>(); }
    public class InfernalArrowBanner : BannerBase { public override int BannerIndex => 5; public override int NPCType => NPCType<InfernalArrow>(); } //TODO: This banner needs a resprite
    public class GraniteStomperBanner : BannerBase { public override int BannerIndex => 6; public override int NPCType => NPCType<GraniteStomper>(); }
    public class ZombatBanner : BannerBase { public override int BannerIndex => 7; public override int NPCType => NPCType<Zombat>(); }
    public class BloodBatBanner : BannerBase { public override int BannerIndex => 8; public override int NPCType => NPCType<BloodBat>(); }
    public class StalagBeetleBanner : BannerBase { public override int BannerIndex => 9; public override int NPCType => NPCType<StalagBeetle>(); }
    public class AlkaliSpiritBanner : BannerBase { public override int BannerIndex => 10; public override int NPCType => NPCType<AlkaliSpirit>(); }
    public class LimeshellBanner : BannerBase { public override int BannerIndex => 11; public override int NPCType => NPCType<Limeshell>(); }
    public class ConeShellBanner : BannerBase { public override int BannerIndex => 12; public override int NPCType => NPCType<ConeShell>(); }
    public class KrakenBanner : BannerBase { public override int BannerIndex => 13; public override int NPCType => NPCType<Kraken>(); }
    public class SeaSerpentBanner : BannerBase { public override int BannerIndex => 14; public override int NPCType => NPCType<SeaSerpent>(); }
    public class StellatedSlimeBanner : BannerBase { public override int BannerIndex => 18; public override int NPCType => NPCType<StellatedSlime>(); }
    public class BatSlimeBanner : BannerBase { public override int BannerIndex => 19; public override int NPCType => NPCType<BatSlime>(); }
    public class NestGuardianBanner : BannerBase { public override int BannerIndex => 20; public override int NPCType => NPCType<NestGuardian>(); }
    public class AlkalabominationBanner : BannerBase { public override int BannerIndex => 22; public override int NPCType => NPCType<Alkalabomination>(); }
    public class LightEaterBanner : BannerBase { public override int BannerIndex => 26; public override int NPCType => NPCType<LightEater>(); }
    public class TendrilAmalgamBanner : BannerBase { public override int BannerIndex => 27; public override int NPCType => NPCType<TendrilAmalgam>(); }
    public class CrimagoBanner : BannerBase { public override int BannerIndex => 28; public override int NPCType => NPCType<Crimago>(); }
    public class UraraneidBanner : BannerBase { public override int BannerIndex => 29; public override int NPCType => NPCType<Uraraneid>(); }
    public class LivingSpineBanner : BannerBase { public override int BannerIndex => 30; public override int NPCType => NPCType<LivingSpine>(); }
    public class RavenousCursedBanner : BannerBase { public override int BannerIndex => 31; public override int NPCType => NPCType<RavenousCursed>(); }
    public class HydraBanner : BannerBase { public override int BannerIndex => 33; public override int NPCType => NPCType<HydraBody>(); }
    public class FlowWormBanner : BannerBase { public override int BannerIndex => 34; public override int NPCType => NPCType<FlowWorm>(); }
    public class GlowWormBanner : BannerBase { public override int BannerIndex => 35; public override int NPCType => NPCType<GlowWorm>(); }
    public class IlluminantScourerBanner : BannerBase { public override int BannerIndex => 43; public override int NPCType => NPCType<IlluminantScourer>(); }
    public class PegasusBanner : BannerBase { public override int BannerIndex => 44; public override int NPCType => NPCType<Pegasus>(); }
    public class SunServitorBanner : BannerBase { public override int BannerIndex => 45; public override int NPCType => NPCType<SunServitor>(); }
    public class SunKnightBanner : BannerBase { public override int BannerIndex => 46; public override int NPCType => NPCType<SunKnight>(); }
    public class TrailblazerBanner : BannerBase { public override int BannerIndex => 47; public override int NPCType => NPCType<Trailblazer>(); }
    public class PainbowBanner : BannerBase { public override int BannerIndex => 48; public override int NPCType => NPCType<Painbow>(); }
    public class AequoreanBanner : BannerBase { public override int BannerIndex => 49; public override int NPCType => NPCType<Aequorean>(); }
    public class BrineDwellerBanner : BannerBase { public override int BannerIndex => 50; public override int NPCType => NPCType<BrineDweller>(); }
    public class GraniteCrawlerBanner : BannerBase { public override int BannerIndex => 51; public override int NPCType => NPCType<GraniteCrawler>(); }
    public class SlimeyBanner : BannerBase { public override int BannerIndex => 52; public override int NPCType => NPCType<Slimey>(); }
    public class MantleOWarBanner : BannerBase { public override int BannerIndex => 53; public override int NPCType => NPCType<MantleOWar>(); }
}