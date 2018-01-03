using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartManager : MonoBehaviour {

    public static ChartManager instance;

    public GameObject DailyChartObject;
    public GameObject FunctionChartObject;
    public Text worldInfo;

    public Dictionary<string, GameObject> chartDictionary;

    Vector3 lastChartPosition;

	// Use this for initialization
	void Awake () {

        instance = this;

        chartDictionary = new Dictionary<string, GameObject>();

        CreateChart("Enjoyment", new DataExtractors.Player.AverageGamerEnjoyment());
        CreateChart("Number of Quality 1 levels", new DataExtractors.Level.NumberOfQuality1());
        CreateChart("Number of Quality 2 levels", new DataExtractors.Level.NumberOfQuality2());
        CreateChart("Number of Quality 3 levels", new DataExtractors.Level.NumberOfQuality3());
        CreateChart("Couldn't find level", new DataExtractors.Player.AverageSkippedLevelCount());
        CreateChart("Enjoyment Delta", new DataExtractors.Player.AverageEnjoymentDelta());
        CreateChart("Richest Gamer", new DataExtractors.Player.RichestGamer());
        CreateChart("Poorest Gamer", new DataExtractors.Player.PoorestGamer());
        CreateChart("Average Plays per level", new DataExtractors.Level.AveragePlays());
        CreateChart("Average Win Rate", new DataExtractors.Player.AverageWinRate());
        CreateChart("Average plays per gamer", new DataExtractors.Player.AveragePlays());
        CreateChart("Enjoyment from highscore", typeof(BehaviorManager.Enjoyment).GetMethod("EnjoymentFromBeatingAHighscore"), 0, 0, 1100, 1, new object[] { 0, 2000 });
        CreateChart("Enjoyment from beating a level\n first try with average skill", typeof(BehaviorManager.Enjoyment).GetMethod("EnjoymentFromBeatingLevel"), 1, 0, 10, 0.01f, new object[] { 5, 0, 1, 1});

    }

    // Update is called once per frame
    void Update () {
		
	}

    void CreateChart(string name, DataExtractor dataExtractor)
    {

        GameObject chart = InstantiateChartObject(DailyChartObject);

        chart.name = name;

        chart.GetComponent<DailyChartController>().header.text = name;
        chart.GetComponent<DailyChartController>().dataExtractor = dataExtractor;

        chartDictionary.Add(name, chart);

    }
    void CreateChart(string name, System.Reflection.MethodInfo function, int xParam, float start, float end, float delta, object[] parameters)
    {

        GameObject chart = InstantiateChartObject(FunctionChartObject);

        chart.name = name;

        chart.GetComponent<FunctionChartController>().header.text = name;

        chart.GetComponent<FunctionChartController>().function = function;
        chart.GetComponent<FunctionChartController>().xParam = xParam;
        chart.GetComponent<FunctionChartController>().start = start;
        chart.GetComponent<FunctionChartController>().end = end;
        chart.GetComponent<FunctionChartController>().delta = delta;
        chart.GetComponent<FunctionChartController>().parameters = parameters;

        chart.GetComponent<FunctionChartController>().CreateValues();
        chart.GetComponent<FunctionChartController>().UpdateGraph();

        //chartDictionary.Add(name, chart);

    }

    public GameObject InstantiateChartObject(GameObject chartObject)
    {

        GameObject chart;
        if (chartDictionary.Count == 0)
        {

            chart = Instantiate(chartObject, Vector3.zero + Vector3.right, Quaternion.identity);
            lastChartPosition = chart.transform.position;

        }
        else
        {

            chart = Instantiate(chartObject, lastChartPosition + Vector3.right * 2, Quaternion.identity);
            lastChartPosition = chart.transform.position;

        }
        return chart;

    }

    public void UpdateCharts()
    {

        foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
        {
            chartObject.Value.GetComponent<DailyChartController>().UpdateGraph();
        }

        UpdateWorldInfo();

    }

    public void GetDailyData()
    {

        int levelCount = PGWorld.instance.GetLevelCount();
        for (int i = 0; i < levelCount; i++)
            foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
                chartObject.Value.GetComponent<DailyChartController>().dataExtractor.Update(PGWorld.instance.GetLevel(i));

        int playerCount = PGWorld.instance.GetPlayerCount();
        for (int i = 0; i < playerCount; i++)
            foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
                chartObject.Value.GetComponent<DailyChartController>().dataExtractor.Update(PGWorld.instance.GetPlayer(i));

        foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
        {
            DailyChartController chart = chartObject.Value.GetComponent<DailyChartController>();
            chart.AddValue(chart.dataExtractor.GetValue());
            chart.dataExtractor.PrepareForNewDay();
        }

    }
    public void UpdateWorldInfo()
    {

        string info = "World info:\n";
        info += "Number of levels: " + PGWorld.instance.GetLevelCount() + "\n";
        info += "Total player count: " + PGWorld.instance.GetPlayerCount() + "\n";
        info += "Gamers: " + PGWorld.instance.GetGamerCount() + "\n";
        info += "Creators: " + PGWorld.instance.GetCreatorCount() + "\n";

        worldInfo.text = info;

    }
}
