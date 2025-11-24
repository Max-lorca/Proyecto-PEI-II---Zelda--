using System.Collections;
using UnityEngine;

public class BarGateController : MonoBehaviour
{
    private Vector3 originPoint;

    [SerializeField] private GameObject barObject;
    [SerializeField] private Transform barTarget;

    private void Start()
    {
        originPoint = barObject.transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            if (controller.playerStats.haveBossKey)
            {
                barObject.transform.position = barTarget.position; 
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            if (controller.playerStats.haveBossKey)
            {
                barObject.transform.position = originPoint; 
            }
            
        }
    }

}
