using UnityEngine;

public class Bounce : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] private string filterTag = "";           // Leave empty for any object
    [SerializeField] private float cooldown = 0.05f;          // Prevent multi-triggers

    [Header("Bounce Settings")]
    [SerializeField] private float bounceStrength = 15f;      // If using Impulse, think "velocity change for 1 kg"
    [SerializeField] private bool alignWithPadUp = true;      // Use pad up for angled pads
    [SerializeField] private bool resetOpposingVelocity = true; // Remove velocity opposing the bounce

    [Header("Mode")]
    [SerializeField] private bool setVelocityDirectly = false; // If true, set velocity instead of AddForce

    private float lastBounceTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(filterTag) && !other.CompareTag(filterTag)) return;

        var rb = other.attachedRigidbody;
        if (rb == null) return;

        if (Time.time - lastBounceTime < cooldown) return;
        lastBounceTime = Time.time;

        Vector2 dir = alignWithPadUp ? (Vector2)transform.up : Vector2.up;
        dir.Normalize();

        if (setVelocityDirectly)
        {
            // Make bounce mass independent and very snappy
            if (resetOpposingVelocity)
            {
                float vAlong = Vector2.Dot(rb.linearVelocity, dir);
                if (vAlong < 0f) rb.linearVelocity -= vAlong * dir; // remove downward component
            }

            // Add a fixed kick along the bounce direction
            rb.linearVelocity += dir * bounceStrength;
        }
        else
        {
            // Use physics impulse, mass will affect result
            if (resetOpposingVelocity)
            {
                float vAlong = Vector2.Dot(rb.linearVelocity, dir);
                if (vAlong < 0f) rb.linearVelocity -= vAlong * dir;
            }

            rb.AddForce(dir * bounceStrength, ForceMode2D.Impulse);
        }
    }
}
