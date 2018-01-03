using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionChartController : ChartController
{

    public Text xMaxLabel;
    public Text xMinLabel;

    public Text xLabel;

    internal int xParam;
    internal float start;
    internal float end;
    internal float delta;
    internal object[] parameters;
    internal System.Reflection.MethodInfo function;

    public override void Init()
    {
        xParam = 0;
        start = 0;
        end = 1;
        delta = 0.1f;
    }

    public void CreateValues()
    {

        // We make sure that the delta is in the right direction.
        Debug.Assert(Mathf.Abs(start + delta - end) < Mathf.Abs(start - end));

        for(float i = start; i <= end; i += delta)
        {

            parameters[xParam] = i;
            AddValue((float)function.Invoke(null, parameters));

        }

    }

    public override void UpdateGraph()
    {
        xMaxLabel.text = end.ToString();
        xMinLabel.text = start.ToString();
        xLabel.text = function.GetParameters()[xParam].Name;
        base.UpdateGraph();
    }

}
