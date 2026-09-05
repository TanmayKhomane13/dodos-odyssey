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
    private bool playerDetected = false;

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
        
        // Calculate distance between ENEMY & PLAYER
        float distance = Vector3.Distance(
            transform.position, player.position
        );

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // ---------- Enemy AI ------------------------
        
        // detect player
        if (!playerDetected)
        {
            if (distance <= detectionDistance && angle <= 60f)
            {
                playerDetected = true;
            }
            else
            {
                animator.SetFloat("Speed", 0f);
                animator.SetBool("IsAttacking", false);
                return;
            }
        }

        // attack
        if (distance <= attackDistance)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);

            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsAttacking", true);
        }

        // follow
        else
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);

            characterController.Move(directionToPlayer * moveSpeed * Time.deltaTime);

            animator.SetFloat("Speed", 1f);
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
