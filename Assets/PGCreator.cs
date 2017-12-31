using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGCreator : PGPlayer {

    public static new int GetChance()
    {
        return 8;
    }
    public static new string GetName()
    {
        return "Creator";
    }

    int levelsPerMonth = 3;

    List<PGLevel> createdLevels;

    public PGCreator()
    {

        createdLevels = new List<PGLevel>();

    }

    public override string GetInfo()
    {

        string info = GetName() + "\n";

        info += base.GetInfo() + "\n";

        uint totalPlays = 0;
        uint min = uint.MaxValue;
        uint max = uint.MinValue;
        uint unplayed = 0;
        foreach (PGLevel level in createdLevels)
        {

            uint playCount = level.GetPlayCount();
            totalPlays += playCount;

            if (playCount < min)
                min = playCount;
            if (playCount > max)
                max = playCount;

            if (playCount == 0)
                unplayed++;

        }


        info += "Levels created: " + createdLevels.Count + ", Total plays: " + totalPlays + ", Max: " + max + ", Unplayed: " + unplayed + "\n";
        
        return info;

    }

    public override void ProcessHour()
    {

        if(BehaviorManager.Session.StartASessionThisHourAndCreateALevel(levelsPerMonth))
        {

            PGLevel level = new PGLevel(this, BehaviorManager.LevelCreation.DetermineQualityOfNewLevel(), BehaviorManager.LevelCreation.DetermineDifficultyOfNewLevel());
            PGWorld.instance.AddLevel(level);
            createdLevels.Add(level);

        }

    }

}
