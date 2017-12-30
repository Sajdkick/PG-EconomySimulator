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
        skill = BehaviorManager.Gamer.DetermineInitialGamerSkill();

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

        if (BehaviorManager.Session.StartASessionThisHourAndPlayALevel(levelsPlayedPerMonth))
        {

            int levelCount = PGWorld.instance.GetLevelCount();

            if (levelCount == 0)
            {

                noNewLevelError++;

            }
            else if(levelCount == playedLevels.Count)
            {

                noNewLevelError++;
                int level = Random.Range(0, levelCount);
                Play(PGWorld.instance.GetLevel(level));
                
            }
            else
            {

                while (true) {

                    int level = Random.Range(0, levelCount);

                    if (!playedLevels.Contains(level))
                    {

                        playedLevels.Add(level);
                        Play(PGWorld.instance.GetLevel(level));
                        break;

                    }

                }

            }

            // We subtract the base enjoyment, so if we played a level and enjoyed at just as much as the base enjoyment our enjoyment hasn't changed.
            enjoyment -= BehaviorManager.Enjoyment.BaseEnjoymentForAPositiveExperience();

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

                enjoyment += BehaviorManager.Enjoyment.EnjoymentFromBeatingLevel(skill, level.GetDifficulty(), level.GetQuality(), i);
                wins++;
                break;

            }
            else
            {
                if (BehaviorManager.Play.WillPlayerRetry(attempt))
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
