using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    private bool isAttacking = false;
    private bool inputAttack = false;

    private PlayerController playerController;
    private AudioSource audioSource;

    private int count = 0;

    [SerializeField] public GameObject SwordPrefab;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float radiusAttack = 3f;

    [Header("Referencias")]

    [Header("Audio")]
    [SerializeField] private AudioClip slashAudio;

    [SerializeField] private ParticleSystem slashVFX;
    [SerializeField] private ParticleSystem slashVFX2;
    [SerializeField] private ParticleSystem slashVFX3;

    [SerializeField] private ParticleSystem sparkVFX;

    public enum Attack { A = 0, B = 1 , C = 2}
    public Attack actualAttack = Attack.A;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        SwordPrefab.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (playerController.playerStats.haveSword)
        {
            SwordPrefab.SetActive(true);
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.A && playerController.playerStats.haveSword)
        {
            audioSource.PlayOneShot(slashAudio);
            StartCoroutine(AttackAPerformance());
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.B && playerController.playerStats.haveSword)
        {
            audioSource.PlayOneShot(slashAudio);
            StartCoroutine(AttackBPerformance());
        }
        if(!isAttacking && inputAttack && actualAttack == Attack.C && playerController.playerStats.haveSword)
        {
            audioSource.PlayOneShot(slashAudio);
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
        slashVFX.Play();
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for(int i = 0; i < hits.Length; i++)
        {
            switch (hits[i]?.gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = hits[i].gameObject.GetComponent<BasicKnight>();
                    enemy.TakeDamage(this.damage);
                    StartCoroutine(enemy.TakeKnockBack());
                    sparkVFX.Play();
                    break;
                case "SlugBomb":
                    SlugBomb slugEnemy = hits[i].gameObject.GetComponent<SlugBomb>();
                    slugEnemy.TakeDamage(this.damage);
                    StartCoroutine(slugEnemy.TakeKnockBack());
                    sparkVFX.Play();
                    break;
                case "Grass":
                    GrassController grass = hits[i].gameObject.GetComponent<GrassController>();
                    GameplayManager.instance.DropItems(grass.transform, grass.maxSepiasAmount);
                    Destroy(grass.gameObject);
                    sparkVFX.Play();
                    break;
                case "OctoRock":
                    OctoRock octoRock = hits[i].gameObject.GetComponent<OctoRock>();
                    octoRock.TakeDamage(this.damage);
                    sparkVFX.Play();
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        inputAttack = false;
        isAttacking = false;
    }
    private IEnumerator AttackBPerformance()
    {
        isAttacking = true;
        slashVFX2.Play();


        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for(int i = 0; i< hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = hits[i].gameObject.GetComponent<BasicKnight>();
                    enemy.TakeDamage(this.damage);
                    StartCoroutine(enemy.TakeKnockBack());
                    sparkVFX.Play();
                    break;
                case "SlugBomb":
                    SlugBomb slugEnemy = hits[i].gameObject.GetComponent<SlugBomb>();
                    slugEnemy.TakeDamage(this.damage);
                    StartCoroutine(slugEnemy.TakeKnockBack());
                    sparkVFX.Play();
                    break;
                case "Grass":
                    GrassController grass = hits[i].gameObject.GetComponent<GrassController>();
                    GameplayManager.instance.DropItems(grass.transform, grass.maxSepiasAmount);
                    Destroy(grass.gameObject);
                    sparkVFX.Play();
                    break;
                case "OctoRock":
                    OctoRock octoRock = hits[i].gameObject.GetComponent<OctoRock>();
                    octoRock.TakeDamage(this.damage);
                    sparkVFX.Play();
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        inputAttack = false;
        isAttacking = false;

    }
    private IEnumerator AttackCPerformance()
    {
        isAttacking = true;
        slashVFX3.Play();

        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack);

        for (int i = 0; i < hits.Length; i++)
        {
            switch (hits[i].gameObject.tag)
            {
                case "BasicKnight":
                    BasicKnight enemy = hits[i].gameObject.GetComponent<BasicKnight>();
                    enemy.TakeDamage(this.damage);
                    StartCoroutine(enemy.TakeKnockBack());
                    sparkVFX.Play();
                    break;
                case "SlugBomb":
                    SlugBomb slugEnemy = hits[i].gameObject.GetComponent<SlugBomb>();
                    slugEnemy.TakeDamage(this.damage);
                    StartCoroutine(slugEnemy.TakeKnockBack());
                    sparkVFX.Play();
                    break;
                case "Grass":
                    GrassController grass = hits[i].gameObject.GetComponent<GrassController>();
                    GameplayManager.instance.DropItems(grass.transform, grass.maxSepiasAmount);
                    Destroy(grass.gameObject);
                    sparkVFX.Play();
                    break;
                case "OctoRock":
                    OctoRock octoRock = hits[i].gameObject.GetComponent<OctoRock>();
                    octoRock.TakeDamage(this.damage);
                    sparkVFX.Play();
                    break;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
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
