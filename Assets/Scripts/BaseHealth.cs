using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private float maxHealth;

    public float Health => Mathf.Max(health, 0f);
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        maxHealth = health;
    }

    public void takeDamage(float damage)
    {
        if (health <= 0f) return;

        health -= damage;
        if (health <= 0f)
        {
            if (GameSession.Instance != null) GameSession.Instance.OnBaseDestroyed();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.gameObject.GetComponent<Enemy>();

            takeDamage(enemyHealth.Health);
            enemyHealth.Die();

            Debug.Log("vida da base:" + health);
        }
    }
}
