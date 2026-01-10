using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;

    public void PlayButton()
    {
        sceneFader.LoadScene("Level1");
    }

    public void SettingsButton()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
