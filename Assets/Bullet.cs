using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(10f);
        }

        Destroy(gameObject);
    }
}
