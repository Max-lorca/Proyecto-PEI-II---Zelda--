using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform FocusPoint;
    [SerializeField] private float sensibility = 1.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(FocusPoint);
    }
}
