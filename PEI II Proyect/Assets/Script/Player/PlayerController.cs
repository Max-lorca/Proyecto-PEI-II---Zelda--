using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private JumpController jumpController;
    private AttackController attackController;
    private ShieldController shieldController;
    private PlayerInput playerInput;
    //Vectores
    private Vector3 playerGravity;
    private Vector2 input;
    private Vector3 moveDir;
    [HideInInspector] public Vector3 horizontalMovement;
    //Ints

    //Floats
    private float gravity = 9.81f;

    //Booleanos

    [Header("Movimiento")]
    [SerializeField] private float mass = 5f;
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private Transform cameraTransform;

    [Header("Stats")]
    [SerializeField] public PlayerStats playerStats; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        jumpController = GetComponent<JumpController>();   
        attackController = GetComponent<AttackController>();
        shieldController = GetComponent<ShieldController>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        if(input.magnitude >= 0.1f)
        {
            horizontalMovement = input.x * transform.right + input.y * transform.forward;
        }
        else
        {
            horizontalMovement = Vector3.zero;
        }
        if(characterController.isGrounded && playerGravity.y < 0)
        {
            playerGravity.y = -2f;
        }
        else
        {
            playerGravity.y -= (gravity / mass) * Time.deltaTime;
        }
        if(horizontalMovement.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.3f);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDir = Vector3.zero;
        }
        Vector3 newMovement = horizontalMovement + playerGravity + moveDir;
        characterController.Move(newMovement * playerStats.speed*Time.deltaTime);
    }
    public void TakeDamage(int damage)
    {
        playerStats.life -= damage;
    }
}
