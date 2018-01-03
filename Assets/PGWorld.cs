using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGWorld : MonoBehaviour {

    public static PGWorld instance;

    public int day = 0;

    List<PGPlayer> playerList;
    public List<PGLevel>[] levelList;

    // Use this for initialization
    void Start() {

        instance = this;

        playerList = new List<PGPlayer>();
        CreatePlayers(1000);

        levelList = new List<PGLevel>[5];
        for (int i = 0; i < levelList.Length; i++)
            levelList[i] = new List<PGLevel>();

        //CreateLevels(1000);

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Random.InitState((int)(Time.time * 1000));

            for (int i = 0; i < 1; i++)
                ProcessYear();

            ChartManager.instance.UpdateCharts();

        }

        if (Input.GetKeyDown(KeyCode.I))
        {

            int index = Random.Range(0, playerList.Count);

            string info = playerList[index].GetInfo();

            Debug.Log(info);

        }

    }

    void Clear()
    {

        playerList = new List<PGPlayer>();
        CreatePlayers(1000);

        levelList = new List<PGLevel>[4];

    }

    void CreatePlayers(int count)
    {

        for (int i = 0; i < count; i++)
            playerList.Add(PGPlayer.CreatePlayer());

    }
    void CreateLevels(int count)
    {
        PGCreator admin = new PGCreator();
        for (int i = 0; i < count; i++)
        {

            AddLevel(new PGLevel(admin, BehaviorManager.LevelCreation.DetermineQualityOfNewLevel(), BehaviorManager.LevelCreation.DetermineDifficultyOfNewLevel()));
        }

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

        ChartManager.instance.GetDailyData();

        day++;

    }

    public void AddLevel(PGLevel level)
    {
        int quality = (int)level.GetQuality();
        if (levelList[quality].Count == 0)
            levelList[quality].Add(level);
        else
        {

            for(int i = 0; i < levelList[quality].Count; i++)
            {

                if(levelList[quality][i].GetDifficulty() > level.GetDifficulty())
                {

                    levelList[quality].Insert(i, level);
                    break;

                }

            }

        }

    }
    public PGLevel GetLevel(int quality, int index)
    {
        return levelList[quality][index];
    }
    public PGLevel GetLevel(int index)
    {
        int count = 0;
        for (int i = 0; i < levelList.Length; i++)
        {
            int temp = count;
            count += levelList[i].Count;
            if(index < count)
            {
                return levelList[i][index - temp];
            }
        }
        return null;
    }
    public int GetLevelCount()
    {
        int count = 0;
        for(int i = 0; i < levelList.Length; i++)
            count += levelList[i].Count;
        return count;
    }

    public PGPlayer GetPlayer(int index)
    {
        return playerList[index];
    }
    public int GetPlayerCount()
    {
        return playerList.Count;
    }
    public int GetGamerCount()
    {
        int gamerCount = 0;
        for (int i = 0; i < playerList.Count; i++)
            if ((string)playerList[i].GetType().GetMethod("GetName").Invoke(null, null) == "Gamer")
                gamerCount++;
        return gamerCount;
    }
    public int GetCreatorCount()
    {
        int creatorCount = 0;
        for (int i = 0; i < playerList.Count; i++)
            if ((string)playerList[i].GetType().GetMethod("GetName").Invoke(null, null) == "Creator")
                creatorCount++;
        return creatorCount;
    }


}
