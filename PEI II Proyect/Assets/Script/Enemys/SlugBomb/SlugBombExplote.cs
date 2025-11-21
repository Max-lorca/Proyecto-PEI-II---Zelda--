using System.Collections;
using UnityEngine;

public class SlugBombExplote : MonoBehaviour
{
    [HideInInspector] public bool isExploting = false;

    [SerializeField] private int explotionDamage = 1;
    [SerializeField] private float explotionRadius = 3f;
    [SerializeField] private float explotionTime;

    public IEnumerator Explotion()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explotionRadius);

        foreach(Collider hit in hits)
        {
            switch (hit.tag)
            {
                case "Player":
                    PlayerController player = hit.GetComponent<PlayerController>();
                    player.TakeDamage(this.explotionDamage);
                    break;
            }
        }
        yield return null;
    }
}
