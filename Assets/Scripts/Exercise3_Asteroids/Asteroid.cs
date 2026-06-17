/*
 * Assignment: Asteroids Game - Asteroid Script - PART 2
 * 
 * Objective: Create a functional asteroid script. This script will be responsible for the functionality of the asteroids.
 * this should include initial velocity, angular velocity, and breaking into smaller asteroids when destroyed.
 * Remember, asteroids should only spawn through the AsteroidSpawner script. 
 
* Requirements:
* 1. The asteroid should start with a set velocity but a random angular velocity. Both of these are set in the Rigidbody2D
*       Hint: All movement for the asteroid should be done via a Rigidbody2D and should be able to be set at Start.
* 2. When the asteroid is destroyed, it should spawn two smaller asteroids if it is not already the smallest size. 
*       Hint: How can you use a function to set the AsteroidSpawner variable from a different script?
* 3. When the asteroid hits the player, it should destroy the player. 
*/

using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    private Rigidbody2D rb;
    private AsteroidSpawner spawner;
    private Vector2 velocity;
    private int childrenToSpawn = 2;

    void Start()
    {
        spawner = FindAnyObjectByType<AsteroidSpawner>();
        rb = GetComponent<Rigidbody2D>();

        velocity = Vector2.one * speed;
        rb.linearVelocity = velocity;
        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    void Update()
    {
    }

    /// <summary>
    /// break large asteroids into two smaller ones when asteroid is destroyed
    /// </summary>
    private void BreakAsteroid()
    {
        if (size != AsteroidSize.Small)
        {
            SpawnChildren(size - 1);
        }
    }


    private void SpawnChildren(AsteroidSize childSize)
    {
        Debug.Log("spawning size " + childSize);
        for (int i = 0; i < childrenToSpawn; i++)
        {
            spawner.SpawnAsteroid(transform.position, childSize);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            BreakAsteroid();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}