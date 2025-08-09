using UnityEngine;
using TMPro;

public class InteractionIndicator : MonoBehaviour
{
    [Header("Visual Settings")]
    public string indicatorText = "!";
    public float bobHeight = 0.5f; // How much the indicator bobs up and down
    public float bobSpeed = 2f; // Speed of the bobbing animation
    public Color indicatorColor = Color.yellow;
    public float fontSize = 24f;
    
    [Header("Positioning")]
    public Vector3 offset = new Vector3(0, 2f, 0); // Offset from NPC position
    public bool followNPC = true; // Whether the indicator should follow the NPC
    
    private TextMeshPro indicatorTextMesh;
    private Vector3 startPosition;
    private float bobTime;
    private Transform npcTransform;
    
    void Start()
    {
        SetupTextMesh();
        
        startPosition = transform.position;
        
        if (followNPC)
        {
            npcTransform = transform.parent;
            if (npcTransform == null)
            {
                npcTransform = GetComponentInParent<Transform>();
            }
        }
    }
    
    void SetupTextMesh()
    {
        indicatorTextMesh = GetComponent<TextMeshPro>();
        if (indicatorTextMesh == null)
        {
            indicatorTextMesh = gameObject.AddComponent<TextMeshPro>();
        }
        
        indicatorTextMesh.text = indicatorText;
        indicatorTextMesh.fontSize = fontSize;
        indicatorTextMesh.color = indicatorColor;
        indicatorTextMesh.alignment = TextAlignmentOptions.Center;
        indicatorTextMesh.fontStyle = FontStyles.Bold;
        
        indicatorTextMesh.transform.rotation = Quaternion.identity;
    }
    
    void Update()
    {
        if (npcTransform != null && followNPC)
        {
            // Follow the NPC position
            transform.position = npcTransform.position + offset;
        }
        
        // Bobbing animation
        bobTime += Time.deltaTime * bobSpeed;
        float bobOffset = Mathf.Sin(bobTime) * bobHeight;
        
        Vector3 currentPosition = transform.position;
        currentPosition.y += bobOffset;
        transform.position = currentPosition;
        
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0); 
        }
    }
    
    public void SetIndicatorText(string newText)
    {
        indicatorText = newText;
        if (indicatorTextMesh != null)
        {
            indicatorTextMesh.text = indicatorText;
        }
    }
    
    public void SetIndicatorColor(Color newColor)
    {
        indicatorColor = newColor;
        if (indicatorTextMesh != null)
        {
            indicatorTextMesh.color = indicatorColor;
        }
    }
}
