using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGGamer : PGPlayer {

    public static new int GetChance()
    {
        return 92;
    }

    public static string name = "Gamer";
    public static new string GetName()
    {
        return name;
    }

    public class LevelGamerData
    {

        public LevelGamerData(PGLevel _level)
        {
            level = _level;
            beaten = false;
            plays = 0;
            bestAttempt = -10;
        }

        public PGLevel level;
        public bool beaten;
        public int plays;
        public float bestAttempt;

    }

    List<LevelGamerData> playedLevels;
    int levelsPlayedPerMonth = 10;

    float skill;

    uint plays = 0;
    uint retries = 0;
    uint wins = 0;
    uint skippedLevel = 0;

    public PGGamer() {

        playedLevels = new List<LevelGamerData>();
        skill = BehaviorManager.Gamer.DetermineInitialGamerSkill();

    }

    public override string GetInfo()
    {

        string info = GetName() + "\n";

        info += base.GetInfo() + "\n";

        info += "Skill: " + skill + ", Wins: " + wins + ", Retries: " + retries + ", Plays: " + plays + ", W/P: " + (float)wins / plays + "\n";
        info += "Levels beaten: " + playedLevels.Count + "\n";
        info += "Enjoyment: " + enjoyment + "\n";

        return info;

    }

    public override void ProcessHour()
    {

        if (BehaviorManager.Session.StartASessionThisHourAndPlayALevel(levelsPlayedPerMonth))
        {

            int levelCount = PGWorld.instance.GetLevelCount();

            //There are no levels.
            if (levelCount != 0)
            {

                SelectLevel();

            }

            // We subtract the base enjoyment, so if we played a level and enjoyed at just as much as the base enjoyment our enjoyment hasn't changed.
            enjoyment -= BehaviorManager.Enjoyment.BaseEnjoymentForAPositiveExperience();

        }

    }

    public void SelectLevel()
    {

        for (int i = 4; i >= 0; i--)
        {

            int levelCount = PGWorld.instance.levelList[i].Count;
            bool unplayed = false;
            for (int j = 0; j < levelCount; j++)
            {

                PGLevel level = PGWorld.instance.levelList[i][j];
                LevelGamerData levelGamerData = null;
                for (int k = 0; k < playedLevels.Count; k++)
                {

                    if (playedLevels[k].level == level)
                    {

                        levelGamerData = playedLevels[k];
                        break;

                    }

                }
                if (levelGamerData == null)
                {

                    unplayed = true;
                    levelGamerData = new LevelGamerData(level);

                }

                float chanceToPlayLevel = BehaviorManager.Gamer.ChanceToPlayLevel(levelGamerData.plays, levelGamerData.beaten, skill, levelGamerData.bestAttempt, levelGamerData.level.GetDifficulty(), levelGamerData.level.GetQuality());

                if (Random.Range(0, 100.0f) < chanceToPlayLevel)
                {

                    Play(levelGamerData);

                    if(unplayed)
                        playedLevels.Add(levelGamerData);

                    return;
                }

                skippedLevel++;
                enjoyment += BehaviorManager.Enjoyment.BaseEnjoymentForNotChoosingLevel();

            }

        }

    }

    public void Play(LevelGamerData levelGamerData)
    {

        PGLevel level = levelGamerData.level;

        //A player can at most attempt a level 10 times in a row.
        for (int i = 0; i < 10; i++)
        {

            levelGamerData.plays++;
            plays++;

            float attempt = 0;
            bool won = level.Play(this, out attempt);

            if (attempt > levelGamerData.bestAttempt)
                levelGamerData.bestAttempt = attempt;

            if (won)
            {

                enjoyment += BehaviorManager.Enjoyment.EnjoymentFromBeatingLevel(skill, level.GetDifficulty(), level.GetQuality(), i);
                levelGamerData.beaten = true;
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
    public float GetWinRate()
    {

        if (plays == 0)
            return 0;

        return (float)wins / plays;

    }
    public uint GetSkippedLevelCount()
    {

        return skippedLevel;

    }
    public uint GetPlays()
    {

        return plays;

    }

}
