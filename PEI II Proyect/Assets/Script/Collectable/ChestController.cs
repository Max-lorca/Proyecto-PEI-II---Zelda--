using UnityEngine;

public class ChestController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                AttackController playerAttack = other.gameObject.GetComponent<AttackController>();
                playerAttack.haveSword = true;
                playerAttack.SwordPrefab.SetActive(true);
                break;
        }
    }
}
