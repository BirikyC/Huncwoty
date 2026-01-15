using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private PauseManager pauseManager;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ToggleMenu(bool isPaused)
    {
        gameObject.SetActive(isPaused);

        if (isPaused)
        {
            gameTimer.Stop();
        }
        else
        {
            gameTimer.Continue();
        }
    }

    public void ResumeButton()
    {
        pauseManager.UnPause();
    }

    public void QuitButton()
    {

        gameTimer.Continue();
        sceneFader.LoadScene("MainMenu");
    }
}
