using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;

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
        gameObject.SetActive(false);
        gameTimer.Continue();
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
