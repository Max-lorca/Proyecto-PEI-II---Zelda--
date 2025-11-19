using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour
{
    
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI sepiasText;

    [SerializeField] private RawImage hearthPrefab;
    [SerializeField] private Transform hearthContainer;
    [SerializeField] private List<RawImage> hearths;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarTextos();
        UpdateHearhts();
    }
    private void UpdateHearhts()
    {
        foreach(var h in hearths)
        {
            Destroy(h.gameObject);
        }
        hearths.Clear();

        for(int i = 0; i < playerStats.life; i++)
        {
            RawImage newHearth = Instantiate(hearthPrefab, hearthContainer);
            hearths.Add(newHearth);
        }
    }
    private void ActualizarTextos()
    {
        sepiasText.text = playerStats.sepias.ToString();
    }
}
