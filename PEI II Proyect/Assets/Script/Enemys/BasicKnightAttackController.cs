using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicKnightAttackController : MonoBehaviour
{
    [HideInInspector] public bool isAttacking = false;


    [Header("Attack parameters")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float firstAttackCooldown = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [Range(0, 10)] private float radiusAttack = 3f;

    public IEnumerator AttackPerformance()
    {
        isAttacking = true;

        yield return new WaitForSeconds(firstAttackCooldown);
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for (int i = 0; i < hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "Player":
                    PlayerController player = hits[i].gameObject.GetComponent<PlayerController>();
                    player.TakeDamage(damage);
                    break;
                case "PlayerShield":
                    break;
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
