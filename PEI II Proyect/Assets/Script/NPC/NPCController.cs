using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [Header("Diálogo del NPC")]
    [TextArea]
    [SerializeField] private string[] dialogueLines;

    [Header("Referencias UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;

    [SerializeField] private int price = 80;

    private bool isTalking = false;
    private bool isSell = false;
    private int dialogueIndex = 0;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
    public void Interact()
    {
        if (isSell) return;

        if (!isTalking)
            StartDialogue();
        else
            ContinueDialogue();
    }
    private void StartDialogue()
    {
        isTalking = true;
        dialogueIndex = 0;

        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[0];
    }

    private void ContinueDialogue()
    {
        dialogueIndex++;

        if(dialogueIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueLines[dialogueIndex];
    }

    private void EndDialogue()
    {
        isTalking = false;

        StartCoroutine(TrySellShield());
    }

    private IEnumerator TrySellShield()
    {
        if (playerStats.sepias <= price) 
            yield return null;
        else
        {
            dialogueText.text = "Escudo Vendido!!!";
            ShieldController shield = GameplayManager.instance.GetPlayerReference().GetComponent<ShieldController>();
            shield.haveShield = true;
            yield return new WaitForSeconds(1f);
            dialogueText.text = "";
            dialoguePanel.SetActive(false);
            isSell = true;

        }

    }
}
