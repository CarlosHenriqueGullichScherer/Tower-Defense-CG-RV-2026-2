
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [Header("Health")]
    [SerializeField] private float health = 10f;
    [SerializeField] private int points = 10;
    [SerializeField] private int gold = 10;

    [Header("Animation")]
    [SerializeField] private Sprite[] walkSprites = new Sprite[24];
    [SerializeField] private Sprite[] damageSprites = new Sprite[8];
    [SerializeField] private Sprite[] deathSprites = new Sprite[24];
    [SerializeField] private float frameDuration = 0.1f;

    private SpriteRenderer spriteRenderer;

    private Transform[] waypoints;
    private int currentWaypoint = 0;

    private int currentDirection = 2;
    private int currentFrame = 0;
    private float animationTimer;

    private bool dead;
    private bool takingDamage;

    private Coroutine damageCoroutine;

    public float Health
    {
        get { return health; }
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = walkSprites[12];
    }

    public void SetWayPoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    void Update()
    {
        if (dead || takingDamage) return;

        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];

        Vector2 direction = target.position - transform.position;

        UpdateDirection(direction);

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                dead = true;
                StartCoroutine(PlayDeathAnimation());
                return;
            }
        }

        AnimateWalk();
    }

    private void UpdateDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            currentDirection = direction.x > 0 ? 1 : 3;
        }
        else
        {
            currentDirection = direction.y > 0 ? 0 : 2;
        }
    }

    private void AnimateWalk()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer >= frameDuration)
        {
            animationTimer = 0f;

            currentFrame = (currentFrame + 1) % 6;

            int index = currentDirection * 6 + currentFrame;

            spriteRenderer.sprite = walkSprites[index];
        }
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;

        health -= damage;

        if (health <= 0f)
        {
            dead = true;

            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);

            if (GameSession.Instance != null)
                GameSession.Instance.RegisterKill(points, gold);

            StartCoroutine(PlayDeathAnimation());
            return;
        }

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(PlayDamageAnimation());
    }

    private IEnumerator PlayDamageAnimation()
    {
        takingDamage = true;

        for (int i = 0; i < 2; i++)
        {
            int index = currentDirection * 2 + i;

            spriteRenderer.sprite = damageSprites[index];

            yield return new WaitForSeconds(frameDuration);
        }

        takingDamage = false;
        damageCoroutine = null;

        currentFrame = 0;
        animationTimer = 0f;
        spriteRenderer.sprite = walkSprites[currentDirection * 6];
    }

    private IEnumerator PlayDeathAnimation()
    {
        for (int i = 0; i < 6; i++)
        {
            int index = currentDirection * 6 + i;

            spriteRenderer.sprite = deathSprites[index];

            yield return new WaitForSeconds(frameDuration);
        }

        Destroy(gameObject);
    }
    public void Die()
    {
        if (dead) return;

        dead = true;

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        StartCoroutine(PlayDeathAnimation());
    }
}
