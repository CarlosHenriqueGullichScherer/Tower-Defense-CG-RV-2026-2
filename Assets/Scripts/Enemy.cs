using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

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
                ReachBase();
            }
        }
    }
    private void ReachBase()
    {
        Destroy(gameObject);

        Debug.Log("Enemy reached the base");
    }
}
