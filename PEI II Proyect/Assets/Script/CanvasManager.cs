using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI sepiasText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarTextos();
    }

    private void ActualizarTextos()
    {
        sepiasText.text = "Sepias : " + playerStats.sepias.ToString();
    }
}
