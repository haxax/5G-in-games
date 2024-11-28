using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerObject : MonoBehaviour
{
    [SerializeField] private FivegPoint my5G;
    [SerializeField] private bool isSimulated = false;
    private int currentScore;
    public TMP_Text scoreText;


    private void Start()
    {
        SetLocation(WorldManager.Instance.CurrentGpsLocation);
        WorldManager.Instance.OnGpsLocationUpdate.AddListener(SetLocation);
        currentScore = 0;
    }

    private void HandleScore()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void SetLocation(Vector2 ingameLocation)
    {
        if (isSimulated) { return; }
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);
        my5G.IsWithingRadius(gameObject);
    }
    public void SetSimulatedLocation(Vector2 ingameLocation)
    {
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);
        my5G.SetCircleMaterial(my5G.YellowMat);
    }
    void onCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Crystal")
        {
            currentScore ++;
            HandleScore();
        }
    }
}