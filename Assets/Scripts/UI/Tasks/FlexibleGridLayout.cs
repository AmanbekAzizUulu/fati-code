using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FlexibleGridLayout : MonoBehaviour
{
    [Header("Spacing")]
    public float spacingX = 10f;
    public float spacingY = 10f;

    [Header("Padding")]
    public float paddingLeft = 10f;
    public float paddingRight = 10f;
    public float paddingTop = 10f;
    public float paddingBottom = 10f;

    private RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        if (rectTransform == null) return;

        float containerWidth = rectTransform.rect.width;
        float maxWidth = containerWidth - paddingLeft - paddingRight;

        float x = paddingLeft;
        float y = -paddingTop;

        float rowHeight = 0f;

        foreach (RectTransform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            float width = child.rect.width;
            float height = child.rect.height;

            if (x + width > paddingLeft + maxWidth)
            {
                x = paddingLeft;
                y -= rowHeight + spacingY;
                rowHeight = 0f;
            }

            child.anchorMin = new Vector2(0, 1);
            child.anchorMax = new Vector2(0, 1);
            child.pivot = new Vector2(0, 1);

            child.anchoredPosition = new Vector2(x, y);

            x += width + spacingX;
            rowHeight = Mathf.Max(rowHeight, height);
        }
    }
}