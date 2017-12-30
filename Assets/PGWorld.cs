using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGWorld : MonoBehaviour {

    public static PGWorld instance;

    public int day = 0;

    List<PGPlayer> playerList;
    List<PGLevel> levelList;

    // Use this for initialization
    void Start() {

        instance = this;

        playerList = new List<PGPlayer>();
        CreatePlayers(1000);
        CountPlayers();

        levelList = new List<PGLevel>();

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            ProcessYear();

            Debug.Log("Level count: " + levelList.Count + ", No level Errors: " + PGGamer.noNewLevelError);

            CountGold();

        }

        if (Input.GetKeyDown(KeyCode.I))
        {

            int index = Random.Range(0, playerList.Count);

            string info = playerList[index].GetInfo();

            Debug.Log(info);

        }

    }

    void CreatePlayers(int count)
    {

        for (int i = 0; i < count; i++)
            playerList.Add(PGPlayer.CreatePlayer());

    }

    void CountPlayers()
    {

        int gamerCount = 0;
        int creatorCount = 0;
        for (int i = 0; i < playerList.Count; i++)
        {

            if ((string)playerList[i].GetType().GetMethod("GetName").Invoke(null, null) == "Creator")
                creatorCount++;
            if ((string)playerList[i].GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                gamerCount++;

        }

        Debug.Log("Gamer count: " + gamerCount);
        Debug.Log("Creator count: " + creatorCount);

    }
    void CountLevelPlays()
    {

        uint totalPlays = 0;
        uint max = uint.MinValue;
        uint min = uint.MaxValue;
        uint unplayed = 0;
        foreach (PGLevel level in levelList)
        {

            uint playCount = level.GetPlayCount();

            if (playCount == 0)
                unplayed++;

            if (playCount < min)
                min = playCount;
            if (playCount > max)
                max = playCount;

            totalPlays += playCount;

        }
        float averagePlays = (float)totalPlays / levelList.Count;

        Debug.Log("Average Plays per level: " + averagePlays + ", Max: " + max + ", Unplayed: " + unplayed);

    }
    void CountGold()
    {

        Dictionary<string, int> playerCount = new Dictionary<string, int>();
        Dictionary<string, int> goldCount = new Dictionary<string, int>();

        int max = 0;
        int min = 0;

        for (int i = 0; i < playerList.Count; i++)
        {

            string name = (string)playerList[i].GetType().GetMethod("GetName").Invoke(null, null);

            int gold = playerList[i].GetGold();
            if (gold > playerList[max].GetGold())
                max = i;
            if (gold < playerList[min].GetGold())
                min = i;

            if (playerCount.ContainsKey(name))
            {

                playerCount[name]++;
                goldCount[name] += gold;

            }
            else
            {

                playerCount.Add(name, 1);
                goldCount.Add(name, playerList[i].GetGold());

            }

        }

        foreach (KeyValuePair<string, int> item in playerCount)
        {

            Debug.Log("Average " + item.Key + " gold: " + (float)goldCount[item.Key] / item.Value);

        }

        Debug.Log("Richest player: " + playerList[max].GetGold() + " gold, " + playerList[max].GetType().GetMethod("GetName").Invoke(null, null));
        Debug.Log("Poorest player: " + playerList[min].GetGold() + " gold, " + playerList[min].GetType().GetMethod("GetName").Invoke(null, null));

    }

    void ProcessYear()
    {

        for (int i = 0; i < 12; i++)
            ProcessMonth();

    }
    void ProcessMonth()
    {

        for (int i = 0; i < 31; i++)
            ProcessDay();

    }
    void ProcessDay()
    {

        for(int i = 0; i < playerList.Count; i++)
            playerList[i].ProcessDay();

        day++;

    }

    public void AddLevel(PGLevel level)
    {

        levelList.Add(level);

    }
    public PGLevel GetLevel(int index)
    {

        return levelList[index];

    }
    public int GetLevelCount()
    {

        return levelList.Count;

    }

}
