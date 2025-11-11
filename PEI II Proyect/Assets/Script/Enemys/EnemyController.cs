using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    // PRIVATE 
    private FMC followController;
    private Vector3 originSpawn = Vector3.zero;

    private NavMeshAgent agent;

    // PUBLIC

    public enum Estate { Static = 0, Follow = 1, Patrol = 2}
    public Estate actualState = Estate.Static;

    // SERIALIZE
    [SerializeField] public float life = 100;
    [SerializeField] private float minDistanceFollow = 10f;



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
        float distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceOfPlayer <= minDistanceFollow)
        {
            agent.destination = player.transform.position;
        }
        else
        {
            agent.destination = originSpawn;
        }
    }

    public void TakeDamage(float damage)
    {
        this.life -= damage;
    }
}
