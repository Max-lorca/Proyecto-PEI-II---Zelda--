using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private bool isAttacking = false;

    private RaycastHit[] attackHits = new RaycastHit[5];

    [Header("Attack parameters")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1.5f;
    [Range(0, 10)] private float radiusAttack = 3f;
    private IEnumerator AttackPerformance()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            int hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

            for(int i = 0; i < hits; i++)
            {
                switch (attackHits[i].collider.gameObject.tag)
                {
                    case "Player":
                        PlayerController player = attackHits[i].collider.gameObject.GetComponent<PlayerController>();
                        player.TakeDamage(damage);
                        break;
                }
            }
            
            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }
    }
}
