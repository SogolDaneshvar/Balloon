using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;   // Speed at which the background scrolls
    public float scrollDistance = 10f;  // The distance the background should scroll
    private Vector3 startingPosition;   // The initial position of the background
    private bool isScrolling = true;    // Whether or not the background is currently scrolling

    void Start()
    {
        // Record the initial position of the background group
        startingPosition = transform.position;
    }

    void Update()
    {
        // If the background should scroll, move it upwards
        if (isScrolling)
        {
            // Move the background downwards at a constant speed
            transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            // Check if the background has scrolled the desired distance
            if (Vector3.Distance(startingPosition, transform.position) >= scrollDistance)
            {
                isScrolling = false;  // Stop scrolling once the distance is reached
            }
        }
    }
}

