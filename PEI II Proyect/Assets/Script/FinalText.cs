using UnityEngine;

public class FinalText : MonoBehaviour
{
    [SerializeField] private float velocity = 10f;

    private void Update()
    {
        transform.position += transform.up * velocity * Time.deltaTime;
    }
}
