using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float score = 0;

    public void IncreaseScore(float amount)
    {
        score += amount;
       scoreText.text = score.ToString(); // Display only the score number
        Debug.Log("Score updated: " + score); // Debug log
    }

    // Getter method to retrieve the current score
    public int GetScore()
    {
        return (int)score; // Return the current score
    }
}
