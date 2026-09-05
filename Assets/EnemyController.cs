using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private CharacterController characterController;
    private Animator animator;
    public Transform player;
    public float attackDistance = 1f;
    public float detectionDistance = 10f;
    public float moveSpeed = 7f;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        SetRagdoll(false);
    }

    void Update()
    {
        // check if enemy dead
        if (isDead)
            return;
        
        if (player == null)
            return;
        
        float distance = Vector3.Distance(
            transform.position, player.position
        );

        // ---------- Enemy AI ------------------------
        if (distance <= attackDistance)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsAttacking", true);
        }
        else if (distance <= detectionDistance) {
            // walk

            Vector3 direction = player.position - transform.position;

            direction.y = 0f;
            direction.Normalize();

            transform.rotation = Quaternion.LookRotation(direction);

            characterController.Move(direction * moveSpeed * Time.deltaTime);

            animator.SetFloat("Speed", 1f);
            animator.SetBool("IsAttacking", false);
        }
        else
        {
            // idle
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsAttacking", false);
        }
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
        isDead = true;

        if (animator != null)
        {
            animator.enabled = false;
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

        if (characterController != null)
        {
            characterController.enabled = !enabled;
        }
    }

    public void ApplyBulletImpact(Vector3 direction, float force)
    {
        if (characterController == null || !characterController.enabled)
            return;
        
        characterController.Move(direction.normalized * force);
    }
}
