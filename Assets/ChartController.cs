using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartController : MonoBehaviour
{

    public Text header;
    public Text yMaxLabel;
    public Text yMinLabel;

    public LineChart chart;

    protected float yMaxValue = float.MinValue;
    protected float yMinValue = float.MaxValue;

    protected List<float> values;

    public void Awake()
    {

        values = new List<float>();
        Init();

    }
    public virtual void Init() { }

    public void AddValue(float value)
    {

        if (value < yMinValue)
            yMinValue = value;
        if (value > yMaxValue)
            yMaxValue = value;

        values.Add(value);

    }

    public virtual void UpdateGraph()
    {

        //If the difference is less than 10, we want some decimals.
        if (yMaxValue - yMinValue < 10)
        {

            yMinLabel.text = (yMinValue).ToString("0.00");
            yMaxLabel.text = (yMaxValue).ToString("0.00");

        }
        else
        {

            yMinLabel.text = ((int)yMinValue).ToString();
            yMaxLabel.text = ((int)yMaxValue).ToString();

        }
        chart.UpdateData(values.ToArray());

    }

}
