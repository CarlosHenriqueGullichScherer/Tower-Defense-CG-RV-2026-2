
using UnityEngine;
using System.Collections;

public class TurretAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] fireSprites = new Sprite[32];
    [SerializeField] private float frameDuration = 0.1f;
    [SerializeField] private TurretAim turretAim;

    private SpriteRenderer spriteRenderer;
    private Coroutine currentAnimation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayFireAnimation(int direction)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation = StartCoroutine(AnimateFire(direction));
    }

    private IEnumerator AnimateFire(int direction)
    {
        turretAim.SetAnimating(true);

        int startIndex = direction * 4;

        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.sprite = fireSprites[startIndex + i];
            yield return new WaitForSeconds(frameDuration);
        }

        turretAim.SetAnimating(false);

        currentAnimation = null;
    }

}
