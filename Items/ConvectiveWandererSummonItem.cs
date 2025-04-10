﻿using Polarities.Biomes;
using Polarities.NPCs.ConvectiveWanderer;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items
{
    public class ConvectiveWandererSummonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(Sounds.ConvectiveBabyDeath, player.position);
            if (!NPC.AnyNPCs(NPCType<ConvectiveWanderer>()) && player.InModBiome(GetInstance<LavaOcean>()) && PolaritiesSystem.convectiveWandererSpawnTimer == 0) PolaritiesSystem.convectiveWandererSpawnTimer = 1;
            return true;
        }
    }
}