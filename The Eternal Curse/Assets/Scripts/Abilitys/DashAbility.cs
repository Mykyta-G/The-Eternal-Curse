using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class DashAbility : Ability
{
    [Header("Dash Settings")]
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    
    private GameObject playerObject;
    private PlayerMove playerMove;
    private TrailRenderer trailRenderer;
    private Rigidbody2D playerRigidbody;
    private Health playerHealth;
    private MonoBehaviour coroutineRunner;
    
    public override void Activate()
    {
        // Get references from the player
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerMove = playerObject.GetComponent<PlayerMove>();
                trailRenderer = playerObject.GetComponentInChildren<TrailRenderer>();
                playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
                playerHealth = playerObject.GetComponent<Health>();
                coroutineRunner = playerObject.GetComponent<MonoBehaviour>();
            }
        }
        
        if (coroutineRunner != null)
        {
            coroutineRunner.StartCoroutine(PerformDash());
        }
    }
    
    private IEnumerator PerformDash()
    {
        // Make player invincible during dash
        if (playerHealth != null)
        {
            playerHealth.isInvincible = true;
        }
        
        // Get dash direction from player input
        Vector2 dashDirection = Vector2.zero;
        if (playerMove != null)
        {
            dashDirection = playerMove.GetMoveInput();
        }
        
        if (dashDirection.magnitude == 0)
        {
            dashDirection = Vector2.right * Mathf.Sign(playerObject.transform.localScale.x);
        }
        dashDirection.Normalize();
        
        // Apply dash force
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = dashDirection * dashingPower;
        }
        
        // Enable trail effect
        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }
        
        // Wait for dash duration
        yield return new WaitForSeconds(dashingTime);
        
        // Disable trail effect
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
        
        // Remove invincibility after dash ends
        if (playerHealth != null)
        {
            playerHealth.isInvincible = false;
        }
        
        // Stop movement
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
        }
    }
}
