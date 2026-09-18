using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    private bool hasHit = false;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) 
            return;

        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            hasHit = true;
            player.TakeDamage(10f);
            Destroy(gameObject);
        }
    }
}
