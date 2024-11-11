using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Check if fire damage is disabled
            if (FireChoice.fireDamageDisabled && gameObject.CompareTag("Fire"))
            {
                Debug.Log("Player is immune to fire damage.");
                return;
            }

            collision.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}
