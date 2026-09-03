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
        
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {   
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
            col.enabled = true;
        }

        if (mainCollider != null)
        {
            mainCollider.enabled = !enabled;
        }
    }

    public void ApplyBulletImpact(Vector3 direction, float force)
    {
        transform.position += direction.normalized * force;
    }
}
