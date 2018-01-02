using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartManager : MonoBehaviour {

    public static ChartManager instance;

    public GameObject ChartObject;

    public Dictionary<string, GameObject> chartDictionary;

    Vector3 lastChartPosition;

	// Use this for initialization
	void Awake () {

        instance = this;

        chartDictionary = new Dictionary<string, GameObject>();

        CreateChart("Enjoyment", new DataExtractors.Player.AverageGamerEnjoyment());
        CreateChart("Enjoyment Delta", new DataExtractors.Player.AverageEnjoymentDelta());
        CreateChart("Richest Gamer", new DataExtractors.Player.RichestGamer());
        CreateChart("Poorest Gamer", new DataExtractors.Player.PoorestGamer());
        CreateChart("Average Plays per level", new DataExtractors.Level.AveragePlays());
        CreateChart("Average Win Rate", new DataExtractors.Player.AverageWinRate());
        CreateChart("Couldn't find level", new DataExtractors.Player.AverageCouldntFindLevelCount());
        CreateChart("Average plays per gamer", new DataExtractors.Player.AveragePlays());

    }

    // Update is called once per frame
    void Update () {
		
	}

    void CreateChart(string name, DataExtractor dataExtractor)
    {

        GameObject chart;
        if (chartDictionary.Count == 0)
        {

            chart = Instantiate(ChartObject);
            lastChartPosition = chart.transform.position;

        }
        else
        {

            chart = Instantiate(ChartObject, lastChartPosition + Vector3.right * 2, Quaternion.identity);
            lastChartPosition = chart.transform.position;

        }

        chart.name = name;

        chart.GetComponent<ChartController>().header.text = name;
        chart.GetComponent<ChartController>().dataExtractor = dataExtractor;

        chartDictionary.Add(name, chart);

    }

    public void AddValue(string name, float value)
    {

        chartDictionary[name].GetComponent<ChartController>().AddValue(value);

    }

    public void UpdateCharts()
    {

        foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
        {
            chartObject.Value.GetComponent<ChartController>().UpdateGraph();
        }

    }

    public void GetDailyData()
    {

        int levelCount = PGWorld.instance.GetLevelCount();
        for (int i = 0; i < levelCount; i++)
            foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
                chartObject.Value.GetComponent<ChartController>().dataExtractor.Update(PGWorld.instance.GetLevel(i));

        int playerCount = PGWorld.instance.GetPlayerCount();
        for (int i = 0; i < playerCount; i++)
            foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
                chartObject.Value.GetComponent<ChartController>().dataExtractor.Update(PGWorld.instance.GetPlayer(i));

        foreach (KeyValuePair<string, GameObject> chartObject in chartDictionary)
        {
            ChartController chart = chartObject.Value.GetComponent<ChartController>();
            chart.AddValue(chart.dataExtractor.GetValue());
            chart.dataExtractor.PrepareForNewDay();
        }

    }

}
