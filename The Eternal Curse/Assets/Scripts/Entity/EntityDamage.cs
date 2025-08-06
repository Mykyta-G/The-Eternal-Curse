using UnityEngine;

[System.Serializable]
public class StatusEffectData
{
    public StatusEffectType effectType;
    [Range(0f, 100f)]
    public float amountPerHit = 25f;
}

public class EntityDamage : MonoBehaviour
{
    public int damage = 10;
    public bool enableStatusEffects = false;
    public float fillSpeed = 0.25f;
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
                    targetStatus.Initialize(fillSpeed, maxStatus);

                    foreach (var effect in statusEffects)
                    {
                        if (effect.effectType != StatusEffectType.None)
                        {
                            targetStatus.ApplyEffect(effect.effectType, effect.amountPerHit);
                        }
                    }
                }
            }
        }
    }
}
