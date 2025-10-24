using Unity.VisualScripting;
using UnityEngine;

public class SepiasController : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private bool follow = false;

    private Vector3 direction;
    [Header("Valores")]
    [SerializeField] private int valor = 1;
    [SerializeField] private float velocityMovement = 5f;
    [SerializeField] private float minDistanceToFollowPlayer = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        
    }
    void Update()
    {
        Transform player = GameplayManager.instance.GetPlayerReference().transform;
        float distanceOfPlayer = Vector3.Distance(transform.position, player.position);

        direction = (player.position - transform.position).normalized;
        if (distanceOfPlayer <= minDistanceToFollowPlayer)
            follow = true;
    }
    private void FixedUpdate()
    {
        if (follow)
        {
            rb.AddForce(direction * velocityMovement); 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.playerStats.sepias += valor;
        }
    }
}
