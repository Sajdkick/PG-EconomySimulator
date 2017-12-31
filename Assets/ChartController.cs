using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChartController : MonoBehaviour {

    public Text header;
    public Text max;
    public Text min;

    public DataExtractor dataExtractor;
    public LineChart chart;

    float minValue = float.MaxValue;
    float maxValue = float.MinValue;

    List<float> values;

    public void Start()
    {

        values = new List<float>();

    }

    public void AddValue(float value)
    {

        if (value < minValue)
            minValue = value;
        if (value > maxValue)
            maxValue = value;

        values.Add(value);

    }
    
    public void UpdateGraph()
    {

        chart.UpdateData(values.ToArray());
        min.text = ((int)minValue).ToString();
        max.text = ((int)maxValue).ToString();

    }

}
