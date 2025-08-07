using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    // Public property to access moveSpeed for status effects
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    
    private Vector2 moveInput;
    private PlayerInput playerInput;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f; 
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    private Rigidbody2D rb;
    private Health playerHealth;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<Health>(); 
        
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }
    
    void Update()
    {
        if (isDashing) return; // Movement is disabled during dash

        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return; // Movement is disabled during dash
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        // Make player invincible during dash
        if (playerHealth != null)
        {
            playerHealth.isInvincible = true;
        }
        
        Vector2 dashDirection = moveInput;
        if (dashDirection.magnitude == 0)
        {
            dashDirection = Vector2.right * Mathf.Sign(transform.localScale.x);
        }
        dashDirection.Normalize(); 
        
        if (rb != null)
        {
            rb.linearVelocity = dashDirection * dashingPower;
        }
        
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        
        // Remove invincibility after dash ends
        if (playerHealth != null)
        {
            playerHealth.isInvincible = false;
        }
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}