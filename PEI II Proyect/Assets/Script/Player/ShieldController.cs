using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    private PlayerController playerController;
    private bool inputShield = false;
    [HideInInspector] public bool haveShield = false;
    
    [Header("Referencias")]
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private Transform shieldActivePosition;
    [SerializeField] private Transform shieldInactivePosition;
    [Header("Variables")]
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float distance = 2f;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        shieldPrefab.SetActive(false);
    }

    void Update()
    {
        shieldPrefab.SetActive(haveShield);
        if (inputShield)
        {
            ShieldPerformance();
        }
        else
        {
            shieldPrefab.transform.position = Vector3.Lerp(shieldPrefab.transform.position, shieldInactivePosition.position, 10f * Time.deltaTime);
        }
    }

    public void ShieldPerformance()
    {
        
        shieldPrefab.transform.position = Vector3.Lerp(shieldPrefab.transform.position, shieldActivePosition.transform.position, 10f * Time.deltaTime);

        RaycastHit[] hits = new RaycastHit[5];

        int hitCount = Physics.SphereCastNonAlloc(shieldPrefab.transform.position, radius, transform.forward, hits, distance);

        for(int i = 0; i < hitCount; i++)
        {
            switch (hits[i].collider.tag)
            {
                case "Proyectile":
                    GameObject proyectil = hits[i].collider.gameObject;
                    Destroy(proyectil);
                    break;
                case "BasicKnight":
                    BasicKnight basicKnight = hits[i].collider.gameObject.GetComponent<BasicKnight>();
                    basicKnight.TakeKnockBack();
                    break;
            }
        }
    }
    public void InputShield(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            inputShield = true;
        }
        if (ctx.canceled)
        {
            inputShield = false;
        }
    }
}
