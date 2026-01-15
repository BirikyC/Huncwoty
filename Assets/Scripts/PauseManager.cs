using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private PlayerController player;

    private bool isPaused = false;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        isPaused = !isPaused;
        pauseMenu.ToggleMenu(isPaused);
        player.ToggleFreezeMovement(isPaused);
    }

    public void UnPause()
    {
        isPaused = false;
        pauseMenu.ToggleMenu(isPaused);
        player.ToggleFreezeMovement(isPaused);
    }
}
