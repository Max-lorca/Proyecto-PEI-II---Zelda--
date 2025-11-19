using UnityEngine;

public class GrassController : MonoBehaviour
{
    [SerializeField] private int maxSepias;
    [HideInInspector] public int randomSepias;
    [SerializeField] private float randomRotationY;
    public float maxRotation = 180;

    private void Start()
    {
        randomRotationY = Random.Range(0, maxRotation);
        transform.Rotate(Vector3.up * randomRotationY);
    }

    private void Update()
    {
        randomSepias = Random.Range(0, maxSepias);
    }
}
