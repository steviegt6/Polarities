﻿using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Items
{
    public class GigabatSummonItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);

            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 16;
            Item.rare = 1;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 4;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCType<NPCs.Gigabat.Gigabat>()) && (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight);
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCType<NPCs.Gigabat.Gigabat>());
            SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/NPC_Killed_4")
            {
                Volume = 2f,
                Pitch = -8f
            }, player.position);
            return true;
        }
    }
}