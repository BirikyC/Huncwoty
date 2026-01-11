using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DialogueManager : MonoBehaviour
{
    public event Action<string> OnDialogueFinished;
    public event Action<string> OnDeathDialogueFinished;

    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private PlayerController player;
    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private List<string> startDialogueTexts = new List<string>();
    [SerializeField] private List<string> endDialogueTexts = new List<string>();
    [SerializeField] private List<string> dieDialogueTexts = new List<string>();
    private List<string> currentDialogueText;
    private int currentText = 0;

    private bool isActive = false;
    private bool isLastDialogue = false;
    private bool isDeathDialogue = false;

    private string nextSceneName = "";

    private void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player.OnDied += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        player.OnDied -= HandlePlayerDeath;
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
            else if (isDeathDialogue)
            {
                OnDeathDialogueFinished.Invoke(nextSceneName);
            }

                gameTimer.Continue();
            player.ToggleFreezeMovement(false);
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

        gameTimer.Stop();
        player.ToggleFreezeMovement(true);
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

        gameTimer.Stop();
        player.ToggleFreezeMovement(true);
    }

    private void HandlePlayerDeath()
    {
        currentDialogueText = dieDialogueTexts;
        isActive = true;

        dialogueCanvas.gameObject.SetActive(true);

        ShowText(currentDialogueText[0]);
        currentText = 1;

        isDeathDialogue = true;

        nextSceneName = "MainMenu";

        gameTimer.Stop();
        player.ToggleFreezeMovement(true);
    }
}
