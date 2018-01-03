using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataExtractor {

    protected abstract void Init();

    public virtual void Update(PGLevel level) { }
    public virtual void Update(PGPlayer player) { }

    public abstract float GetValue();
    public virtual void PrepareForNewDay() { Init(); }

}

public abstract class PlayerDataExtractor : DataExtractor { }

namespace DataExtractors
{

    namespace Player
    {
        public class AverageGamerEnjoyment : PlayerDataExtractor
        {

            float totalEnjoyment;
            int playerCount;

            public AverageGamerEnjoyment()
            {
                Init();
            }
            protected override void Init()
            {
                totalEnjoyment = 0;
                playerCount = 0;
            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    totalEnjoyment += player.GetEnjoyment();
                    playerCount++;

                }

            }

            public override float GetValue()
            {

                return totalEnjoyment / playerCount;

            }

        }
        public class AverageEnjoymentDelta : PlayerDataExtractor
        {

            float totalEnjoyment;
            int playerCount;

            float oldTotalEnjoyment;
            int oldPlayerCount;

            public AverageEnjoymentDelta()
            {
                Init();
            }
            protected override void Init()
            {
                totalEnjoyment = 0;
                playerCount = 0;

                oldTotalEnjoyment = 0;
                oldPlayerCount = 0;

            }
            public override void PrepareForNewDay()
            {

                oldTotalEnjoyment = totalEnjoyment;
                oldPlayerCount = playerCount;

                totalEnjoyment = 0;
                playerCount = 0;

            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    totalEnjoyment += player.GetEnjoyment();
                    playerCount++;

                }

            }

            public override float GetValue()
            {

                if (playerCount == 0 || oldPlayerCount == 0)
                    return 0;

                return (totalEnjoyment / playerCount) - (oldTotalEnjoyment / oldPlayerCount);

            }

        }
        public class RichestGamer : PlayerDataExtractor
        {

            int max;

            public RichestGamer()
            {
                Init();
            }
            protected override void Init()
            {
                max = int.MinValue;
            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    if (player.GetGold() > max)
                        max = player.GetGold();

                }

            }

            public override float GetValue()
            {

                return max;

            }

        }
        public class PoorestGamer : PlayerDataExtractor
        {

            int min;

            public PoorestGamer()
            {
                Init();
            }
            protected override void Init()
            {
                min = int.MaxValue;
            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    if (player.GetGold() < min)
                        min = player.GetGold();

                }

            }

            public override float GetValue()
            {

                return min;

            }

        }

        public class AverageWinRate : PlayerDataExtractor
        {

            float totalWinRate;
            uint playerCount;

            public AverageWinRate()
            {
                Init();
            }
            protected override void Init()
            {
                totalWinRate = 0;
                playerCount = 0;
            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    PGGamer gamer = (PGGamer)player;

                    totalWinRate += gamer.GetWinRate();
                    playerCount++;

                }

            }

            public override float GetValue()
            {

                return totalWinRate / playerCount;

            }

        }
        public class AverageSkippedLevelCount : PlayerDataExtractor
        {

            float total;
            uint playerCount;

            public AverageSkippedLevelCount()
            {
                Init();
            }
            protected override void Init()
            {
                total = 0;
                playerCount = 0;
            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    PGGamer gamer = (PGGamer)player;

                    total += gamer.GetSkippedLevelCount();
                    playerCount++;

                }

            }

            public override float GetValue()
            {

                return total / playerCount;

            }

        }
        public class AveragePlays : PlayerDataExtractor
        {

            float total;
            uint playerCount;

            public AveragePlays()
            {
                Init();
            }
            protected override void Init()
            {
                total = 0;
                playerCount = 0;
            }

            public override void Update(PGPlayer player)
            {

                if ((string)player.GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                {

                    PGGamer gamer = (PGGamer)player;

                    total += gamer.GetPlays();
                    playerCount++;

                }

            }

            public override float GetValue()
            {

                return total / playerCount;

            }

        }

    }
    namespace Level
    {
        public class AveragePlays : PlayerDataExtractor
        {

            uint totalPlays;
            int levelCount;

            public AveragePlays()
            {
                Init();
            }
            protected override void Init()
            {
                totalPlays = 0;
                levelCount = 0;
            }

            public override void Update(PGLevel level)
            {

                totalPlays += level.GetPlayCount();
                levelCount++;

            }

            public override float GetValue()
            {

                return (float)totalPlays / levelCount;

            }

        }
        public class NumberOfQuality1 : PlayerDataExtractor
        {

            int levelCount;

            public NumberOfQuality1()
            {
                Init();
            }
            protected override void Init()
            {
                levelCount = 0;
            }

            public override void Update(PGLevel level)
            {

                if (level.GetQuality() <= 1)
                    levelCount++;

            }

            public override float GetValue()
            {

                return levelCount;

            }

        }
        public class NumberOfQuality2 : PlayerDataExtractor
        {

            int levelCount;

            public NumberOfQuality2()
            {
                Init();
            }
            protected override void Init()
            {
                levelCount = 0;
            }

            public override void Update(PGLevel level)
            {

                if (level.GetQuality() > 1 && level.GetQuality() <= 2)
                    levelCount++;

            }

            public override float GetValue()
            {

                return levelCount;

            }

        }
        public class NumberOfQuality3 : PlayerDataExtractor
        {

            int levelCount;

            public NumberOfQuality3()
            {
                Init();
            }
            protected override void Init()
            {
                levelCount = 0;
            }

            public override void Update(PGLevel level)
            {

                if (level.GetQuality() > 2 && level.GetQuality() <= 3)
                    levelCount++;

            }

            public override float GetValue()
            {

                return levelCount;

            }
        }
        public class NumberOfQuality4 : PlayerDataExtractor
        {

            int levelCount;

            public NumberOfQuality4()
            {
                Init();
            }
            protected override void Init()
            {
                levelCount = 0;
            }

            public override void Update(PGLevel level)
            {

                if (level.GetQuality() > 3 && level.GetQuality() <= 4)
                    levelCount++;

            }

            public override float GetValue()
            {

                return levelCount;

            }
        }
    }

}