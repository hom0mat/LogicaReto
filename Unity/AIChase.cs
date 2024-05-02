using UnityEngine;
using UnityEngine.UI;

public class AIChase : MonoBehaviour
{

    public Transform player; // Reference to the player's Transform
    public float moveSpeed; // Speed at which the enemy moves
    private Animator animator; // Reference to the Animator component

    AudioManager audioManager;
    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to the enemy
    }

    //private void Awake()
    //{
    //    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    //}
    void Update()
    {
        if (player != null)
        {
            // Move the enemy towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Determine direction to move based on player's position relative to the enemy
            Vector2 direction = player.position - transform.position;

            // Set animation parameters based on direction
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Move horizontally
                animator.SetFloat("Horizontal", Mathf.Sign(direction.x));
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Speed", moveSpeed);
            }
            else
            {
                // Move vertically
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", Mathf.Sign(direction.y));
                animator.SetFloat("Speed", moveSpeed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //audioManager.PlaySFX(audioManager.animalsCollision);
            HealthManager.health--;
        }
    }
}

