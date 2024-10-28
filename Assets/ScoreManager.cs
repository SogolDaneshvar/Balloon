using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI CoinScoreText;
    public TextMeshProUGUI MeteorScoreText;
    private float meteorScore;
    private float CoinScore = 0;
   // private float scoreCount = 0;

    public void IncreaseCoinScore(float amount)
    {
        CoinScore += amount;
        CoinScoreText.text = CoinScore.ToString(); // Display only the score number
        Debug.Log("Score updated: " + CoinScore); // Debug log
    }
    // Method to increase meteor score
    public void IncreaseMeteorScore(float amount)
    {
        meteorScore += amount;
        MeteorScoreText.text = meteorScore.ToString();
        Debug.Log("Meteor Score updated: " + meteorScore);
    }
    // Getter method to retrieve the current score
    public int GetScore()
    {
        return (int)CoinScore; // Return the current score
    }
}
