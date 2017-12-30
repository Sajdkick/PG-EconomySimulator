using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PGPlayer {

    public static int GetChance()
    {
        return 0;
    }
    public static string GetName()
    {
        return "";
    }

    public static int GetTotalChance()
    {

        List<System.Type> genres = ReflectiveEnumerator.GetEnumerableOfType<PGPlayer>();

        int totalChance = 0;
        foreach (System.Type _genre in genres)
            totalChance += (int)_genre.GetMethod("GetChance").Invoke(null, null);

        return totalChance;

    }

    public static PGPlayer CreatePlayer()
    {
 
        int roll = Random.Range(0, GetTotalChance());

        List<System.Type> players = ReflectiveEnumerator.GetEnumerableOfType<PGPlayer>();
        int totalChance = 0;
        foreach (System.Type _player in players)
        {

            totalChance += (int)_player.GetMethod("GetChance").Invoke(null, null);
            if(totalChance > roll)
                return (PGPlayer)System.Activator.CreateInstance(_player);

        }

        //We shouldn't be able to get here.
        Debug.Assert(false);

        return null;

    }

    protected int gold = 0;
    protected float enjoyment = 0;

    public PGPlayer() { }

    public int GetGold()
    {
        return gold;
    }
    public void ChangeGold(int changeIngold)
    {
        ChangeEnjoyment(BehaviorManager.Enjoyment.EnjoymentFromGold(changeIngold));
        gold += changeIngold;
    }
    public float GetEnjoyment()
    {
        return enjoyment;
    }
    public void ChangeEnjoyment(int changeInEnjoyment)
    {
        enjoyment += changeInEnjoyment;
    }

    public virtual string GetInfo() {

        return "Gold: " + gold;

    }

    public void ProcessDay() {

        for (int i = 0; i < 16; i++)
        {

            ProcessHour();

        }

    }
    public virtual void ProcessHour() { }

}
