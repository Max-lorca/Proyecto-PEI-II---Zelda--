using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private bool isAttacking = false;
    private bool inputAttack = false;

    private PlayerController playerController;
    private RaycastHit[] attackHits = new RaycastHit[5];
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float radiusAttack = 3f;
    [Header("Referencias")]
    [SerializeField] private GameObject attackPrefab;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        attackPrefab.SetActive(false);
    }
    private void Update()
    {
        if(!isAttacking && inputAttack)
        {
            StartCoroutine(AttackPerformance());
        }
    }

    private IEnumerator AttackPerformance()
    {
        isAttacking = true;
        attackPrefab.SetActive(true);
        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for(int i = 0; i < hits; i++)
        {
            switch (attackHits[i].collider.gameObject.tag)
            {
                case "Enemy":
                    EnemyController enemy = attackHits[i].collider.gameObject.GetComponent<EnemyController>();
                    enemy.TakeDamage(this.damage);
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        attackPrefab.SetActive(false);
        inputAttack = false;
        isAttacking = false;
    }
    public void InputAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            inputAttack = true;
        }
    }
}
