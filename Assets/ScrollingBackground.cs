using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollingBackground : MonoBehaviour
{
    public GameManagerScript GameManager;  // Reference to the SceneTransitionManager
    public float scrollSpeed;
    [SerializeField] private Renderer bgrenderer;
    public float distanceTraveled = 0f;  // Track the total distance traveled
    public float distanceToCity = 1000f;  // The total distance needed to reach the city

    // Update is called once per frame
    void Update()
    {
        bgrenderer.material.mainTextureOffset += new Vector2(0, Time.deltaTime * scrollSpeed);
        //Increment the distance based on background scroll speed and time passed
        distanceTraveled += scrollSpeed * Time.deltaTime;

        // Check if player has reached the city
        if (distanceTraveled >= distanceToCity)
        {
            TriggerCityTransition();
        }
    }
    private void TriggerCityTransition()
    {
        // Use SceneTransitionManager to transition to the final city scene
        GameManager.StartSceneTransition("CityScene");
    }

    public float GetProgressPercentage()
    {
        // Return the percentage of the journey completed
        return Mathf.Clamp01(distanceTraveled / distanceToCity);
    }
}

