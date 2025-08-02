using UnityEngine;

public class EnemyBehavier : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
