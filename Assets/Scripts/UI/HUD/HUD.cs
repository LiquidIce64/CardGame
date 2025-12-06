using Characters;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
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

    public void GameOver()
    {
        if (gameOver) return;
        paused = false;
        gameOver = true;
        Time.timeScale = 0f;
        if (menuInstance != null) Destroy(menuInstance);
        menuInstance = Instantiate(gameOverScreen, transform);
    }
}
