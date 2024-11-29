using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Q4ComponentPlayer<T1, T2, T3> : Q4Player where T1 : Q4Variable<T2, T3>
{
    [SerializeField] private T1 preset;
    [SerializeField] private T2 customValue;
    [SerializeField] private T3 customEvaluatable;


    public T1 Preset { get => preset; protected set => preset = value; }
    public T2 CustomValue { get => customValue; protected set => customValue = value; }
    public T3 CustomEvaluatable { get => customEvaluatable; protected set => customEvaluatable = value; }

    public override void OnPlay()
    {
        if (Preset == null) { CreatePreset(); }
        base.OnPlay();
    }

    protected override void OnUpdate()
    {
        Set(preset.Evaluate(Counter));
    }
    protected abstract void CreatePreset();
    protected abstract void Set(T2 currentValue);
    protected abstract T2 Get();
}