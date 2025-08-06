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
        poisonValue = poisonTarget;
        frostValue = frostTarget;
        burnValue = burnTarget;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateBar(Poison, poisonValue);
        UpdateBar(Frost, frostValue);
        UpdateBar(Burn, burnValue);
    }

    private void UpdateBar(Image bar, float value)
    {
        if (bar == null) 
        {
            Debug.LogWarning($"Status effect bar is null! Value: {value}");
            return;
        }

        bool shouldBeVisible = value > 0f;
        bar.fillAmount = value / maxStatus;
        bar.gameObject.SetActive(shouldBeVisible);
    }

    public void ApplyEffect(StatusEffectType effectType, float amount)
    {
        switch (effectType)
        {
            case StatusEffectType.Poison:
                poisonTarget = Mathf.Clamp(poisonTarget + amount, 0f, maxStatus);
                Debug.Log($"Poison applied to {gameObject.name}: +{amount} (Total: {poisonTarget})");
                if (Poison == null) Debug.LogWarning("Poison UI Image is not assigned!");
                break;

            case StatusEffectType.Frost:
                frostTarget = Mathf.Clamp(frostTarget + amount, 0f, maxStatus);
                Debug.Log($"Frost applied to {gameObject.name}: +{amount} (Total: {frostTarget})");
                if (Frost == null) Debug.LogWarning("Frost UI Image is not assigned!");
                break;

            case StatusEffectType.Burn:
                burnTarget = Mathf.Clamp(burnTarget + amount, 0f, maxStatus);
                Debug.Log($"Burn applied to {gameObject.name}: +{amount} (Total: {burnTarget})");
                if (Burn == null) Debug.LogWarning("Burn UI Image is not assigned!");
                break;

            case StatusEffectType.None:
            default:
                Debug.Log("No status effect applied.");
                break;
        }
    }

    // Temporary debug method - call this from Start() to test if UI bars work
    void Start()
    {
        // Uncomment the line below to test if UI bars show up
        // TestUIBars();
    }
    
    // Temporary debug method - uncomment the call in Start() to test
    void TestUIBars()
    {
        Debug.Log("Testing UI bars...");
        poisonTarget = 50f;
        frostTarget = 75f;
        burnTarget = 25f;
        Debug.Log($"Set test values - Poison: {poisonTarget}, Frost: {frostTarget}, Burn: {burnTarget}");
    }
}
