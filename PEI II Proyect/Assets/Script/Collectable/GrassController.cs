using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GrassController : MonoBehaviour
{

    [Header("Referencias")]
    [SerializeField] private GameObject sepiaPrefab;
    [SerializeField] private GameObject hearthPrefab;

    [SerializeField] private int maxSepiasAmount = 5;
    [SerializeField] private float hearthChance = 0.3f; // 30% de probabilidad de que salga un corazon

    public float maxRotation = 180;

    private void Start()
    {
        float randomRotationY = Random.Range(0, maxRotation);
        transform.Rotate(Vector3.up * randomRotationY);
    }
    public void DropItems()
    {
        float r = Random.value;

        if (r <= hearthChance)
        {
            Instantiate(hearthPrefab, transform.position, hearthPrefab.transform.rotation);
            return;
        }

        int amount = Random.Range(1, maxSepiasAmount + 1);

        for (int i = 0; i < amount; i++)
        {
            Instantiate(sepiaPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
