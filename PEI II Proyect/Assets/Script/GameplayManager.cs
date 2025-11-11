using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private PlayerController playerReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadPlayerReference();
    }
    public void LoadPlayerReference()
    {
        playerReference = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public PlayerController GetPlayerReference()
    {
        return playerReference;
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
