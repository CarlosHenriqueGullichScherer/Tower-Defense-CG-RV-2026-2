using Unity.VisualScripting;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public void takeDamage(float damage)
    {
        health -= damage;
        if(health <=  0f) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            Enemy enemyHealth = other.gameObject.GetComponent<Enemy>();
            takeDamage(enemyHealth.Health);
            Destroy(other.gameObject);
            Debug.Log("vida da base:" + health);
        } 
    }
}
