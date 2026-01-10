using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VectorGraphics;

public class DialogueManager : MonoBehaviour
{
    public event Action<string> OnDialogueFinished;

    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private List<string> startDialogueTexts = new List<string>();
    [SerializeField] private List<string> endDialogueTexts = new List<string>();
    private List<string> currentDialogueText;
    private int currentText = 0;

    private bool isActive = false;
    private bool isLastDialogue = false;

    private string nextSceneName = "";

    private void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isActive) return;

        if(currentText >= currentDialogueText.Count)
        {
            isActive = false;
            currentDialogueText.Clear();

            dialogueCanvas.gameObject.SetActive(false);

            if (isLastDialogue)
            {
                OnDialogueFinished.Invoke(nextSceneName);
            }
            return;
        }

        string text = currentDialogueText[currentText];
        currentText++;

        ShowText(text);
    }

    private void ShowText(string text)
    {
        dialogueText.text = text;
    }

    public void StartFirstDialogue()
    {
        currentDialogueText = startDialogueTexts;
        isActive = true;

        dialogueCanvas.gameObject.SetActive(true);

        ShowText(currentDialogueText[0]);
        currentText = 1;
    }

    public void StartLastDialogue(string sceneName)
    {
        currentDialogueText = endDialogueTexts;
        isActive = true;

        dialogueCanvas.gameObject.SetActive(true);

        ShowText(currentDialogueText[0]);
        currentText = 1;

        isLastDialogue = true;

        nextSceneName = sceneName;
    }
}
