using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4LightColor : Q4ComponentPlayer<Q4VarColor, Color, Gradient>
{
    [SerializeField] private Light light;
    protected override void CreatePreset()
    {
        Preset = new Q4VarColor(CustomValue, CustomEvaluatable);
    }

    protected override Color Get()
    {
        return light.color;
    }

    protected override void Set(Color currentValue)
    {
        light.color = currentValue;
    }
}