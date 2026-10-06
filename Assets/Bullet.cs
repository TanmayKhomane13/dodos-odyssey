using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Basics")]
    public float speed = 20f;
    private bool hasHit = false;

    // Effects on Impact
    [Header("Effects on Impact")]
    public float impactForce = 0.3f;
    public GameObject impactEffect;
   
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) 
            return;
        

        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        AlienBossController alienBoss = collision.gameObject.GetComponentInParent<AlienBossController>();

        Vector3 impactDirection = transform.forward;

        // Normal Enemy
        if (enemy != null)
        {
            hasHit = true;
            enemy.TakeDamage(10f);
            enemy.ApplyBulletImpact(impactDirection, impactForce);
        }

        // Alien Boss
        else if (alienBoss != null)
        {
            hasHit = true;
            alienBoss.TakeDamage(10f);
            alienBoss.ApplyBulletImpact(impactDirection, impactForce);
        }

        // instantiate particle system effect
        Instantiate(
            impactEffect, collision.contacts[0].point, Quaternion.identity
        );

        // Destroy bullet for ANY collision
        Destroy(gameObject);
    }
}
