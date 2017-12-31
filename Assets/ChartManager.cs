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

        CreateChart("Enjoyment");
        CreateChart("Richest Player");

    }

    // Update is called once per frame
    void Update () {
		
	}

    void CreateChart(string name)
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

        chart.GetComponent<ChartController>().header.text = name;

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

}
