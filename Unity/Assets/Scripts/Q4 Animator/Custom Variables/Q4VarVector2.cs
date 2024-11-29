using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Q4VarVector2 : Q4Variable<Vector2, AnimationCurve>
{
    public Q4VarVector2(Vector2 customValue, AnimationCurve animationCurve) : base(customValue, animationCurve) { }

    public override Vector2 Evaluate(float progress)
    {
        return Evaluatable.Evaluate(progress) * Value;
    }
}