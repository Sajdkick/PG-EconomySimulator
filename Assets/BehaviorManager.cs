using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorManager {

    public static class Enjoyment
    {

        public static int EnjoymentFromBeatingLevel(float playerSkill, float levelDifficulty, uint levelQuality, int numberOfAttempts)
        {

            float skillDifference = playerSkill - levelDifficulty;
            float qualityEnjoyment = BaseEnjoymentFromLevel(levelQuality);

            // The enjoyment of a level depends on the skill difference, the quality of the level, and the amount of tries it took.
            // If the skill difference is 0, you get the enjoyment times 1. If the skill difference is 10 or -10, you get times 0 and times 2 respectivly.
            // If the player tries multiple time, we assume they are enjoying it, and multiply the enjoyment.
            return (int)(qualityEnjoyment * ((10 - skillDifference) / 10) * (numberOfAttempts + 1));

        }

        public static int EnjoymentFromBeatingTheHighscore()
        {

            return 0;

        }

        public static int EnjoymentFromGold(int reward)
        {

            return 0;

        }

        public static float BaseEnjoymentFromLevel(uint levelQuality)
        {

            return Mathf.Pow(levelQuality, 2);

        }

        public static float BaseEnjoymentForAPositiveExperience()
        {

            // 2 * 2 because we want the player to atleast play a level of quality 2 to have seen the session as a positive experience.
            // We add the + 2 because we want the experience to be a bit more then just quality 2.
            return 2 * 2 + 2;

        }

    }

    public static class Play
    {

        public static bool WillPlayerRetry(float attempt)
        {

            float retry = Random.Range(0, 20.0f) - Mathf.Abs(attempt); //If retry is bigger than 0, we try again!
            return retry > 0;
            
        }

        public static bool DetermineIfWeBeatALevel(float playerSkill, float levelDifficulty, out float attempt)
        {

            float skillDifference = playerSkill - levelDifficulty; //The difference in skill and difficulty, ranges between 5 for very easy and -5 for extremelly hard.
            float roll = Random.Range(0, 20.0f);

            attempt = roll - (10 - skillDifference); //We roll a number between 0 and 10, if this number - our skill difference is positive, it's a successful attempt.

            return attempt > 0;

        }

    }
 
    public static class LevelCreation
    {

        public static uint DetermineQualityOfNewLevel()
        {

            uint quality = 0;
            int roll = Random.Range(0, 100);
            if (roll < 50)
                quality = 1; //These levels leave a poor impression on the player.
            else if (roll < 80)
                quality = 2; //These levels are good, but not that good. "Yeah it was okay"
            else if (roll < 95)
                quality = 3; //These levels are cool! "I really liked it, i might recommend it to someone"
            else
                quality = 4; //These are incredible, everyone has to see this!!!

            return quality;

        }

        public static float DetermineDifficultyOfNewLevel()
        {

            return Random.Range(0, 10.0f);

        }

    }

    public static class Session
    {

        public static bool StartASessionThisHourAndCreateALevel(int levelsPerMonth)
        {

            return Random.Range(0, 16 * 30) > 16 * 30 - (levelsPerMonth + 1);

        }

        public static bool StartASessionThisHourAndPlayALevel(float levelsPlayedPerMonth)
        {

            return Random.Range(0, 8 * 30) > 8 * 30 - levelsPlayedPerMonth;

        }

    }

    public static class Gamer
    {

        public static float DetermineInitialGamerSkill()
        {

            return Random.Range(0, 10.0f);

        }

    }

}
