using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum StatusEffectType
{
    None,
    Poison,
    Frost,
    Burn
}

public class EntityStatusEffect : MonoBehaviour
{
    [Header("UI Components")]
    public Image Poison;
    public Image Frost;
    public Image Burn;

    [Header("Position Settings")]
    public float barSpacing = 30f; // Distance between bars
    public float startYPosition = 30f; // Starting Y position for first bar

    private float poisonValue, frostValue, burnValue;
    private float poisonTarget, frostTarget, burnTarget;

    private float maxStatus = 100f;

    // Track which effects are active and their order
    private List<StatusEffectType> activeEffects = new List<StatusEffectType>();

    public void Initialize(float maxStatus)
    {
        this.maxStatus = Mathf.Max(1f, maxStatus);
    }

    void Update()
    {
        // Update current values from targets
        poisonValue = poisonTarget;
        frostValue = frostTarget;
        burnValue = burnTarget;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateBar(Poison, poisonValue, "Poison");
        UpdateBar(Frost, frostValue, "Frost");
        UpdateBar(Burn, burnValue, "Burn");
        
        // Update positions based on active effects
        UpdatePositions();
    }

    private void UpdateBar(Image bar, float value, string effectName)
    {
        if (bar == null) 
        {
            Debug.LogWarning($"[{gameObject.name}] {effectName} UI Image is not assigned!");
            return;
        }

        bool shouldBeVisible = value > 0f;
        bar.fillAmount = value / maxStatus;
        
        if (bar.gameObject.activeSelf != shouldBeVisible)
        {
            bar.gameObject.SetActive(shouldBeVisible);
        }
    }

    private void UpdatePositions()
    {
        activeEffects.Clear();
        
        if (poisonValue > 0f) activeEffects.Add(StatusEffectType.Poison);
        if (frostValue > 0f) activeEffects.Add(StatusEffectType.Frost);
        if (burnValue > 0f) activeEffects.Add(StatusEffectType.Burn);

        // Position each active effect
        for (int i = 0; i < activeEffects.Count; i++)
        {
            Image effectBar = GetBarForEffect(activeEffects[i]);
            
            if (effectBar != null)
            {
                RectTransform rectTransform = effectBar.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    float yPos = startYPosition - (i * barSpacing);
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPos);
                }
            }
        }
    }

    private Image GetBarForEffect(StatusEffectType effectType)
    {
        switch (effectType)
        {
            case StatusEffectType.Poison: return Poison;
            case StatusEffectType.Frost: return Frost;
            case StatusEffectType.Burn: return Burn;
            default: return null;
        }
    }

    public void ApplyEffect(StatusEffectType effectType, float amount)
    {
        switch (effectType)
        {
            case StatusEffectType.Poison:
                poisonTarget = Mathf.Clamp(poisonTarget + amount, 0f, maxStatus);
                if (Poison == null) Debug.LogWarning($"[{gameObject.name}] Poison UI Image is not assigned!");
                break;

            case StatusEffectType.Frost:
                frostTarget = Mathf.Clamp(frostTarget + amount, 0f, maxStatus);
                if (Frost == null) Debug.LogWarning($"[{gameObject.name}] Frost UI Image is not assigned!");
                break;

            case StatusEffectType.Burn:
                burnTarget = Mathf.Clamp(burnTarget + amount, 0f, maxStatus);
                if (Burn == null) Debug.LogWarning($"[{gameObject.name}] Burn UI Image is not assigned!");
                break;

            case StatusEffectType.None:
            default:
                break;
        }
        
        UpdateUI();
    }

    void Start()
    {
        // Check if UI components are assigned
        if (Poison == null) Debug.LogWarning($"[{gameObject.name}] Poison UI Image is not assigned!");
        if (Frost == null) Debug.LogWarning($"[{gameObject.name}] Frost UI Image is not assigned!");
        if (Burn == null) Debug.LogWarning($"[{gameObject.name}] Burn UI Image is not assigned!");
    }
}
