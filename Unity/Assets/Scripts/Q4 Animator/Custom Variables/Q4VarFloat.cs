using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4VarFloat : Q4Variable<float, AnimationCurve>
{
    public Q4VarFloat(float customValue, AnimationCurve animationCurve) : base(customValue, animationCurve) { }

    public override float Evaluate(float progress)
    {
        return Evaluatable.Evaluate(progress) * Value;
    }
}