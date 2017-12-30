using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGLevel {

    PGCreator creator;

    uint playCount;

    uint quality;
    float difficulty;

    List<float> highscore;
    float bestScore;

    public PGLevel(PGCreator _creator, uint _quality, float _difficulty) {

        creator = _creator;
        quality = _quality;
        difficulty = _difficulty;
        playCount = 0;

        highscore = new List<float>();
        bestScore = float.MinValue;

    }
    public uint GetPlayCount()
    {

        return playCount;

    }
    public float GetDifficulty()
    {

        return difficulty;

    }
    public uint GetQuality()
    {

        return quality;

    }

    public bool Play(PGGamer gamer, out float attempt)
    {

        playCount++;
        creator.ChangeGold(3);

        int reward = 5;

        bool won = BehaviorManager.Play.DetermineIfWeBeatALevel(gamer.GetSkill(), difficulty, out attempt);

        if (won)
        {

            reward += 15;

            //We add the attempt to the highscore list.
            highscore.Add(attempt);
            if (attempt > bestScore)
            {

                gamer.ChangeEnjoyment(BehaviorManager.Enjoyment.EnjoymentFromBeatingTheHighscore());

                bestScore = attempt;
                if (highscore.Count >= 10) //You beat the highscore on a level with at least 10 plays.
                    reward += 50;
                else reward += 10; //You were one of the ten first to beat the level.

            }

        }

        gamer.ChangeGold(reward);

        return won;

    }

}
