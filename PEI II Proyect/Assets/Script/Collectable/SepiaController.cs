using Unity.VisualScripting;
using UnityEngine;

public class SepiasController : MonoBehaviour
{
    private SphereCollider sphereCollider;
    private bool follow = false;

    private Vector3 direction;
    [Header("Valores de moneda")]
    [SerializeField] private int valor = 1;
    [Header("Variables de movimiento")]
    [SerializeField] private float velocityMovement = 5f;
    [SerializeField] private float minDistanceToFollowPlayer = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
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
            transform.position += direction * velocityMovement * Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.playerStats.sepias += valor;
            Destroy(this.gameObject);
        }
    }
}
