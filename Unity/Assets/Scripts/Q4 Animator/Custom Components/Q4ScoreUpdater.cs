using UnityEngine;
using TMPro;

public class Q4ScoreUpdater : Q4ComponentPlayer<Q4VarInt, int, AnimationCurve>
{
    [SerializeField] private PlayerObject player;
    [SerializeField] private string scoreTxt = "Score: {0}";
    [SerializeField] private TMP_Text txt;
    protected override void CreatePreset()
    {
        Preset = new Q4VarInt(CustomValue, CustomEvaluatable);
    }
    public void UpdatePreset() { CreatePreset(); }

    protected override int Get()
    {
        return player.CurrentScore;
    }

    protected override void Set(int currentValue)
    {
        txt.text = string.Format(scoreTxt, currentValue);
    }
}