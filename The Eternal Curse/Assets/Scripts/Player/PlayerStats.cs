using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int attackSpeed = 1;
    private int maxMana = 100;
    public int mana;
    private int maxArmor = 100;
    public int armor;
    public int MaxMana => maxMana;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mana = maxMana;
        armor = maxArmor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useMana(int amount)
    {
        if (mana >= amount)
        {
            mana -= amount;
        }
    }
}
