using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    private float distanceOfPlayer;
    // PRIVATE 
    private FMC followController;
    private Vector3 originSpawn = Vector3.zero;

    private NavMeshAgent agent;

    // PUBLIC

    public enum Estate { Static = 0, Follow = 1, Patrol = 2, Attack = 3}
    public Estate actualState = Estate.Static;

    // SERIALIZE
    [SerializeField] public float life = 100;
    [SerializeField] private float minDistanceFollow = 10f;

    [SerializeField] private float minDistanceStop = 5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        followController = GetComponent<FMC>();
        agent = GetComponent<NavMeshAgent>();
        originSpawn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController player = GameplayManager.instance.GetPlayerReference();
        distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceOfPlayer <= minDistanceFollow)
        {
            agent.destination = player.transform.position;
        }
        else
        {
            agent.destination = originSpawn;
        }
    }

    private void States()
    {
        switch (actualState)
        {
            case Estate.Static:
                break;
            case Estate.Follow:
                break;
            case Estate.Patrol:
                break;
            case Estate.Attack:
                break;
        }
    }

    private void ChangeStates()
    {

    }

    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
