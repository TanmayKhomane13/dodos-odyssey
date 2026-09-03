using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;

    private Animator animator;
    private Collider mainCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        SetRagdoll(false);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy Health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Alien Died!");
        
        if (animator != null)
        {
            animator.enabled = false;
        }

        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        SetRagdoll(true);
    }

    void SetRagdoll(bool enabled)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !enabled;
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = enabled;
        }

        if (mainCollider != null)
        {
            mainCollider.enabled = !enabled;
        }
    }
}
