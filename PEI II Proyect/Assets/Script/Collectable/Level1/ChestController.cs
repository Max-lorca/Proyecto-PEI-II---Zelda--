using UnityEngine;

public class ChestController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                PlayerController player = GameplayManager.instance.GetPlayerReference();

                if (player.playerStats.haveSwordKey)
                {
                    player.playerStats.haveSword = true;
                    player.TryGetComponent(out AttackController attackController);
                    attackController.SwordPrefab.SetActive(true); 
                }
                break;
        }
    }
}
