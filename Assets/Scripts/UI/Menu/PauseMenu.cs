using Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public void Start()
        {
            Player.InputActions.UI.Restart.performed += _ => RestartGame();
        }

        public void Unpause()
        {
            HUD.Instance.TogglePause();
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            WaveController.Instance.enabled = false;
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
