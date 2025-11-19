using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private PlayerController playerReference;

    [SerializeField] private GameObject sepiasPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            LoadPlayerReference();
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if(playerReference == null)
        {
            LoadPlayerReference();
        }
        else
        {
            return;
        }
    }

    public void InstantiateSepias(float cantidad, Transform posicion)
    {
        for (int i = 0; i <= cantidad; i++)
        {
            Instantiate(sepiasPrefab, posicion.position, sepiasPrefab.transform.rotation);
        }
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
