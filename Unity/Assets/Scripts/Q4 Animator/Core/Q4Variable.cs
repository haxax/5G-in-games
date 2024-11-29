using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Q4Variable<T1, T2>
{
    public Q4Variable(T1 customValue, T2 customEvaluatable)
    { value = customValue; evaluatable = customEvaluatable; }

    [SerializeField] private T1 value;
    [SerializeField] private T2 evaluatable;

    public T1 Value => value;
    public T2 Evaluatable => evaluatable;

    public abstract T1 Evaluate(float progress);
}