using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject coinPrefab; // Reference to the coin prefab
    public GameObject BatPrefab; // Reference to the bat prefab 
    public float spawnInterval = 2f; // Time between spawns
    public float xRange = 8f; // Horizontal range for spawning
    public float yStart = 5f; // Vertical start position for spawning
    private void Start()
    {
        // Start the spawning process
        InvokeRepeating("SpawnObject", spawnInterval, spawnInterval);
    }

    private void SpawnObject()
    {
        // Randomly choose a position within the xRange
        Vector2 spawnPosition = new Vector2(Random.Range(-xRange, xRange), yStart);

        // Randomly decide whether to spawn a coin or a bat
        if (Random.value > 0.5f)
        {
            if (!IsPositionOccupied(spawnPosition))
            {
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            if (!IsPositionOccupied(spawnPosition))
            {
                Instantiate(BatPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    private bool IsPositionOccupied(Vector2 position)
    {
        float checkRadius = 1f; // Radius for checking overlaps, adjust as needed
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, checkRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Coin") || collider.CompareTag("Bat")) // Update tags
            {
                return true;
            }
        }
        return false;
    }
}

