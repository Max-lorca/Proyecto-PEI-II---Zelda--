using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    public void InteractAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            InteractionPreformance();
        }
    }
    private void InteractionPreformance()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);

        foreach(Collider hit in hits)
        {
            switch (hit.gameObject.tag)
            {
                case "NPC":
                    hit.TryGetComponent(out NPCController npc);
                    npc.Interact();
                    break;
                case "Door":
                    hit.TryGetComponent(out DoorController door);
                    door.Interact();
                    break;
            }
        }
    }

}
