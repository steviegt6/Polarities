﻿using Microsoft.Xna.Framework;
using Polarities.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace Polarities.NPCs.Critters
{
    //TODO: Group with critters in the bestiary if it's even possible
    public class Sunfish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.CountsAsCritter[Type] = true;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    BuffID.Venom,
                    BuffID.BoneJavelin,
                    BuffType<Desiccating>(),
                }
            };
            NPCID.Sets.DebuffImmunitySets/* tModPorter Removed: See the porting notes in https://github.com/tModLoader/tModLoader/pull/3453 */.Add(Type, debuffData);

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitPositionXOverride = 0,
                Position = new Vector2(10, 0),
                SpriteDirection = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);

            PolaritiesNPC.bestiaryCritter[Type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                //spawn conditions
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				//flavor text
				this.TranslatedBestiaryEntry(),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 48;
            DrawOffsetY = 32;

            NPC.aiStyle = 16;
            AIType = NPCID.Goldfish;

            NPC.defense = 20;
            NPC.lifeMax = 2000;
            NPC.chaseable = false;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
        }

        public override bool PreAI()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && Main.player[i].Center.Distance(NPC.Center) < 40)
                {
                    Main.player[i].AddBuff(BuffType<SunfishBuff>(), 60 * 60 * 5);
                }
            }

            NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;

            return true;
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen += 20;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 12)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = (NPC.frame.Y + frameHeight) % (12 * frameHeight);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.Ocean.Chance * 0.2f;
            }
            else
            {
                return 0f;
            }
        }
        public override bool CheckDead()
        {
            for (int i = 1; i <= 5; i++)
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SunfishGore" + i).Type);
            return true;
        }
    }

    public class SunfishBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 4;
            player.GetModPlayer<PolaritiesPlayer>().spawnRate *= 0.75f;
        }
    }
}
