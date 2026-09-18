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
    public Transform aimPoint;
    public GameObject bulletPrefab;
    private Vector3 bulletTarget;
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
        
    }
}
