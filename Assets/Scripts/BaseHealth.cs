using Unity.VisualScripting;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float health { get; set; } = 100f;
    public void takeDamage(float damage)
    {
        health -= damage;
        if(health <=  0f) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            takeDamage(enemyHealth.health);
            Destroy(other.gameObject);
            Debug.Log(health);
        } 
    }
}
