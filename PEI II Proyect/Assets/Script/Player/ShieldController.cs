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
    [Header("Variables")]
    [SerializeField] private float radius = 5;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        shieldPrefab.SetActive(false);
    }

    void Update()
    {
        if (inputShield)
        {
            ShieldPerformance();
        }
        else
        {
            shieldPrefab.SetActive(false);
        }
    }

    public void ShieldPerformance()
    {
        shieldPrefab.SetActive(true);
         Collider[] hitCount = Physics.OverlapSphere(shieldPrefab.transform.position, radius);

        for(int i = 0; i < hitCount.Length; i++)
        {
            switch (hitCount[i].gameObject.tag)
            {
                case "Proyectile":
                    GameObject proyectil = hitCount[i].gameObject.gameObject;
                    Destroy(proyectil);
                    break;
                case "BasicKnight":
                    BasicKnight basicKnight = hitCount[i].gameObject.gameObject.GetComponent<BasicKnight>();
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
