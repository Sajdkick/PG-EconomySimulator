using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGGamer : PGPlayer {

    public static new int GetChance()
    {
        return 92;
    }
    public static new string GetName()
    {
        return "Gamer";
    }

    public static int noNewLevelError = 0;

    List<int> playedLevels;
    int levelsPlayedPerMonth = 10;

    float skill;

    uint plays = 0;
    uint retries = 0;
    uint wins = 0;

    public PGGamer() {

        playedLevels = new List<int>();
        skill = Random.Range(0, 10.0f);

    }

    public override string GetInfo()
    {

        string info = GetName() + "\n";

        info += base.GetInfo() + "\n";

        info += "Skill: " + skill + ", Wins: " + wins + ", Retries: " + retries + ", Plays: " + plays + ", W/P: " + (float)wins / plays + "\n";
        info += "Levels played: " + playedLevels.Count + "\n";

        return info;

    }

    public override void ProcessHour()
    {

        if (Random.Range(0, 8 * 30) > 8 * 30 - levelsPlayedPerMonth)
        {

            int levelCount = PGWorld.instance.GetLevelCount();

            if (levelCount == 0)
            {

                noNewLevelError++;
                return;

            }

            if(levelCount == playedLevels.Count)
            {

                noNewLevelError++;
                int level = Random.Range(0, levelCount);
                Play(PGWorld.instance.GetLevel(level));
                return;

            }

            while (true) {

                int level = Random.Range(0, levelCount);

                if (!playedLevels.Contains(level))
                {

                    playedLevels.Add(level);
                    Play(PGWorld.instance.GetLevel(level));
                    return;

                }

            }

        }

    }

    public void Play(PGLevel level)
    {

        //A player can at most attempt a level 10 times in a row.
        for (int i = 0; i < 10; i++)
        {

            plays++;

            float attempt = 0;
            bool won = level.Play(this, out attempt);

            if (won)
            {
                wins++;
                break;
            }
            else
            {
                float retry = Random.Range(0, 20.0f) - Mathf.Abs(attempt); //If retry is bigger than 0, we try again!
                if (retry > 0)
                {

                    retries++;
                    continue;

                }
                else break;
            }

        }

    }

    public float GetSkill()
    {

        return skill;

    }

}
