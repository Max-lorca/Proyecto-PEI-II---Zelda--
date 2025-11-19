using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private bool isAttacking = false;
    private bool inputAttack = false;

    [HideInInspector] public bool haveSword = false;

    private PlayerController playerController;

    private int count = 0;


    [SerializeField] public GameObject SwordPrefab;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float radiusAttack = 3f;
    [Header("Referencias")]
    [SerializeField] private GameObject SlashPrefab;


    public enum Attack { A = 0, B = 1 , C = 2}
    public Attack actualAttack = Attack.A;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        SwordPrefab.SetActive(false);
        SlashPrefab.SetActive(false);
    }
    private void Update()
    {

        if(!isAttacking && inputAttack && actualAttack == Attack.A && haveSword)
        {
            StartCoroutine(AttackAPerformance());
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.B && haveSword)
        {
            StartCoroutine(AttackBPerformance());
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.C && haveSword)
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
    }

    private IEnumerator AttackAPerformance()
    {
        isAttacking = true;
        SlashPrefab.SetActive(true);
        RaycastHit[] attackHits = new RaycastHit[5];

        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for(int i = 0; i < hits; i++)
        {
            switch (attackHits[i].collider?.gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = attackHits[i].collider.gameObject.GetComponent<BasicKnight>();
                    enemy.TakeKnockBack();
                    enemy.TakeDamage(this.damage);
                    break;
                case "Grass":
                    GrassController grass = attackHits[i].collider.gameObject.GetComponent<GrassController>();
                    GameplayManager.instance.InstantiateSepias(grass.randomSepias, grass.transform);
                    Destroy(grass.gameObject);
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        SlashPrefab.SetActive(false);
        inputAttack = false;
        isAttacking = false;
    }
    private IEnumerator AttackBPerformance()
    {
        isAttacking = true;
        SlashPrefab.SetActive(true);
        SlashPrefab.transform.Rotate(Vector3.right, 25);


        RaycastHit[] attackHits = new RaycastHit[5];
        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for(int i = 0; i< hits; i++)
        {
            switch (attackHits[i].collider?.gameObject.tag)
            {
                case "BasicNight":
                    BasicKnight enemy = attackHits[i].collider.gameObject.GetComponent<BasicKnight>();
                    enemy.TakeKnockBack();
                    enemy.TakeDamage(this.damage);
                    break;
                case "Grass":
                    GrassController grass = attackHits[i].collider.gameObject.GetComponent<GrassController>();
                    GameplayManager.instance.InstantiateSepias(grass.randomSepias, grass.transform);
                    Destroy(grass.gameObject);
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        SlashPrefab.SetActive(false);
        inputAttack = false;
        isAttacking = false;

    }
    private IEnumerator AttackCPerformance()
    {
        isAttacking = true;
        SlashPrefab.SetActive(true);
        SlashPrefab.transform.Rotate(Vector3.right, -25);
        RaycastHit[] attackHits = new RaycastHit[5];
        float hits = Physics.SphereCastNonAlloc(transform.position, radiusAttack, transform.forward, attackHits);

        for (int i = 0; i < hits; i++)
        {
            switch (attackHits[i].collider?.gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = attackHits[i].collider.gameObject.GetComponent<BasicKnight>();
                    enemy.TakeKnockBack();
                    enemy.TakeDamage(this.damage);
                    break;
                case "Grass":
                    GrassController grass = attackHits[i].collider.gameObject.GetComponent<GrassController>();
                    GameplayManager.instance.InstantiateSepias(grass.randomSepias, grass.transform);
                    Destroy(grass.gameObject);
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        SlashPrefab.SetActive(false);
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
