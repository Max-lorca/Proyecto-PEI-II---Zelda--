using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private bool isAttacking = false;
    private bool inputAttack = false;

    

    private PlayerController playerController;

    private int count = 0;


    [SerializeField] public GameObject SwordPrefab;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float radiusAttack = 3f;

    [SerializeField] public bool haveSword = false;
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

        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for(int i = 0; i < hits.Length; i++)
        {
            switch (hits[i]?.gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = hits[i].gameObject.GetComponent<BasicKnight>();
                    enemy.TakeKnockBack();
                    enemy.TakeDamage(this.damage);
                    break;
                case "Grass":
                    GrassController grass = hits[i].gameObject.GetComponent<GrassController>();
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


        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for(int i = 0; i< hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "BasicNight":
                    BasicKnight enemy = hits[i].gameObject.GetComponent<BasicKnight>();
                    enemy.TakeKnockBack();
                    enemy.TakeDamage(this.damage);
                    break;
                case "Grass":
                    GrassController grass = hits[i].gameObject.GetComponent<GrassController>();
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

        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for (int i = 0; i < hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = hits[i].gameObject.GetComponent<BasicKnight>();
                    enemy.TakeKnockBack();
                    enemy.TakeDamage(this.damage);
                    break;
                case "Grass":
                    GrassController grass = hits[i].gameObject.GetComponent<GrassController>();
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
