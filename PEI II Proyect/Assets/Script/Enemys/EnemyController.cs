using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] public float life = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
