using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;

    public void PlayButton()
    {
        sceneFader.LoadScene("Level2");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
