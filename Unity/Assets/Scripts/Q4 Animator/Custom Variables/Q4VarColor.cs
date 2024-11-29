using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Q4VarColor : Q4Variable<Color, Gradient>
{
    public Q4VarColor(Color customValue, Gradient gradient) : base(customValue, gradient) { }

    public override Color Evaluate(float progress)
    {
        return Evaluatable.Evaluate(progress) * Value;
    }
}