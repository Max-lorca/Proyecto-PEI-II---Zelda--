using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SlugBombAttack : MonoBehaviour
{

    private bool isAttacking = false;
    private SlugBombExplote exploteController;

    [SerializeField] private float velocityAttack;
    [SerializeField] private float radiusAttack = 2;

    [SerializeField] private float initialAttackCooldown = 1.5f;
    [SerializeField] private float attackTime;

    private void Start()
    {
        exploteController = GetComponent<SlugBombExplote>();
    }

    public IEnumerator AttackPerformance(NavMeshAgent agent)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

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

        yield return null;

    }
}
