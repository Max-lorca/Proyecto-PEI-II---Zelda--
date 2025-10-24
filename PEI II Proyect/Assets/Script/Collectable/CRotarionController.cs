using UnityEngine;

public class CRotationController : MonoBehaviour
{
    [SerializeField] float rotationVelocity;
    void Update()
    {
        transform.Rotate(Vector3.up * rotationVelocity * Time.deltaTime);
    }
}
