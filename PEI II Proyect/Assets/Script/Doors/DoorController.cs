using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private bool isOpen = false;

    private Quaternion targetRotation;
    private Quaternion originalRotation;

    [SerializeField] private GameObject door;
    [SerializeField] private float rotationTarget = 120f;
    [SerializeField] private float rotationTime = 1f;

    private void Start()
    {
        originalRotation = door.transform.localRotation;

        Vector3 newRotationTarget = door.transform.localEulerAngles;
        newRotationTarget.y += rotationTarget;

        targetRotation = Quaternion.Euler(newRotationTarget);
    }

    public void Interact()
    {
        if (isOpen)
        {
            StartCoroutine(RotationPerformance(targetRotation, originalRotation));
        }
        else
        {
            StartCoroutine(RotationPerformance(originalRotation, targetRotation));
        }

        isOpen = !isOpen;
    }

    private IEnumerator RotationPerformance(Quaternion actualRotation, Quaternion targetRotation)
    {

        float elapsed = 0f;

        while(elapsed < rotationTime)
        {
            float t = elapsed / rotationTime;

            door.transform.localRotation = Quaternion.Lerp(actualRotation, targetRotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        door.transform.localRotation = targetRotation;

    }
}
