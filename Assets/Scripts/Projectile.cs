
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float damage = 2f;

    [SerializeField] private Sprite[] animationSprites = new Sprite[4];
    [SerializeField] private float frameDuration = 0.1f;

    [SerializeField] private Sprite[] impactSprites = new Sprite[6];
    [SerializeField] private float impactFrameDuration = 0.08f;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private float collisionRadius = 0.15f;
    private bool isImpacting;

    private Enemy target;
    private SpriteRenderer spriteRenderer;

    private float animationTimer;
    private int currentFrame;
    [SerializeField] private float maxDistanceAfterTargetLost = 15f;

    private Vector2 lastDirection;
    private bool targetLost;
    private float distanceAfterTargetLost;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(Enemy newTarget)
    {
        target = newTarget;

        if (target != null)
        {
            lastDirection = (target.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        if (isImpacting) return;

        if (target == null || target.Health <= 0f)
        {
            targetLost = true;
        }

        if (!targetLost)
        {
            Vector2 direction = target.transform.position - transform.position;

            if (direction != Vector2.zero)
                lastDirection = direction.normalized;

            transform.position = Vector2.MoveTowards(
                transform.position,
                target.transform.position,
                speed * Time.deltaTime
            );

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                target.TakeDamage(damage);
                AudioManager.Instance.PlaySFX(impactSound);
                StartCoroutine(PlayImpactAnimation());
                return;
            }
        }
        else
        {
            float distance = speed * Time.deltaTime;

            transform.position += (Vector3)(lastDirection * distance);

            distanceAfterTargetLost += distance;

            if (distanceAfterTargetLost >= maxDistanceAfterTargetLost)
            {
                Destroy(gameObject);
                return;
            }
        }

        if (CheckCollision())
            return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameDuration)
        {
            animationTimer = 0f;

            currentFrame = (currentFrame + 1) % animationSprites.Length;

            spriteRenderer.sprite = animationSprites[currentFrame];
        }
    }
    private IEnumerator PlayImpactAnimation()
    {
        isImpacting = true;

        for (int i = 0; i < impactSprites.Length; i++)
        {
            spriteRenderer.sprite = impactSprites[i];
            yield return new WaitForSeconds(impactFrameDuration);
        }

        Destroy(gameObject);
    }

    private bool CheckCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            collisionRadius
        );

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy == null || enemy.Health <= 0f)
                continue;

            if (enemy == target && !targetLost)
                continue;

            enemy.TakeDamage(damage);
            AudioManager.Instance.PlaySFX(impactSound);
            StartCoroutine(PlayImpactAnimation());

            return true;
        }

        return false;
    }

}
