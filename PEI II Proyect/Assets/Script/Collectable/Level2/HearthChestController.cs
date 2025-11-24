using UnityEngine;

public class HearthChestController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            if (controller.playerStats.haveHearthKey)
            {
                controller.playerStats.AddMaxLife(1);
                controller.playerStats.AddLife(1);
                Destroy(this.gameObject);
            }
        }
    }
}
