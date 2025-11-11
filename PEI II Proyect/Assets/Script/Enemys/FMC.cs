using UnityEngine;
using UnityEngine.AI;

public class FMC : MonoBehaviour
{
    // Follow Movement Controller

    private Vector3 originSpawn;

    [SerializeField] private float minDistanceFollow = 10f;

    public void Follow(NavMeshAgent agent)
    {
        PlayerController player = GameplayManager.instance.GetPlayerReference();
        float distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceOfPlayer <= minDistanceFollow)
        {
            agent.destination = player.transform.position;
        }
    }
}
