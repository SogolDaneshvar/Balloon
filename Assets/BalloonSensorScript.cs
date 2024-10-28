using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BalloonSensorScript : MonoBehaviour
{
    public float speed; // Speed of balloon movement
    public float verticalSpeed; // Speed of vertical movement
    public float fallingSpeed; // Speed of the balloon when falling down
    private Rigidbody2D rb;
    public ScoreManager scoreManager; // Reference to the ScoreManager
    private Animator BalloonAnimator; // Reference to the balloon Animator component
    public Animator HeartBarAnimator; // Reference to the heart bar animator component
    public GameObject AirPuffPrefab; // Reference to the airpuff prefab

    // Serial port for sensor data
    private SerialPort serialPort;
    private float averaged_ax = 0;
    private float averaged_ay = 0;

    // Smoothing variables
    private Vector2 previousSpeed = Vector2.zero;
    private Vector2 targetSpeed = Vector2.zero;
    private Vector2 currentSpeed = Vector2.zero; // Holds the current smoothed speed
    public float smoothingFactor = 0.2f; // Increase for more responsiveness

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Disable gravity for the balloon

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not assigned in the Inspector!");
        }

        BalloonAnimator = GetComponent<Animator>();
        if (HeartBarAnimator == null)
        {
            Debug.LogError("HeartBarAnimator not assigned in the Inspector!");
        }

        // Initialize serial port for sensor data
        serialPort = new SerialPort("COM3", 9600);
        serialPort.Open();

        if (serialPort.IsOpen)
        {
            Debug.Log("Serial port opened successfully.");
        }
    }

    void FixedUpdate()
    {
        // Read sensor data if available
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            try
            {
                string line = serialPort.ReadLine();
                string[] sensorData = line.Split(',');

                averaged_ay = -int.Parse(sensorData[0]);
                averaged_ax = int.Parse(sensorData[1]);

                // Calculate target speed based on new sensor data
                targetSpeed = new Vector2(averaged_ax * speed, averaged_ay * verticalSpeed);

                // Exponential smoothing of speed for smoother movement
                currentSpeed = Vector2.Lerp(currentSpeed, targetSpeed, smoothingFactor);

                // Apply movement using smoothed speed
                transform.Translate(currentSpeed * Time.fixedDeltaTime);

                // Update previous speed for the next frame
                previousSpeed = currentSpeed;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading serial data: " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}


/* private void OnTriggerEnter2D(Collider2D other)
 {
     if (other.CompareTag("Coin"))
     {
         scoreManager.IncreaseCoinScore(0.5f); // Call the IncreaseScore method on the ScoreManager instance
         Destroy(other.gameObject); // Destroy the coin
         Debug.Log("Coin collected"); // Debug log
     }
     else if (other.CompareTag("Bat"))
     {
         Debug.Log("Collision with Bat detected");

         collisionCount++;
         if (collisionCount == 1)
         {
             TriggerFirstHole();
             SpawnAirPuff(firstHoleLocalPosition, Quaternion.identity); // No rotation for the first air puff
         }
     }
     if (collisionCount == 2)
     {
         TriggerSecondHole();
         // SpawnAirPuff(secondHoleLocalPosition, Quaternion.Euler(0, 0, 180)); // Rotate the second air puff by 180 degrees on the Z-axis

     }
     else if (collisionCount == 3)
     {
         HeartBarAnimator.SetTrigger("ThirdCollision"); // this wiil trigger the transition to the third state
         ActivateGravityOnBalloon();
     }



 }

 void CalculateHolePositions()
 {
     // Get the SpriteRenderer component
     SpriteRenderer balloonRenderer = GetComponent<SpriteRenderer>();

     // Ensure the balloonRenderer is valid
     if (balloonRenderer == null || balloonRenderer.sprite == null)
     {
         Debug.LogError("Balloon SpriteRenderer or sprite is missing!");
         return;
     }

     // Get the size of the sprite in local space, accounting for the local scale of the balloon
     Vector3 balloonSize = balloonRenderer.sprite.bounds.size; // Local size of the sprite
     Vector3 localScale = transform.localScale; // Get the local scale of the balloon GameObject

     // Multiply the sprite's size by the local scale to get the actual size in the world
     balloonSize = new Vector3(balloonSize.x * localScale.x, balloonSize.y * localScale.y, 0);

     // Debug the balloon size to check the values
     Debug.Log("Balloon Size: " + balloonSize);

     // Calculate the positions for the holes using more precise double calculations, converted to float for Vector3
     double firstHoleX = balloonSize.x / 2;    // Near the right side
     double firstHoleY = -balloonSize.y / 25;  // Slightly towards the top-left
     double secondHoleX = -balloonSize.x / 2;  // Near the left side
     double secondHoleY = balloonSize.y / 4;   // Slightly towards the bottom-right

     // Convert the double values to floats and store in Vector3 for local positioning
     firstHoleLocalPosition = new Vector3((float)firstHoleX, (float)firstHoleY, 0);
     secondHoleLocalPosition = new Vector3((float)secondHoleX, (float)secondHoleY, 0);

     // Debug the hole positions to check the values
     Debug.Log("First Hole Position: " + firstHoleLocalPosition);
     Debug.Log("Second Hole Position: " + secondHoleLocalPosition);
 }

 void TriggerFirstHole()
 {
     // Set Animator parameter or trigger for the first hole
     BalloonAnimator.SetTrigger("FirstHole"); // This will trigger the transition to the first hole animation state
     HeartBarAnimator.SetTrigger("FirstCollision"); // this will trigger the transition to the first hear bar state
     Debug.Log("Triggered first hole animation & first hearbat animation");
 }

 void TriggerSecondHole()
 {
     // Set Animator parameter or trigger for the second hole
     BalloonAnimator.SetTrigger("SecondHole"); // This will trigger the transition to the second hole animation state
     HeartBarAnimator.SetTrigger("SecondCollision"); // this will trigger the transition to the Second collision state
     Debug.Log("Triggered second hole animation & the second heartbar animation");
 }


 // Spawn the AirPuff prefab at the specified position
 void SpawnAirPuff(Vector3 localPosition, Quaternion rotation)
 {
     // Instantiate air puff at the balloon's position plus the local offset for the hole
     Vector3 spawnPosition = transform.position + localPosition;
     spawnPosition.z = -2; // Move the air puff to the front (adjust this as needed)

     GameObject airPuff = Instantiate(AirPuffPrefab, spawnPosition, rotation);

     // Set the air puff as a child of the balloon, so it moves with the balloon
     airPuff.transform.SetParent(transform);

     // Destroy the air puff object after the animation finishes (e.g., 1 second)
     Destroy(airPuff, 1f); // Adjust the time based on your animation length
 }





 void ActivateGravityOnBalloon()
 {
     // Find the Balloon GameObject (if not already referenced)
     GameObject Balloon = GameObject.FindWithTag("Balloon");

     if (Balloon != null)
     {
         // Get the Rigidbody2D component attached to the Balloon
         Rigidbody2D RB = Balloon.GetComponent<Rigidbody2D>();

         if (RB != null)
         {
             // Set gravityScale to 1 to enable gravity and make the balloon fall
             RB.gravityScale = fallingSpeed;
             Debug.Log("Gravity activated on Balloon"); // Debug log to confirm gravity activation
             //  Add a script to handle when the balloon exits the camera view**
             Balloon.AddComponent<BalloonExitHandler>();
         }

     }
     else
     {
         Debug.LogError("Balloon instance not found");
     }
 }
} */



/*
public class BalloonExitHandler : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        // Trigger the game over screen from the GameManager once balloon exits camera view
        FindObjectOfType<GameManagerScript>().GameOver();
        Debug.Log("Balloon has exited the camera view. Game Over triggered.");
    }
} */