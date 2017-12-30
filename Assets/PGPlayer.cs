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

    protected uint gold = 0;
    protected uint enjoyment = 0;

    public PGPlayer() { }

    public uint GetGold()
    {
        return gold;
    }
    public void GiveGold(uint _gold)
    {
        gold += _gold;
    }
    public uint GetEnjoyment()
    {
        return enjoyment;
    }
    public void ChangeEnjoyment(uint enjoymentChange)
    {
        enjoyment += enjoymentChange;
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
