using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataExtractor {

    public virtual void Update(PGLevel level) { }
    public virtual void Update(PGPlayer player) { }

    public abstract float GetValue();

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
        public class RichestGamer : PlayerDataExtractor
        {

            int max = int.MinValue;

            public RichestGamer()
            {

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
    }
    namespace Level
    {

    }

}