using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;

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

    [SerializeField] private GameObject barra;
    [SerializeField] private GameObject barra1;
    [SerializeField] private GameObject barra2;

    [SerializeField] private Transform TargetBarra1;
    [SerializeField] private Transform TargetBarra2;
    [SerializeField] private Transform TargetBarra3;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        barra.SetActive(false);
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
            barra.SetActive(false);
            BarPosition(0f);
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
            barra.SetActive(true);
            BarPosition(2f);
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
        TargetPoint target = GameObject.FindWithTag("TargetPoint").GetComponent<TargetPoint>();

        if(Vector3.Distance(transform.position, target.transform.position) <= minTargetDistance)
        {
            lockTarget = target.transform;
            isTargetOnLocked = true;
        }
        else
        {
            lockTarget = this.gameObject.transform;
            isTargetOnLocked = false;
        }
    }

    private void BarPosition(float position)
    {
        Transform originalTransform1, originalTransform2;

        originalTransform1 = barra1.transform;
        originalTransform2 = barra2.transform;

        float targetTransform1 = originalTransform1.position.y - position;
        float targetTransform2 = originalTransform2.position.y + position;

        barra1.transform.position = new Vector2(0f,targetTransform1);
        barra2.transform.position = new Vector2(0f,targetTransform2);
    }

    public void OnLockIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isTargetLocked = !isTargetLocked;
        }
    }
}
