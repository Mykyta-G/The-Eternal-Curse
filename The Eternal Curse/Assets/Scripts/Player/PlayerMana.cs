using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public Image manaBar;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        UpdateManaBar();
    }

    void Update()
    {
        UpdateManaBar();
    }

    private void UpdateManaBar()
    {
        if (manaBar == null || playerStats == null)
        {
            return;
        }

        float fill = 0f;
        if (playerStats.MaxMana > 0)
        {
            fill = Mathf.Clamp01((float)playerStats.mana / (float)playerStats.MaxMana);
        }
        manaBar.fillAmount = fill;
    }
}
