using Characters;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;
    private static HUD instance;
    private bool paused = false;
    private bool gameOver = false;
    private GameObject menuInstance;

    public static HUD Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Player.InputActions.UI.Pause.performed += _ => TogglePause();
    }

    public void TogglePause()
    {
        if (gameOver) return;
        if (paused)
        {
            paused = false;
            Time.timeScale = 1f;
            if (menuInstance != null) Destroy(menuInstance);
            return;
        }
        paused = true;
        Time.timeScale = 0f;
        menuInstance = Instantiate(pauseMenu, transform);
    }

    public void GameOver(bool isVictory = false)
    {
        if (gameOver) return;
        paused = false;
        gameOver = true;
        Time.timeScale = 0f;
        if (menuInstance != null) Destroy(menuInstance);
        var screen = isVictory ? victoryScreen : gameOverScreen;
        menuInstance = Instantiate(screen, transform);
    }
}
