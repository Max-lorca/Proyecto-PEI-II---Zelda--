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
    //Ints

    //floats
    private float gravity = 9.81f;

    [Header("Variables")]
    [SerializeField] private float velocityMovement = 5f;
    [SerializeField] private float mass = 5f;

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
        Vector3 horizontalMovement = input.x * transform.right + input.y * transform.forward;

        if(characterController.isGrounded && playerGravity.y < 0)
        {
            playerGravity.y = -2f;
        }
        else
        {
            playerGravity.y -= (gravity / mass) * Time.deltaTime;
        }

        Vector3 newMovement = horizontalMovement + playerGravity;

        characterController.Move(newMovement * velocityMovement*Time.deltaTime);
    }
}
