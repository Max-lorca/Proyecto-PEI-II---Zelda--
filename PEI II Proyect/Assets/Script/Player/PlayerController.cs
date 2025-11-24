using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;

    private Vector3 playerGravity;
    private Vector2 input;
    //Lock-in
    private bool isTargetLockedInput = false;
    private bool rotationTarget = false;

    [SerializeField] private float minTargetDistance = 8f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float mass = 5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public PlayerStats playerStats;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 8f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing = false;
    private float lasDashTime = -999f;
    private Vector3 dashDirection;

    [Header("Lock-on System")]
    [SerializeField] private Transform lockTarget;

    [SerializeField] public TargetPoint actualTarget;
    [SerializeField] public float searchRadius = 20f;
    [SerializeField] public LayerMask enemyLayer;

    [SerializeField] private GameObject barra1;
    [SerializeField] private GameObject barra2;

    [SerializeField] private Transform targetTransform1, targetTransform2;
    [SerializeField] private Transform originalTransform1, originalTransform2;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        string spawnName = GameplayManager.instance.nextSpawnPoint;

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawnObject = GameObject.Find(spawnName);

            if (spawnObject != null)
            {
                transform.position = spawnObject.transform.position;
                transform.rotation = spawnObject.transform.rotation;
            }

            GameplayManager.instance.nextSpawnPoint = "";
        }
        
    }

    void Update()
    {
        // Leer movimiento
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        // Aplicar gravedad
        if (characterController.isGrounded && playerGravity.y < 0)
            playerGravity.y = -2f;
        else
            playerGravity.y -= (gravity / mass) * Time.deltaTime;

        if (this.playerStats.life <= 0)
        {
            this.playerStats.ResetStats();
            GameplayManager.instance.ResetLevel();
        }


        // Target
        LockOn();
        // Movimiento
        HandleMovement();

        // Aplicar gravedad al final
        characterController.Move(playerGravity * Time.deltaTime);
    }

    void HandleMovement()
    {
        if (isDashing) return;

        if (input.magnitude < 0.1f) return;


        if (!isTargetLockedInput)
        {
            //camera.LookAt = this.gameObject.transform;
            barra1.transform.position = Vector3.Lerp(barra1.transform.position, originalTransform1.position, 2f * Time.deltaTime);
            barra2.transform.position = Vector3.Lerp(barra2.transform.position, originalTransform2.position, 2f * Time.deltaTime);
            //  Modo libre (tipo exploración)
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            Vector3 moveDir = (camRight * input.x + camForward * input.y).normalized;

            // Girar hacia la dirección de movimiento
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // Mover
            characterController.Move(moveDir * playerStats.speed * Time.deltaTime);
        }
        else if (lockTarget != null)
        {
            //camera.LookAt = lockTarget.transform;
            barra1.transform.position = Vector3.Lerp(barra1.transform.position, targetTransform1.position, 2f * Time.deltaTime);
            barra2.transform.position = Vector3.Lerp(barra2.transform.position, targetTransform2.position, 2f * Time.deltaTime);

            //Vector3 camDirection = (lockTarget.transform.position - cameraTransform.position).normalized;

            //cameraTransform.rotation = Quaternion.LookRotation(camDirection);
            //  Modo Z-Target
            Vector3 toTarget = lockTarget.position - transform.position;

            toTarget.y = 0f;

            Quaternion lookRot;

            // Mantener orientación hacia el enemigo o jugador

            if (rotationTarget)
            {
                lookRot = Quaternion.LookRotation(toTarget); 
            }
            else
            {
                lookRot = Quaternion.LookRotation(transform.forward);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

            // Movimiento relativo al personaje
            Vector3 moveDir = (transform.right * input.x + transform.forward * input.y).normalized;
            characterController.Move(moveDir * playerStats.speed * Time.deltaTime);
        }
    }

    private void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);

        if (hits.Length == 0)
        {
            actualTarget = null;
            return;
        }

        float closest = Mathf.Infinity;
        TargetPoint best = null;
        foreach(var h in hits)
        {
            TargetPoint tp = h.GetComponentInChildren<TargetPoint>();
            if (tp == null) continue;

            float dist = Vector3.Distance(transform.position, tp.transform.position);
            if(dist < closest)
            {
                closest = dist;
                best = tp;
            }
        }

        actualTarget = best;
    }

    private void LockOn()
    {
        if(actualTarget == null)
        {
            lockTarget = this.transform;
            rotationTarget = false;
            return;
        }

        float dist = Vector3.Distance(transform.position, actualTarget.transform.position);

        if(dist <= minTargetDistance)
        {
            lockTarget = actualTarget.transform;
            rotationTarget = true;
        }
        else
        {
            actualTarget = null;
            lockTarget = transform;
            rotationTarget = false;
        }
    }
    public void OnLockIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isTargetLockedInput = !isTargetLockedInput;

            if (isTargetLockedInput)
                FindClosestTarget();
            else
                actualTarget = null;
        }
    }

    private void TryDash()
    {
        if (isDashing) return;
        if (Time.time < lasDashTime + dashCooldown) return;

        isDashing = true;
        lasDashTime = Time.time;
        dashDirection = transform.forward;

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float elapsed = 0f;

        float dashSpeed = dashDistance / dashDuration;

        while(elapsed < dashDuration)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }
    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            TryDash();
        }
    }
    public void TakeDamage(int damage)
    {
        this.playerStats.life -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "SwordChestKey":
                playerStats.haveSwordKey = true;
                Destroy(other.gameObject);
                break;
            case "HearthChestKey":
                playerStats.haveHearthKey = true;
                Destroy(other.gameObject);
                break;
            
        }
    }
}
