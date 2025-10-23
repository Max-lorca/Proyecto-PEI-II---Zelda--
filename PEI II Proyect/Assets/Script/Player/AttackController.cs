using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private bool isAttacking = false;
    private bool inputAttack = false;

    private PlayerController playerController;

    private int count = 0;

    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float radiusAttack = 3f;
    [Header("Referencias")]
    [SerializeField] private GameObject attackPrefab;


    public enum Attack { A = 0, B = 1 , C = 2}
    public Attack actualAttack = Attack.A;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        attackPrefab.SetActive(false);
    }
    private void Update()
    {
        if(!isAttacking && inputAttack && actualAttack == Attack.A)
        {
            StartCoroutine(AttackAPerformance());
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.B)
        {
            StartCoroutine(AttackBPerformance());
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.C)
        {
            StartCoroutine(AttackCPerformance());
        }
        if(count >= 4)
        {
            count = 0;
        }

        if(count == 0)
        {
            actualAttack = Attack.A;
        }
        if(count == 1)
        {
            actualAttack = Attack.B;
        }
        if(count == 2)
        {
            actualAttack = Attack.C;
        }

        Debug.Log(count);
        Debug.Log(attackPrefab.transform.rotation);
    }

    private IEnumerator AttackAPerformance()
    {
        isAttacking = true;
        attackPrefab.SetActive(true);
        RaycastHit[] attackHits = new RaycastHit[5];

        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for(int i = 0; i < hits; i++)
        {
            switch (attackHits[i].collider?.gameObject.tag)
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
    private IEnumerator AttackBPerformance()
    {
        isAttacking = true;
        attackPrefab.SetActive(true);
        attackPrefab.transform.Rotate(Vector3.right, 25);


        RaycastHit[] attackHits = new RaycastHit[5];
        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for(int i = 0; i< hits; i++)
        {
            switch (attackHits[i].collider?.gameObject.tag)
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
    private IEnumerator AttackCPerformance()
    {
        isAttacking = true;
        attackPrefab.SetActive(true);
        attackPrefab.transform.Rotate(Vector3.right, -25);
        RaycastHit[] attackHits = new RaycastHit[5];
        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for (int i = 0; i < hits; i++)
        {
            switch (attackHits[i].collider?.gameObject.tag)
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
            count++;
        }
    }
}
