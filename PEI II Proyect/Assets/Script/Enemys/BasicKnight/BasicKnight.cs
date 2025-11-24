using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class BasicKnight : MonoBehaviour
{
    // PRIVATE 
    private float distanceOfPlayer;

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

    [SerializeField] private int cantSepias = 5;

    [Header("Attack Parameters")]
    [HideInInspector] private bool isInKnockBack = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            GameplayManager.instance.DropItems(this.transform, cantSepias);

            TargetPoint selfTarget = GetComponentInChildren<TargetPoint>();
            Destroy(this.gameObject);

            Destroy(selfTarget.currentTarget.gameObject);
            Destroy(selfTarget.gameObject);
        }

        if (!isInKnockBack)
        {
            ChangeStates();
            State(player); 
        }

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
                if (!attackController.isAttacking)
                {
                    StartCoroutine(attackController.AttackPerformance()); 
                }
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
            agent.enabled = false;
            Transform player = GameplayManager.instance.GetPlayerReference().transform;
            Vector3 knockBackDirection = (this.transform.position - player.position).normalized;

            knockBackDirection.y = 0f;
            knockBackDirection = knockBackDirection.normalized;

            float elapsed = 0f;

            while (elapsed <= knockBackCooldown)
            {
                transform.position += knockBackDirection * knockBackValue * Time.deltaTime;

                elapsed += Time.deltaTime;
                yield return null;
            }

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
                transform.position = hit.position;

            isInKnockBack = false;
            agent.enabled = true;
        }
    }
    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
