using UnityEngine;

public class StatusEffectDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool enableDebugMode = false;
    public KeyCode testPoisonKey = KeyCode.P;
    public KeyCode testFrostKey = KeyCode.F;
    public KeyCode testBurnKey = KeyCode.B;
    public KeyCode clearAllKey = KeyCode.C;
    
    [Header("Test Values")]
    public float testAmount = 25f;
    
    private EntityStatusEffect statusEffect;
    
    void Start()
    {
        statusEffect = GetComponent<EntityStatusEffect>();
        if (statusEffect == null)
        {
            Debug.LogError($"[{gameObject.name}] StatusEffectDebugger: No EntityStatusEffect component found!");
        }
        else
        {
            Debug.Log($"[{gameObject.name}] StatusEffectDebugger: Found EntityStatusEffect component");
        }
    }
    
    void Update()
    {
        if (!enableDebugMode || statusEffect == null) return;
        
        if (Input.GetKeyDown(testPoisonKey))
        {
            Debug.Log($"[{gameObject.name}] Debug: Testing Poison effect");
            statusEffect.ApplyEffect(StatusEffectType.Poison, testAmount);
        }
        
        if (Input.GetKeyDown(testFrostKey))
        {
            Debug.Log($"[{gameObject.name}] Debug: Testing Frost effect");
            statusEffect.ApplyEffect(StatusEffectType.Frost, testAmount);
        }
        
        if (Input.GetKeyDown(testBurnKey))
        {
            Debug.Log($"[{gameObject.name}] Debug: Testing Burn effect");
            statusEffect.ApplyEffect(StatusEffectType.Burn, testAmount);
        }
        
        if (Input.GetKeyDown(clearAllKey))
        {
            Debug.Log($"[{gameObject.name}] Debug: Clearing all status effects");
            statusEffect.ApplyEffect(StatusEffectType.Poison, -1000f); // Clear poison
            statusEffect.ApplyEffect(StatusEffectType.Frost, -1000f);  // Clear frost
            statusEffect.ApplyEffect(StatusEffectType.Burn, -1000f);   // Clear burn
        }
    }
    
    [ContextMenu("Test All Status Effects")]
    void TestAllStatusEffects()
    {
        if (statusEffect == null) return;
        
        Debug.Log($"[{gameObject.name}] Testing all status effects...");
        statusEffect.ApplyEffect(StatusEffectType.Poison, testAmount);
        statusEffect.ApplyEffect(StatusEffectType.Frost, testAmount);
        statusEffect.ApplyEffect(StatusEffectType.Burn, testAmount);
    }
}
