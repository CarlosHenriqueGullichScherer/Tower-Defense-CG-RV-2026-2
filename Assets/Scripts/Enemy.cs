using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float health = 10f;

    public float Health {
        get { return health; }
    }

    private Transform[] waypoints;
    private int currentWaypoint = 0;
    
    public void SetWayPoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
 
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f) Destroy(gameObject);
    }
}
