using UnityEngine;
using UnityEngine.UI;

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

    private float poisonValue, frostValue, burnValue;
    private float poisonTarget, frostTarget, burnTarget;

    private float maxStatus = 100f;

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
        
        // Only change visibility if it's different from current state
        if (bar.gameObject.activeSelf != shouldBeVisible)
        {
            bar.gameObject.SetActive(shouldBeVisible);
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
        
        // Force immediate UI update
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
