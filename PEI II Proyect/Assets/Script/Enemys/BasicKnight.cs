using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class BasicKnight : MonoBehaviour
{
    // PRIVATE 
    private float distanceOfPlayer;
    private Vector3 originSpawn = Vector3.zero;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private BasicKnightAttackController attackController;

    // PUBLIC

    public enum Estate { Idle = 0, Chase = 1, Attack = 3}
    public Estate actualState = Estate.Idle;

    // SERIALIZE
    [SerializeField] public float life = 100;
    [SerializeField] private float minDistanceFollow;

    [SerializeField] private float minDistanceStop;

    [Header("Knight Parameters")]
    [SerializeField] private float knockBackCooldown;
    [SerializeField] private float knockBackValue;

    [Header("Attack Parameters")]
    [SerializeField] private float damage;
    [SerializeField] private float firstAttackCooldown;
    [SerializeField] private float attackCooldown;
    private bool isInKnockBack = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        attackController = GetComponent<BasicKnightAttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = GameplayManager.instance.GetPlayerReference().transform;
        distanceOfPlayer = Vector3.Distance(transform.position, player.position);

        if(this.life <= 0)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            TargetPoint target = GetComponentInChildren<TargetPoint>();
            playerController.RemoveTarget(target);
            Destroy(this.gameObject);
        }

        ChangeStates();
        State(player);

    }
    private void ChangeStates()
    {
        if (distanceOfPlayer <= minDistanceStop)
        {
            actualState = Estate.Attack;
        }
        else if (distanceOfPlayer <= minDistanceFollow)
        {
            actualState = Estate.Chase;
        }
        else
        {
            actualState = Estate.Idle;
        }
    }
    private void State(Transform player)
    {
        switch (actualState)
        {
            case Estate.Idle:
                break;
            case Estate.Chase:
                Follow(player);
                break;
            case Estate.Attack:
                agent.isStopped = true;
                StartCoroutine(attackController.AttackPerformance());
                break;
        }
    }
    private void Follow(Transform playerTransform)
    {
        agent.isStopped = false;
        agent.destination = playerTransform.position;
    }
    public IEnumerator TakeKnockBack()
    {
        if (!isInKnockBack)
        {
            isInKnockBack = true;
            agent.isStopped = true;
            Transform player = GameplayManager.instance.GetPlayerReference().transform;
            Vector3 knockBackDirection = (this.transform.position - player.position).normalized;

            knockBackDirection.y = 0.5f;
            knockBackDirection = knockBackDirection.normalized;

            rb.AddForce(knockBackDirection * knockBackValue);
            yield return new WaitForSeconds(knockBackCooldown);
            isInKnockBack = false;
            agent.isStopped = false;
        }
    }
    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
