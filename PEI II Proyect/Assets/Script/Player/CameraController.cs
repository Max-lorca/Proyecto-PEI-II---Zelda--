using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -5f);
    [SerializeField] private Transform focusPoint;
    [SerializeField] private float sensibility = 0.7f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        focusPoint = playerController.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = playerController.transform.position + offset;
    }
}
