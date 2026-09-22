using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health { get; set; } = 10f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f) Destroy(gameObject);
    }
}
