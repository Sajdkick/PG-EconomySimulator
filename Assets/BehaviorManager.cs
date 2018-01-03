using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorManager {

    public static class Enjoyment
    {

        public static float EnjoymentOfNotFindingALevelToPlay()
        {

            return -BaseEnjoymentForAPositiveExperience() * 10;

        }

        public static float EnjoymentFromBeatingLevel(float playerSkill, float levelDifficulty, float levelQuality, float numberOfAttempts)
        {

            float skillDifference = playerSkill - levelDifficulty;
            float qualityEnjoyment = BaseEnjoymentFromLevel(levelQuality);

            // The enjoyment of a level depends on the skill difference, the quality of the level, and the amount of tries it took.
            // If the skill difference is 0, you get the enjoyment times 1. If the skill difference is 10 or -10, you get times 0 and times 2 respectivly.
            // If the player tries multiple time, we assume they are enjoying it, and multiply the enjoyment.
            return (qualityEnjoyment * ((10 - skillDifference) / 10.0f) * (numberOfAttempts + 1));

        }

        public static float EnjoymentFromBeatingAHighscore(float playsOnLevel, float mostPlays)
        {

            //We want the enjoyment to scale depending on how popular the level is, which is based to how close the play count is to that of the most played level.

            float enjoyment = 9; //Base of 9, that of a good level.

            // An exponential curve, that makes it more exciting the closer the level is to the most popular one.
            float popularFactor = Mathf.Pow(playsOnLevel / mostPlays, 3);

            // If the level has less then 1000 plays, it's not as exciting. But all above 1000 plays feel the same.
            // Makes sure that you wont get super excited if you get the highscore on the most popular level, even though the most popular level hasn't got that many plays.
            float playFactor = (Mathf.Clamp(mostPlays, 0, 1000) / 1000);

            // The most enjoyment you can get from beating a highscore
            float enjoymentPeak = 100 * BaseEnjoymentForAPositiveExperience();

            // We create an exponential curve so that the more plays, the more exciting. If we just beat the most popular level
            enjoyment += popularFactor * playFactor * enjoymentPeak;

            return enjoyment;

        }

        public static float BaseEnjoymentFromLevel(float levelQuality)
        {
            return Mathf.Pow(levelQuality, 2);
        }

        public static float BaseEnjoymentForAPositiveExperience()
        {
            // 2 * 2 because we want the player to atleast play a level of quality 2 to have seen the session as a neutral, not good and not bad.
            // We add the + 2 because we want the experience to be a bit positive.
            return 2 * 2 + 2;
        }

        public static float BaseEnjoymentForNotChoosingLevel()
        {
            return 2;
        }

        public static float GoldToEnjoyment(int gold)
        {

            return 0;// gold / 10.0f;

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
            if (roll < 5)
                quality = 4; //These levels leave a poor impression on the player.
            else if (roll < 20)
                quality = 3; //These levels are good, but not that good. "Yeah it was okay"
            else if (roll < 50)
                quality = 2; //These levels are cool! "I really liked it, i might recommend it to someone"
            else
                quality = 1; //These are incredible, everyone has to see this!!!

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

        public static float ChanceToPlayLevel(int plays, bool beaten, float playerSkill, float bestAttempt, float levelDifficulty, float levelQuality)
        {
            
            // 100% chance to play is a level where, plays = 0, beaten = false, playerSkill = levelDifficulty, levelQuality = 4.

            float chance = 0;

            //If we've never played it
            if (plays == 0)
            {

                // We add 40 percent because we haven't played it.
                chance += 40;

                //We add 10 percent if it's right on our skill level.
                float skillDifference = Mathf.Abs(playerSkill - levelDifficulty);
                chance += 10 * (10 - skillDifference) / 10.0f;

                //We add 50 percent if it's of the best quality.
                chance += Mathf.Pow(levelQuality / 4.0f, 3) * 4 * 12.5f;

            }
            else if (!beaten)
            {

                // If we have less then a 100 attempts, we add 20 percent.
                if(plays < 100)
                    chance += 20;

                // If our best attempt was very close to beating it, we add 30 percent.
                chance += (1 - (Mathf.Abs(bestAttempt) / 20.0f)) * 30;

                //We add 50 percent if it's of the best quality.
                chance += Mathf.Pow(levelQuality / 4.0f, 3) * 4 * 12.5f;

            }
            else
            {

                // If our best attempt was very close to not beating it, we add 30 percent.
                chance += (1 - (Mathf.Abs(bestAttempt) / 20.0f)) * 30;

                //We add 35 percent if it's of the best quality.
                chance += Mathf.Pow(levelQuality / 4.0f, 3) * 4 * 7.5f;

            }

            return chance;

        }

    }

}
