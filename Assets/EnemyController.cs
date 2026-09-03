using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy Health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Alien Died!");
        Destroy(gameObject);
    }
}
