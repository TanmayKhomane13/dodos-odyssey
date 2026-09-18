using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private bool hasHit = false;
    public float impactForce = 0.1f;
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

        if (enemy != null)
        {
            hasHit = true;
            enemy.TakeDamage(10f);
            Vector3 impactDirection = transform.forward;
            enemy.ApplyBulletImpact(impactDirection, impactForce);
            Destroy(gameObject);
        }

        // instantiate particle system effect
        Instantiate(
            impactEffect, collision.contacts[0].point, Quaternion.identity
        );
    }
}
