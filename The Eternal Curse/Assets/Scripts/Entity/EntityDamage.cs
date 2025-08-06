using UnityEngine;

[System.Serializable]
public class StatusEffectData
{
    public StatusEffectType effectType;

    [Range(0f, 100f)]
    [Tooltip("How much the status bar fills per hit")]
    public float amountPerHit = 25f;
}

public class EntityDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10;

    [Header("Status Effect Settings")]
    public bool enableStatusEffects = false;

    [Tooltip("Max value each status effect can reach")]
    public float maxStatus = 100f;

    public StatusEffectData[] statusEffects;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health targetHealth = other.gameObject.GetComponent<Health>();

        if (targetHealth != null && !targetHealth.isInvincible)
        {
            targetHealth.TakeDamage(damage);

            if (enableStatusEffects)
            {
                EntityStatusEffect targetStatus = other.gameObject.GetComponent<EntityStatusEffect>();
                if (targetStatus != null)
                {
                    targetStatus.Initialize(maxStatus);

                    foreach (var effect in statusEffects)
                    {
                        if (effect.effectType != StatusEffectType.None)
                        {
                            targetStatus.ApplyEffect(effect.effectType, effect.amountPerHit);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"No EntityStatusEffect component found on {other.gameObject.name}");
                }
            }
        }
    }
}
