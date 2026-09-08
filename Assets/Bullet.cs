using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float impactForce = 0.1f;
    public GameObject impactEffect;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(10f);

            Vector3 impactDirection = transform.forward;

            enemy.ApplyBulletImpact(impactDirection, impactForce);
        }

        // instantiate particle system effect
        Instantiate(
            impactEffect, collision.contacts[0].point, Quaternion.identity
        );

        Destroy(gameObject);
    }
}
