using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;

    private Vector3 playerGravity;
    private Vector2 input;

    [SerializeField] private float minTargetDistance = 8f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float mass = 5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public PlayerStats playerStats;

    [Header("Lock-on System")]
    [SerializeField] private Transform lockTarget;
    private bool isTargetLocked = false;

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
            //  Modo Z-Target
            Vector3 toTarget = lockTarget.position - transform.position;
            toTarget.y = 0f;

            // Mantener orientación hacia el enemigo
            Quaternion lookRot = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

            // Movimiento relativo al personaje
            Vector3 moveDir = (transform.right * input.x + transform.forward * input.y).normalized;
            characterController.Move(moveDir * playerStats.speed * Time.deltaTime);
        }
    }

    private void TargetObject()
    {
        TargetPoint target = GameObject.FindWithTag("TargetPoint").GetComponent<TargetPoint>();

        if(Vector3.Distance(transform.position, target.transform.position) <= minTargetDistance)
        {
            lockTarget = target.transform;
        }
        else
        {
            lockTarget = this.gameObject.transform;
        }
    }

    public void OnLockIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isTargetLocked = !isTargetLocked;
        }
    }
}
