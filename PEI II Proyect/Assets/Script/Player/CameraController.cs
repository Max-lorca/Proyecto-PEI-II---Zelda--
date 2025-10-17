using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private Transform FocusPoint;
    [SerializeField] private float sensibility = 0.7f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(FocusPoint);
    }
}
