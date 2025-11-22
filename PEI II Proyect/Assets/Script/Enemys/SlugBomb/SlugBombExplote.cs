using System.Collections;
using UnityEngine;

public class SlugBombExplote : MonoBehaviour
{
    [HideInInspector] public bool isExploting = false;

    [SerializeField] private int explotionDamage = 1;
    [SerializeField] private float explotionRadius = 3f;
    [SerializeField] private float explotionTime = 0.3f;

    public IEnumerator Explotion()
    {
        if (isExploting) yield break;

        isExploting = true;

        yield return new WaitForSeconds(explotionTime);
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

        Destroy(this.gameObject);
    }
}
