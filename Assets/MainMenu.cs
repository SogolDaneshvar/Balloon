//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using UnityEngine.XR;
//using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public Slider slider; // Make sure this slider is public so other scripts can access it

    // Method to start the game
    public void PlayGame()
    {
        // Load the main game scene, replace "MainScene" with your game scene name
        SceneManager.LoadScene("MainScene");
    }

    // Method to exit the game
    public void ExitGame()
    {
        Application.Quit();  // Will only work in a built game, not in the editor
    }

   // Method to handle the slider value change
    public void HandleControlSelection()
    {
        int selectedOption = (int)slider.value;  // 0 for Hand, 1 for Feet
        string controlChoice = selectedOption == 0 ? "Hand" : "Feet";

        // Store the selected option if needed (e.g., PlayerPrefs or other variable)
        Debug.Log("Control Selected: " + controlChoice);
    }

}
