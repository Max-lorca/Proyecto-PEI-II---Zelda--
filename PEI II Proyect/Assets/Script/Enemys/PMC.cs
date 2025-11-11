using JetBrains.Annotations;
using UnityEngine;

public class PMC : MonoBehaviour
{
    // Patrol Movement Controller

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float minDistanceChangeWaypoint = 5f;
    [SerializeField] private float timeChangeWaypointCounter = 5f;
    private float timeChangeWaypoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
