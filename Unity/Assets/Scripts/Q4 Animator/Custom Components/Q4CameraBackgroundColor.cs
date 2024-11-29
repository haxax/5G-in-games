using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4CameraBackgroundColor : Q4ComponentPlayer<Q4VarColor, Color, Gradient>
{
    [SerializeField] private Camera camera;
    protected override void CreatePreset()
    {
        Preset = new Q4VarColor(CustomValue, CustomEvaluatable);
    }

    protected override Color Get()
    {
        return camera.backgroundColor;
    }

    protected override void Set(Color currentValue)
    {
        camera.backgroundColor = currentValue;
    }
}