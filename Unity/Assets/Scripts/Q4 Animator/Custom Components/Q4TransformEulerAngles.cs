using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4TransformEulerAngles : Q4ComponentPlayer<Q4VarVector3, Vector3, AnimationCurve>
{
    protected override void CreatePreset()
    {
        Preset = new Q4VarVector3(CustomValue, CustomEvaluatable);
    }

    protected override Vector3 Get()
    {
        return transform.localEulerAngles;
    }

    protected override void Set(Vector3 currentValue)
    {
        transform.localEulerAngles = currentValue;
    }
}