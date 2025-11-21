using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlugBomb : MonoBehaviour
{// PRIVATE 
    //REFERENCIAS
    private NavMeshAgent agent;
    private SlugBombAttack attackController;

    private float distanceOfPlayer;

    private bool isInKnockBack = false;
    // PUBLIC

    public enum Estate {Chase = 1, Attack = 2}
    public Estate actualState = Estate.Chase;

    // SERIALIZE
    [SerializeField] public float life = 100;
    [SerializeField] private float minDistanceFollow;

    [SerializeField] private float minDistanceStop;

    [Header("Knight Parameters")]
    [SerializeField] private float knockBackCooldown;
    [SerializeField] private float knockBackValue;

    [Header("Attack Parameters")]
    [SerializeField] private float detectPlayerRadius = 2f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackController = GetComponent<SlugBombAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = GameplayManager.instance.GetPlayerReference().transform;
        distanceOfPlayer = Vector3.Distance(transform.position, player.position);

        if (this.life <= 0)
        {
            TargetPoint selfTarget = GetComponentInChildren<TargetPoint>();
            Destroy(this.gameObject);

            Destroy(selfTarget.currentTarget.gameObject);
            Destroy(selfTarget.gameObject);
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
    }
    private void State(Transform player)
    {
        switch (actualState)
        {
            case Estate.Chase:
                Follow(player);
                break;
            case Estate.Attack:
                agent.isStopped = true;
                break;
        }
    }
    private void Follow(Transform playerTransform)
    {
        agent.destination = playerTransform.position;
    }
    private IEnumerator ParabolicJump(float duration, float heigth)
    {
        agent.enabled = false;

        Transform player = GameplayManager.instance.GetPlayerReference().transform;

        Vector3 start = transform.position;
        Vector3 end = player.position;

        end.y = start.y;

        float elapsed = 0f;

        while(elapsed < duration)
        {
            float t = elapsed / duration;

            Vector3 position = Vector3.Lerp(start, end, t);

            float parabola = 4f * t * (1f - t);

            position.y += parabola * heigth;

            elapsed += Time.deltaTime;



            yield return null;
        }

        transform.position = end;

        agent.enabled = true;
    }


    public IEnumerator TakeKnockBack()
    {
        if (!isInKnockBack)
        {
            isInKnockBack = true;
            agent.enabled = false;
            Transform player = GameplayManager.instance.GetPlayerReference().transform;
            Vector3 knockBackDirection = (this.transform.position - player.position).normalized;

            knockBackDirection.y = 0.5f;
            knockBackDirection = knockBackDirection.normalized;

            float elapsed = 0f;

            while(elapsed <= knockBackCooldown)
            {
                transform.position += knockBackDirection * knockBackValue * Time.deltaTime;

                elapsed += Time.deltaTime;
                yield return null;
            }
            isInKnockBack = false;
            agent.enabled = true;
        }
    }
    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
