using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Health
    public float health = 100f;

    // Speeds
    public float moveSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 2f;

    private Vector3 velocity;

    private CharacterController controller;
    private Animator animator;
    public Transform cameraTransform;

    // GUN SECTION
    private Transform rightHand;
    public GameObject gun;
    public Transform aimPoint;
    public GameObject bulletPrefab;
    private Vector3 bulletTarget;
    private bool canShoot = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // bone based weapon attachment
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        gun.transform.SetParent(rightHand);
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit hit;

        // Input / state
        bool grounded = controller.isGrounded;
        bool jumpPressed = Input.GetButtonDown("Jump");   
        bool shootPressed = Input.GetMouseButtonDown(0); // checks if LEFT MOUSE BUTTON is pressed  

        // shooting
        if (shootPressed && canShoot && Physics.Raycast(ray, out hit))
        {
            canShoot = false;

            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);

            animator.SetBool("IsShooting", true);

            bulletTarget = hit.point;
            Invoke(nameof(ShootBullet), 0.6f);
            Invoke(nameof(StopShooting), 0.5f);
            Invoke(nameof(EnableShooting), 0.5f);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // movement
        // Vector3 movement = new Vector3(horizontal, 0f, vertical);
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * vertical + cameraRight * horizontal;

        float speed = movement.magnitude;
        animator.SetFloat("Speed", speed);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && movement.magnitude > 0.1f;
        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        animator.SetBool("IsRunning", isRunning);

        // Rotate the player toward the movement direction
        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Move the player
        controller.Move(movement * currentSpeed * Time.deltaTime);

        // Ground Handling
        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump
        if (grounded && jumpPressed)
        {
            velocity.y = Mathf.Sqrt(
                jumpHeight * -2f * Physics.gravity.y
            );
            animator.SetBool("IsJumping", true);
        }
        
        // Gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;

        // Vertical movement
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    // helpers

    // function to take damage
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
        Debug.Log("Player Died!");
    }

    // ----- Shooting related -----------
    void StopShooting()
    {
        animator.SetBool("IsShooting", false);
    }

    void ShootBullet()
    {
        Vector3 bulletDirection = bulletTarget - gun.transform.position;
        bulletDirection.Normalize();

        Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection);

        Instantiate(bulletPrefab, gun.transform.position, bulletRotation);
    }

    void EnableShooting()
    {
        canShoot = true;
    }
    // -------------------------------------

    // ---------- HIT ---------------------
    public void TakeHit()
    {
        animator.SetTrigger("Hit");
        TakeDamage(10f);
    }
}