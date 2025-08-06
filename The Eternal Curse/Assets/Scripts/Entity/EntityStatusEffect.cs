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

    private float fillSpeed = 0.25f;
    private float maxStatus = 100f;

    public void Initialize(float fillSpeed, float maxStatus)
    {
        this.fillSpeed = fillSpeed;
        this.maxStatus = maxStatus;
    }

    void Update()
    {
        poisonValue = Mathf.MoveTowards(poisonValue, poisonTarget, fillSpeed * Time.deltaTime * maxStatus);
        frostValue = Mathf.MoveTowards(frostValue, frostTarget, fillSpeed * Time.deltaTime * maxStatus);
        burnValue = Mathf.MoveTowards(burnValue, burnTarget, fillSpeed * Time.deltaTime * maxStatus);

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
        if (bar == null) return;

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
                Debug.Log($"Poison has been applied to {gameObject.name} for {amount} points.");
                break;

            case StatusEffectType.Frost:
                frostTarget = Mathf.Clamp(frostTarget + amount, 0f, maxStatus);
                Debug.Log($"Frost has been applied to {gameObject.name} for {amount} points.");
                break;

            case StatusEffectType.Burn:
                burnTarget = Mathf.Clamp(burnTarget + amount, 0f, maxStatus);
                Debug.Log($"Burn has been applied to {gameObject.name} for {amount} points.");
                break;

            case StatusEffectType.None:
            default:
                Debug.Log("No status effect applied.");
                break;
        }
    }
}
