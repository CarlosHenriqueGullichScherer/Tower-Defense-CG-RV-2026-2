
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    [SerializeField] private Sprite[] aimSprites = new Sprite[8];
    private bool isAnimating;

    private SpriteRenderer spriteRenderer;

    public int CurrentDirection { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AimAt(Transform target)
    {
        Vector2 direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        int index = Mathf.RoundToInt((90f - angle) / 45f);

        index = (index % 8 + 8) % 8;

        CurrentDirection = index;
        if (!isAnimating)
        {
            spriteRenderer.sprite = aimSprites[index];
        }
    }

    public void SetAnimating(bool value)
    {
        isAnimating = value;

        if (!isAnimating)
        {
            spriteRenderer.sprite = aimSprites[CurrentDirection];
        }
    }
}
