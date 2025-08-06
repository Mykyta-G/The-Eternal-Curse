using UnityEngine;
using UnityEngine.UI;

public class DragPreview : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        gameObject.SetActive(false);
    }
    
    public void Show(Sprite sprite)
    {
        previewImage.sprite = sprite;
        gameObject.SetActive(true);
        canvasGroup.alpha = 0.7f;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void UpdatePosition(Vector2 screenPosition)
    {
        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            rectTransform.position = screenPosition;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                screenPosition,
                parentCanvas.worldCamera,
                out Vector2 localPoint);
            rectTransform.localPosition = localPoint;
        }
    }
}