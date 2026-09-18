using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(10f);
        }

        Destroy(gameObject);
    }
}
