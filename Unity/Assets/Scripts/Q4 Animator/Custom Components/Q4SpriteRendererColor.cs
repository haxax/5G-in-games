using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4SpriteRendererColor : Q4ComponentPlayer<Q4VarColor, Color, Gradient>
{
    [SerializeField] private SpriteRenderer renderer;
    protected override void CreatePreset()
    {
        Preset = new Q4VarColor(CustomValue, CustomEvaluatable);
    }

    protected override Color Get()
    {
        return renderer.color;
    }

    protected override void Set(Color currentValue)
    {
        renderer.color = currentValue;
    }
}