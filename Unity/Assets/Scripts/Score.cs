using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private int currentScore;
    public TMP_Text scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScore = 0;
    }

    // Update Score 
  private void HandleScore()
    {
        scoreText.text = "Score: " + currentScore;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Mean")
        {
            currentScore ++;
            HandleScore();
        }
    }
}
