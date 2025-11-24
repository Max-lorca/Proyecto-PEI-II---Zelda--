using UnityEngine;

public class BossChestController : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.playerStats.haveBossKey = true;
            Destroy(this.gameObject);
        }
    }
}
