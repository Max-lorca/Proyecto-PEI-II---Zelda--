using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlugBomb : MonoBehaviour
{// PRIVATE 
    //REFERENCIAS
    private NavMeshAgent agent;
    private SlugBombExplote exploteController;

    private float distanceOfPlayer;

    private bool isInKnockBack = false;
    private bool isJumping = false;
    // PUBLIC

    public enum Estate {Chase = 1, Attack = 2}
    public Estate actualState = Estate.Chase;

    // SERIALIZE
    [SerializeField] public float life = 100;
    [SerializeField] private float minDistanceFollow;

    [SerializeField] private float minDistanceAttack;

    [Header("Knight Parameters")]
    [SerializeField] private float knockBackCooldown;
    [SerializeField] private float knockBackValue;

    [SerializeField] private float cantSepias = 5f;

    [Header("Attack Parameters")]
    [SerializeField] private float detectPlayerRadius = 2f;

    [Header("Jump Parameters")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private float height = 2.5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        exploteController = GetComponent<SlugBombExplote>();
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

            Destroy(selfTarget.currentTarget.gameObject);
            Destroy(selfTarget.gameObject);
            Destroy(this.gameObject);
        }
        if (!isInKnockBack && !isJumping)
        {
            ChangeStates();
            State(player);
        }

    }
    private void ChangeStates()
    {
        if (distanceOfPlayer <= minDistanceAttack)
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
                if (!isJumping)
                {
                    StartCoroutine(ParabolicJump(duration, height)); 
                }
                break;
        }
    }
    private void Follow(Transform playerTransform)
    {
        if (!agent.enabled) return;

        agent.SetDestination(playerTransform.position);
    }
    private IEnumerator ParabolicJump(float duration, float heigth)
    {
        isJumping = true;
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

            transform.position = position;

            Collider[] hits = Physics.OverlapSphere(transform.position, detectPlayerRadius);


            foreach(Collider hit in hits)
            {
                switch (hit.tag)
                {
                    case "Player":
                        if (!exploteController.isExploting)
                        {
                            StartCoroutine(exploteController.Explotion());
                        }
                    break;
                }
            }


            elapsed += Time.deltaTime;



            yield return null;
        }

        transform.position = end;

        agent.enabled = true;
        isJumping = false;
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

            while(elapsed <= knockBackCooldown)
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
