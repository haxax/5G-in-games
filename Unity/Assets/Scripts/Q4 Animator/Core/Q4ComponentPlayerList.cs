using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Q4ComponentPlayerList<T> : Q4ComponentPlayer<Q4VarInt, int, AnimationCurve>
{
    [SerializeField] private List<T> list = new List<T>();
    private int current = 0;

    public override void OnPlay()
    {
        if (Preset != null)
        {
            CustomEvaluatable = Preset.Evaluatable;
            Preset = null;
        }
        base.OnPlay();
    }

    protected override void CreatePreset()
    {
        CustomValue = list.Count - 1;
        Preset = new Q4VarInt(CustomValue, CustomEvaluatable);
    }

    protected override int Get()
    {
        return current;
    }
    protected override void Set(int currentValue)
    {
        current = currentValue;
        SetCurrentListItem(list[current]);
    }


    protected abstract T GetCurrentListItem();

    protected abstract void SetCurrentListItem(T current);

}