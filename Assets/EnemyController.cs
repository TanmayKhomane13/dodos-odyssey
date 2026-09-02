using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy Health: " + health);
    }
}
