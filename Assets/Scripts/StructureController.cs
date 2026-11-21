using UnityEngine;

public class StructureController : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private TurretAim turretAim;
    [SerializeField] private TurretAnimation turretAnimation;

    private Enemy target;
    private float nextAttackTime;

    void Update()
    {
        if (target != null)
        {
            turretAim.AimAt(target.transform);
        }

        if (target == null || Vector2.Distance(transform.position, target.transform.position) > range)
        {
            FindTarget();
        }

        if (target != null && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    private void FindTarget()
    {
        target = null;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D enemyCollider in enemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();

            if (enemy != null)
            {
                target = enemy;
                break;
            }
        }
    }

    private void Attack()
    {
        if (target == null) return;

        turretAnimation.PlayFireAnimation(turretAim.CurrentDirection);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetTarget(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}