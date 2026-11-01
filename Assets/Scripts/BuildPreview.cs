using UnityEngine;

public class BuildPreview : MonoBehaviour
{
    [SerializeField] private Collider2D buildArea;
    [SerializeField] private LayerMask invalidLayers;
    [SerializeField] private float checkRadius = 0.5f;

    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;

    private SpriteRenderer[] renderers;

    public bool CanBuild { get; private set; }

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void UpdatePreview(Vector3 position)
    {
        transform.position = position;

        Vector2 center = position;

        bool insideArea =
            buildArea != null &&
            buildArea.OverlapPoint(center) &&
            buildArea.OverlapPoint(center + Vector2.up * checkRadius) &&
            buildArea.OverlapPoint(center + Vector2.down * checkRadius) &&
            buildArea.OverlapPoint(center + Vector2.left * checkRadius) &&
            buildArea.OverlapPoint(center + Vector2.right * checkRadius);

        bool blocked = Physics2D.OverlapCircle(center, checkRadius, invalidLayers) != null;

        CanBuild = insideArea && !blocked;

        UpdateColor();
    }

    private void UpdateColor()
    {
        Color color = CanBuild ? validColor : invalidColor;

        foreach (SpriteRenderer sprite in renderers)
        {
            sprite.color = color;
        }
    }
}