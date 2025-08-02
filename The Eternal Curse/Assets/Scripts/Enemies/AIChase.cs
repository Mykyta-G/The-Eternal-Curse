using UnityEngine;

public class AIChase : MonoBehaviour
{
    [Header("Target")]
    public GameObject player;
    
    [Header("Movement")]
    public float speed;
    public float distanceBeforeChase;
    
    [Header("Distances")]
    public float stoppingDistance;
    public float retreatDistance;
    public float chaseDistance;
    public float attackDistance;
    
    [Header("Behavior")]
    public bool canRetreat = true;
    
    private float distance;
    private Vector2 direction;
    private Vector3 originalPosition;
    private bool isRetreating = false;
    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        originalPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;
        
        distance = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;

        if (distance > retreatDistance && canRetreat && !isRetreating)
        {
            isRetreating = true;
        }
        
        if (isRetreating)
        {
            float distanceToOriginal = Vector2.Distance(transform.position, originalPosition);
            if (distanceToOriginal > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
            }
            else
            {
                isRetreating = false; 
            }
        }
        else if (distance < chaseDistance && distance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (distance < attackDistance)
        {
            // Attack logic here
            Debug.Log("Attack");
        }
    }
}
