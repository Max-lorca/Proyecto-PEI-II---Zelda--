using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string spawnPointName;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();

            if (controller.playerStats.haveShield)
            {
                GameplayManager.instance.nextSpawnPoint = spawnPointName;
                SceneManager.LoadScene(sceneName); 
            }
        }
    }
}
