using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private PlayerController playerReference;

    [SerializeField] public string nextSpawnPoint = "";

    [SerializeField] private GameObject sepiaPrefab;
    [SerializeField] private GameObject hearthPrefab;

    [SerializeField] private float hearthChance = 0.3f; // 30% de probabilidad de que salga un corazon
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
    public void DropItems(Transform target, int maxSepiasAmount)
    {
        float r = Random.value;

        if (r <= hearthChance)
        {
            Instantiate(hearthPrefab, target.position, hearthPrefab.transform.rotation);
            return;
        }

        int amount = Random.Range(1, maxSepiasAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            Instantiate(sepiaPrefab, target.position, Quaternion.identity);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPlayerReference();

        if (string.IsNullOrEmpty(nextSpawnPoint)) return;

        GameObject spawn = GameObject.Find(nextSpawnPoint);

        if(spawn != null && playerReference != null)
        {
            playerReference.transform.position = spawn.transform.position;
            playerReference.transform.rotation = spawn.transform.rotation;
        }

        nextSpawnPoint = "";
    }
}
