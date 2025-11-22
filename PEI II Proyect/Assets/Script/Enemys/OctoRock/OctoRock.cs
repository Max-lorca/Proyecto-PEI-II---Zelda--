using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class OctoRock : MonoBehaviour
{
    // PRIVATE 
    private float distanceOfPlayer;

    private NavMeshAgent agent;
    // PUBLIC

    public enum Estate { Idle = 0, Attack = 1 }
    public Estate actualState = Estate.Idle;

    // SERIALIZE

    [Header("References")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject shootPoint;

    [Header("Stats Parameters")]
    [SerializeField] public float life = 1;

    [SerializeField] private float minDistanceAttack;

    [SerializeField] private float cantSepias = 5f;

    [Header("Attack Parameters")]
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float rotationTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = GameplayManager.instance.GetPlayerReference().transform;
        distanceOfPlayer = Vector3.Distance(transform.position, player.position);

        if (this.life <= 0)
        {
            GameplayManager.instance.InstantiateSepias(cantSepias, this.transform);

            TargetPoint selfTarget = GetComponentInChildren<TargetPoint>();

            if (selfTarget.currentTarget != null)
            {
                Destroy(selfTarget.currentTarget.gameObject);
                Destroy(selfTarget.gameObject);

                Destroy(this.gameObject);
            }
        }
        
        ChangeStates();
        State(player);
    }
    private void ChangeStates()
    {
        if (distanceOfPlayer <= minDistanceAttack)
        {
            actualState = Estate.Attack;
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
                agent.isStopped = false;
                break;
            case Estate.Attack:
                agent.isStopped = true;
                AplyRotation(player);
                if (!isAttacking)
                {
                    StartCoroutine(AttackPerformance());
                }
                break;
        }
    }

    private void AplyRotation(Transform player)
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        Quaternion rotTarget = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget, rotationTime * Time.deltaTime);
    }

    private IEnumerator AttackPerformance()
    {
        isAttacking = true;
        var octoRock = Instantiate(rockPrefab, this.shootPoint.transform.position, transform.rotation);
        octoRock.GetComponent<Rock>().SetOrigin(this.transform);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
