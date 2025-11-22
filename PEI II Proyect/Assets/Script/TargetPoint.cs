using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TargetPoint : MonoBehaviour
{

    [HideInInspector] public bool isTargeting = false;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private float rotationTime;
    [SerializeField] private float rotationStopTime = 0.5f;

    [SerializeField] private float altura = 0.2f;
    [SerializeField] private float duracion = 1f;

    [SerializeField] private float cambioColorTime = 0.5f;
    [SerializeField] private Color originalColor;
    [SerializeField] private Color targetColor;

    [SerializeField] public GameObject currentTarget;

    [SerializeField] private float minDistanceTarget;

    void Start()
    {
        if (currentTarget == null)
        {
            currentTarget = Instantiate(targetPrefab, transform.position, Quaternion.identity);
            currentTarget.SetActive(false);

            Renderer rend = currentTarget.GetComponent<Renderer>();
            if (rend != null) originalColor = rend.material.color;

            StartCoroutine(RotationTarget(currentTarget));
            StartCoroutine(FlotingTarget(currentTarget)); 
        }
    }

    private void Update()
    {
        PlayerController player = GameplayManager.instance.GetPlayerReference();
        float distanceOfPlayer = Vector3.Distance(transform.position, player.transform.position);

        currentTarget.transform.position = transform.position;

        if(distanceOfPlayer <= minDistanceTarget)
        {
            currentTarget.SetActive(true);
        }
        else
        {
            currentTarget.SetActive(false);
        }

        if (isTargeting)
        {
            StartCoroutine(CambioColor(currentTarget, targetColor));

            Vector3 dir = (GameplayManager.instance.GetPlayerReference().transform.position - currentTarget.transform.position).normalized;
            currentTarget.transform.rotation = Quaternion.LookRotation(dir);
        }
        else
        {
            StartCoroutine(CambioColor(currentTarget, originalColor));
        }
    }
    private IEnumerator FlotingTarget(GameObject targetPrefab)
    {
        Vector3 posicionInicial = targetPrefab.transform.position;

        while (true)
        {
            yield return StartCoroutine(MoverVerticalmente(targetPrefab, posicionInicial.y, posicionInicial.y + altura, duracion));

            yield return StartCoroutine(MoverVerticalmente(targetPrefab, posicionInicial.y + altura, posicionInicial.y, duracion));
        }
        
    }

    private IEnumerator MoverVerticalmente(GameObject targetPrefab, float inicio, float fin, float time)
    {
        float elapsed = 0f;

        while(elapsed < duracion)
        {
            float t = elapsed / duracion;

            float y = Mathf.Lerp(inicio, fin, Mathf.SmoothStep(0f, 1f, t));

            targetPrefab.transform.position = new Vector3(transform.position.x, y, transform.position.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        targetPrefab.transform.position = new Vector3(transform.position.x, fin, transform.position.z);
    }

    private IEnumerator RotationTarget(GameObject targetPrefab)
    {
        while (true)
        {
            Quaternion rotInicial = targetPrefab.transform.rotation;
            Quaternion rotfinal = rotInicial * Quaternion.Euler(0, 360, 0);

            float elapsed = 0f;

            while(elapsed < rotationTime)
            {
                float t = elapsed / rotationTime;

                targetPrefab.transform.rotation = Quaternion.Slerp(rotInicial, rotfinal, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            targetPrefab.transform.rotation = rotfinal;

            yield return new WaitForSeconds(rotationStopTime);
        }
    }

    private IEnumerator CambioColor(GameObject targetPrefab, Color color)
    {
        float elapsed = 0f;

        Renderer sr = targetPrefab.GetComponent<Renderer>();
        

        while(elapsed < cambioColorTime)
        {
            sr.material.color = Color.Lerp(sr.material.color, color, elapsed / cambioColorTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.material.color = color;
    }

    private void OnDestroy()
    {
        if(currentTarget != null)
        {
            Destroy(currentTarget);
        }
    }

}
