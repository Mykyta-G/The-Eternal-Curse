using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Health>() != null && 
            !other.gameObject.GetComponent<Health>().isInvincible && 
            other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
