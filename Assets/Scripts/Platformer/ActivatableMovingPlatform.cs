using UnityEngine;

public class ActivatableMovingPlatform : MonoBehaviour
{
    bool active = false; // If true, the platform moves.

    public float moveDistance = 3f; // How far to move from the start position
    public float speed = 2f;        // Platform speed
    public bool cycle = false;      // If true, loops instead of ping-pong

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingRight = true;

    void Start()
    {
        active = false;

        startPos = transform.position;
        targetPos = startPos + Vector3.right * moveDistance;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Set as kinematic using the new API
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        if (active)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // When the platform reaches its target, reverse or reset
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                if (cycle)
                {
                    // Instantly reset to the other side
                    transform.position = startPos;
                }
                else
                {
                    // Ping-pong movement
                    movingRight = !movingRight;
                    targetPos = movingRight ? startPos + Vector3.right * moveDistance : startPos - Vector3.right;
                }
            }
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            active = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
