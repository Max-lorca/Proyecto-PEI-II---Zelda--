using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;

    private bool inParry = false;
    private Transform originSpawn;

    [SerializeField] private int damage = 1;
    [SerializeField] private float autoDestructionTime = 10f;

    [SerializeField] private Vector3 velocidadInicial;
    [SerializeField] private float fuerzaHorizontal;
    [SerializeField] private float fuerzaVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Transform playerReference = GameplayManager.instance.GetPlayerReference().transform;
        ParabolicMovement(playerReference);

        Destroy(gameObject, autoDestructionTime);
    }

    private void ParabolicMovement(Transform target)
    {
        Vector3 dir = (target.position - this.transform.position).normalized;
        dir.y = 0f;

        velocidadInicial = dir * fuerzaHorizontal + Vector3.up * fuerzaVertical;

        rb.linearVelocity = velocidadInicial;
    }

    public void SetOrigin(Transform parent)
    {
        this.originSpawn = parent;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(this.damage);
                Destroy(this.gameObject);
                break;
            case "MagicShield":
                inParry = true;
                ParabolicMovement(originSpawn);
                break;
            case "OctoRock":
                if (!inParry) return;
                else
                {
                    OctoRock octoRock = collision.gameObject.GetComponent<OctoRock>();
                    octoRock.TakeDamage(this.damage);
                }
                    break;
        }
    }
}
