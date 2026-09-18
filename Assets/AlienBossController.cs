using UnityEngine;

public class AlienBossController : MonoBehaviour
{
    // BASICS
    public float health = 100f;
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private CharacterController characterController;
    private Animator animator;
    public Transform player;
    public float attackDistance = 5f;
    public float detectionDistance = 20f;
    public float moveSpeed = 10f;
    private bool isDead = false;
    private bool playerDetected = false;
    public float attackCooldown = 1f;

    // GUN SECTION
    private Transform rightHand;
    public GameObject gun;
    public Transform MuzzleUp;
    public Transform MuzzleDown;
    public GameObject bulletPrefab;
    private bool canShoot = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        //SetRagdoll(false);

        // bone based weapon attachment
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        gun.transform.SetParent(rightHand);

    }

    void Update()
    {
        // check if boss dead
        if (isDead)
            return;
        
        if (player == null)
            return;
        
        // Calculate distance between BOSS & PLAYER
        float distance = Vector3.Distance(
            transform.position, player.position
        );

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // ----------- ENEMY AI ----------------
        
        // detect player
        if (!playerDetected)
        {
            if (distance <= detectionDistance && angle <= 100f)
            {
                playerDetected = true;
            }
            else
            {
                animator.SetFloat("Speed", 0f);
                animator.SetBool("isShooting", false);
                return;
            }
        }

        // shoot
        if (distance <= attackDistance)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);

            animator.SetFloat("Speed", 0f);
            animator.SetBool("isShooting", true);

            StartShooting();
        }

        // follow 
        else
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
            characterController.Move(directionToPlayer * moveSpeed * Time.deltaTime);
            animator.SetFloat("Speed", 1f);
            animator.SetBool("isShooting", false);
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

    // -------- SHOOTING ------------------
    void ShootBullet()
    {
        Debug.Log("Shoot Bullet Called");
        Vector3 targetPosition = player.position + Vector3.up * 1.2f;
        Vector3 bulletDirection = targetPosition - MuzzleUp.position;

        Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection);
        Instantiate(bulletPrefab, MuzzleUp.position, bulletRotation);
        Instantiate(bulletPrefab, MuzzleDown.position, bulletRotation);
    }

    void StartShooting()
    {
        if (!canShoot)
            return;
        
        canShoot = false;

        Invoke(nameof(ShootBullet), 0.6f);
        Invoke(nameof(EnableShooting), attackCooldown);
    }

    public void ApplyBulletImpact(Vector3 direction, float force)
    {
        if (characterController == null || !characterController.enabled)
            return;
        
        characterController.Move(direction.normalized * force);
    }

    void EnableShooting()
    {
        canShoot = true;
    }
}
