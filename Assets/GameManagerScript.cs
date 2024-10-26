using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public Animation StartingTransitionAnimation;  // Reference to the start transition Animation component
    public Animation EndingTransitionAnimation;    // Reference to the end transition Animation component
    public string startTransitionClipName;      // Name of the start transition animation clip
    public string endTransitionClipName;        // Name of the end transition animation clip
    public float transitionDuration = 1.5f;       // Duration of the transition animations (match to animation length)
    public GameObject gameOverCanvas; // Reference to the GameOverCanvas
    public TextMeshProUGUI scoreText; // Reference to the score text UI element
    private CanvasGroup canvasGroup; // Reference to the Canvas Group for fading
    private ScoreManager scoreManager;
    void Start()
    {
        gameOverCanvas.SetActive(false); // Ensure the game over UI is hidden at start
        canvasGroup = gameOverCanvas.GetComponent<CanvasGroup>(); // Get the Canvas Group component
        scoreManager = FindObjectOfType<ScoreManager>(); // Find the ScoreManager in the scene
    }

    // Method to trigger the game over screen with fade-in
    public void GameOver()
    {
        gameOverCanvas.SetActive(true); // Activate the Canvas

        // Get the final score from the ScoreManager and display it
        int finalScore = scoreManager.GetScore(); // Get the score from ScoreManager
        scoreText.text = "Score: " + finalScore.ToString(); // Update score text on the game over screen
        StartCoroutine(FadeIn()); // Start the fade-in effect
    }

    // Fade-in coroutine to gradually increase alpha
    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0; // Start fully transparent
        float duration = 1.5f; // Duration of fade-in
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration); // Gradually increase alpha
            yield return null;
        }
    }

    // Method linked to the Play Again button
    public void PlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Reload the current scene
    }
    // Call this method to start the transition to a new scene
    public void StartSceneTransition(string sceneName)
    {
        StartCoroutine(PlayStartTransition(sceneName));
    }
    // Coroutine to handle the start transition and scene load
    private IEnumerator PlayStartTransition(string sceneName)
    {
        // Play the "start transition" animation
        StartingTransitionAnimation.Play(startTransitionClipName);

        // Wait for the animation to finish (adjust transitionDuration to match animation length)
        yield return new WaitForSeconds(transitionDuration);

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    // Call this method when the new scene starts to play the end transition
    public void PlayEndTransition()
    {
        StartCoroutine(PlayEndTransitionRoutine());
    }

    private IEnumerator PlayEndTransitionRoutine()
    {
        // Play the "end transition" animation
       EndingTransitionAnimation.Play(endTransitionClipName);

        // Wait for the animation to finish
        yield return new WaitForSeconds(transitionDuration);
    }
}



