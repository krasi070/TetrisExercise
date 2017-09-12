using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUi;
    public GameObject hud;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        pauseUi.SetActive(!pauseUi.activeSelf);
        if (pauseUi.activeSelf)
        {
            Time.timeScale = 0f;
            hud.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            hud.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level");
        Toggle();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}