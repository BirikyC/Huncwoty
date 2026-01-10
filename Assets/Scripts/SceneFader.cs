using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        if (!dialogueManager) return;
        dialogueManager.OnDialogueFinished += HandleDialogueFinished;
    }

    private void OnDisable()
    {
        if (!dialogueManager) return;
        dialogueManager.OnDialogueFinished -= HandleDialogueFinished;
    }

    public void LoadScene(string sceneName, bool shouldLoadLastDialogue = false)
    {
        if (shouldLoadLastDialogue)
        {
            dialogueManager.StartLastDialogue(sceneName);
        }
        else
        {
            StartCoroutine(FadeOut(sceneName));
        }
    }

    private void HandleDialogueFinished(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        fadeImage.color = fadeImage.color + new Color(0f, 0f, 0f, 1f);

        const float OFFSET_DURATION = 0.5f;
        yield return new WaitForSeconds(OFFSET_DURATION);

        float t = 1f;
        Color c = fadeImage.color;

        while(t > 0)
        {
            t -= Time.deltaTime / fadeDuration;
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;

        if (dialogueManager != null)
        {
            dialogueManager.StartFirstDialogue();
        }
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene(sceneName);
    }
}
