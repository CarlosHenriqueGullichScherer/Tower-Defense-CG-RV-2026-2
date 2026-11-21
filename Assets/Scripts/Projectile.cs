
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
    private bool isImpacting;

    private Enemy target;
    private SpriteRenderer spriteRenderer;

    private float animationTimer;
    private int currentFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(Enemy newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (isImpacting) return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        Vector2 direction = target.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameDuration)
        {
            animationTimer = 0f;

            currentFrame = (currentFrame + 1) % animationSprites.Length;

            spriteRenderer.sprite = animationSprites[currentFrame];
        }

        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            StartCoroutine(PlayImpactAnimation());
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
}
