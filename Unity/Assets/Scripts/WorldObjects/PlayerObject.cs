using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerObject : MonoBehaviour
{
    [SerializeField] private FivegPoint my5G;
    public FivegPoint My5G => my5G;
    [SerializeField] private bool isSimulated = false;
    [SerializeField] private Q4ScoreUpdater scoreUpdater;
    private int currentScore;
    public int CurrentScore => currentScore;


    private void Start()
    {
        SetLocation(WorldManager.Instance.CurrentGpsLocation);
        WorldManager.Instance.OnGpsLocationUpdate.AddListener(SetLocation);
        currentScore = 0;
    }

    public void SetLocation(Vector2 ingameLocation)
    {
        if (isSimulated) { return; }
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);

        if (my5G == null) { return; }
        my5G.IsWithingRadius(gameObject);
    }
    public void SetSimulatedLocation(Vector2 ingameLocation)
    {
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);

        if (my5G == null) { return; }
        my5G.SetCircleMaterial(my5G.YellowMat);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Crystal")
        {
            CollectableGem gem = col.transform.root.GetComponent<CollectableGem>();
            HandleScore(CurrentScore, CurrentScore + gem.Points);
            currentScore += gem.Points;
            gem.OnCollect();
        }
    }

    private void HandleScore(int previousScore, int newScore)
    {
        if(scoreUpdater == null) { return; }
        if (scoreUpdater.Preset == null)
        { scoreUpdater.UpdatePreset(); }
        Keyframe[] newKeys = scoreUpdater.Preset.Evaluatable.keys;
        newKeys[0].value = previousScore;
        newKeys[newKeys.Length - 1].value = newScore;
        scoreUpdater.CustomEvaluatable.keys = newKeys;
        scoreUpdater.UpdatePreset();

        scoreUpdater.Play();
    }
}