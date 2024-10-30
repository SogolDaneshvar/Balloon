using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeteorMovement : MonoBehaviour
{
    private ScoreManager scoreManager;
    public MainMenu mainMenuScript; // Reference to the MainMenuScript to access slider value
    public BalloonSensorScript balloonSensorScript;

    // Amplitude of the zig-zag motion
    public float amplitude = 2.0f;
    // Speed of the zig-zag oscillation
    public float frequency = 2.0f;
    // Downward movement speed
    public float fallSpeed = 5.0f;

    // Initial x position of the meteor
    private float startX;

    void Start()
    {
        // Find the ScoreManager instance in the scene
        scoreManager = FindObjectOfType<ScoreManager>();
        // Record the initial x position when the meteor spawns
        startX = transform.position.x;
    }

    void Update()
    {
        // Zig-zag motion in the x-axis
        float xOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector2(startX + xOffset, transform.position.y);

        // Move down the y-axis
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Destroy if out of bounds (similar to `MoveDown` script)
        if (transform.position.y < -Camera.main.orthographicSize - 1)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mainMenuScript == null)
        {
            mainMenuScript = FindObjectOfType<MainMenu>();
            if (mainMenuScript == null)
            {
                Debug.LogError("MainMenuScript could not be found in the scene.");
                return;
            }
        }

        if (mainMenuScript.slider == null)
        {
            Debug.LogError("Slider reference in MainMenuScript is null.");
        }
    

        // Check if the collider belongs to the balloon
        if (other.CompareTag("Balloon"))
        {
            if (mainMenuScript.slider.value == 0) // If slider is set to 0 (hand)
            {
                if (balloonSensorScript.averaged_az < 13000)
                {
                    scoreManager.IncreaseMeteorScore(1);  // Increase the score by 1
                    Destroy(gameObject);                  // Destroy the meteor
                }
            }
            else if (mainMenuScript.slider.value == 1) // If slider is set to 1 (feet)
            {
                scoreManager.IncreaseMeteorScore(1);      // Increase the score by 1
                Destroy(gameObject);                      // Destroy the meteor
            }
        }
    }
}



