using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGLevel {

    PGCreator creator;

    uint quality;
    uint playCount;

    float difficulty;
    List<float> highscore;
    float bestScore;

    public PGLevel(PGCreator _creator, uint _quality) {

        creator = _creator;
        quality = _quality;
        playCount = 0;

        difficulty = Random.Range(0, 10.0f);
        highscore = new List<float>();
        bestScore = float.MinValue;

    }
    public uint GetPlayCount()
    {

        return playCount;

    }

    public bool Play(PGGamer gamer, out float attempt)
    {

        playCount++;
        creator.GiveGold(3);

        uint reward = 5;

        float skillDifference = gamer.GetSkill() - difficulty; //The difference in skill and difficulty, ranges between 5 for very easy and -5 for extremelly hard.
        float roll = Random.Range(0, 20.0f);

        attempt = roll - (10 - skillDifference); //We roll a number between 0 and 10, if this number - our skill difference is positive, it's a successful attempt.

        bool won = attempt > 0;
        if (won)
        {

            reward += 15;

            //We add the attempt to the highscore list.
            highscore.Add(attempt);
            if (attempt > bestScore)
            {

                bestScore = attempt;
                if (highscore.Count >= 10) //You beat the highscore on a level with at least 10 plays.
                    reward += 50;
                else reward += 10; //You were one of the ten first to beat the level.

            }

        }

        gamer.GiveGold(reward);

        return won;

    }

}
