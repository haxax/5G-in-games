using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Q4VarString : Q4Variable<string, AnimationCurve>
{
    public Q4VarString(string customValue, AnimationCurve animationCurve) : base(customValue, animationCurve) { }

    public override string Evaluate(float progress)
    {
        return Value.Substring(0, Mathf.RoundToInt(Evaluatable.Evaluate(progress) * Value.Length));
    }
}