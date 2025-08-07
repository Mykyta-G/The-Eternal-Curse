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
            Debug.Log($"[{gameObject.name}] Dealt {damage} damage to {other.gameObject.name}");

            if (enableStatusEffects)
            {
                Debug.Log($"[{gameObject.name}] Status effects enabled, checking for EntityStatusEffect on {other.gameObject.name}");
                
                EntityStatusEffect targetStatus = other.gameObject.GetComponent<EntityStatusEffect>();
                if (targetStatus != null)
                {
                    Debug.Log($"[{gameObject.name}] Found EntityStatusEffect on {other.gameObject.name}, initializing with maxStatus: {maxStatus}");
                    targetStatus.Initialize(maxStatus);

                    if (statusEffects != null && statusEffects.Length > 0)
                    {
                        Debug.Log($"[{gameObject.name}] Applying {statusEffects.Length} status effects");
                        foreach (var effect in statusEffects)
                        {
                            if (effect.effectType != StatusEffectType.None)
                            {
                                Debug.Log($"[{gameObject.name}] Applying {effect.effectType} with amount: {effect.amountPerHit}");
                                targetStatus.ApplyEffect(effect.effectType, effect.amountPerHit);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[{gameObject.name}] No status effects configured in statusEffects array");
                    }
                }
                else
                {
                    Debug.LogWarning($"[{gameObject.name}] No EntityStatusEffect component found on {other.gameObject.name}");
                }
            }
            else
            {
                Debug.Log($"[{gameObject.name}] Status effects disabled");
            }
        }
        else
        {
            if (targetHealth == null)
            {
                Debug.Log($"[{gameObject.name}] No Health component found on {other.gameObject.name}");
            }
            else if (targetHealth.isInvincible)
            {
                Debug.Log($"[{gameObject.name}] {other.gameObject.name} is invincible, no damage dealt");
            }
        }
    }
}
