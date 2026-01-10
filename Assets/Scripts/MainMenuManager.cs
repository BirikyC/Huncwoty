using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        // Tymczasowo ustawione na SampleScene
        SceneManager.LoadScene("SampleScene");
    }

    public void SettingsButton()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
