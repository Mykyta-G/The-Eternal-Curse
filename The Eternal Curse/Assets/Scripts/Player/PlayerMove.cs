using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private PlayerInput playerInput;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        
        // Normalize diagonal movement
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }
    
    void Update()
    {
        // Move the player
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}