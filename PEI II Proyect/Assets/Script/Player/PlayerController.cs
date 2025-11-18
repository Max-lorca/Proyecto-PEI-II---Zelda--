using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;

    private List<TargetPoint> targets = new List<TargetPoint>();
    private int targetIndex;

    private Vector3 playerGravity;
    private Vector2 input;
    //Lock-in
    private bool isTargetLocked = false;
    private bool isTargetOnLocked = false;

    [SerializeField] private float minTargetDistance = 8f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float mass = 5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public PlayerStats playerStats;

    [Header("Lock-on System")]
    [SerializeField] private Transform lockTarget;

    [SerializeField] private GameObject barra1;
    [SerializeField] private GameObject barra2;

    [SerializeField] private Transform targetTransform1, targetTransform2;
    [SerializeField] private Transform originalTransform1, originalTransform2;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
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

        // Target
        TargetObject();
        // Movimiento
        HandleMovement();

        // Aplicar gravedad al final
        characterController.Move(playerGravity * Time.deltaTime);
    }

    void HandleMovement()
    {
        if (input.magnitude < 0.1f) return;

        if (!isTargetLocked)
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

            // Mantener orientación hacia el enemigo

            if (isTargetOnLocked)
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

    private void TargetObject()
    {
        if (targets.Count == 0)
        {
            lockTarget = transform;
            isTargetOnLocked = false;
            return;
        }

        targetIndex = Mathf.Clamp(targetIndex, 0, targets.Count - 1);

        TargetPoint target = targets[targetIndex];

        float dist = Vector2.Distance(transform.position, target.transform.position);

        if(dist <= minTargetDistance)
        {
            target.isTargeting = true;
            lockTarget = target.transform;
            isTargetOnLocked = true;
        }
        else
        {
            target.isTargeting = false;
            lockTarget = transform;
            isTargetOnLocked = false;
        }
    }

    public void AddTarget(TargetPoint target)
    {
        if (!targets.Contains(target))
        {
            targets.Add(target);
        }
    }
    public void RemoveTarget(TargetPoint target)
    {
        if (targets.Contains(target))
        {
            targets.Remove(target);
            if (targetIndex >= targets.Count) targetIndex = 0;
        }
    }
    public void ChangeTargetIndexLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && targets.Count > 0)
        {
            targetIndex++;
            if (targetIndex >= targets.Count)
                targetIndex = 0;
        }   
    }
    public void ChangeTargetIndexRight(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            targetIndex--;

            if (targetIndex < 0)
                targetIndex = targets.Count - 1;
        }
    }
    public void OnLockIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isTargetLocked = !isTargetLocked;
        }
    }
    public void TakeDamage(int damage)
    {
        this.playerStats.life -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            TargetPoint target = other.GetComponentInChildren<TargetPoint>();
            AddTarget(target);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            TargetPoint target = other.GetComponentInChildren<TargetPoint>();
            RemoveTarget(target);
        }
    }
}
